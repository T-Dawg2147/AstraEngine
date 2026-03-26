using AstraEngine.Core;
using AstraEngine.Graphics;
using AstraEngine.Graphics.OpenGL;
using AstraEngine.Graphics.Software;
using AstraEngine.Input;
using AstraEngine.Math;
using AstraEngine.Platform;
using AstraEngine.Platform.Windows;
using AstraEngine.Scene;
using AstraEngine.Assets;

namespace AstraEngine.Editor;

public sealed class EditorApplication : IGameApplication
{
    private bool _initialized;
    private WindowsWindow? _window;
    private IRenderDevice? _device;
    private ISwapChain? _swapChain;
    private ICommandList? _commandList;
    private InputManager? _input;
    private Scene.Scene? _scene;
    private AssetManager? _assets;
    private EditorState? _editorState;
    private float _timeAccumulator;

    public void Initialize(EngineHost host)
    {
        _initialized = true;

        PlatformServices.Initialize(new WindowsPlatform(), new NullFileSystem());

        _window = (WindowsWindow)PlatformServices.Platform.CreateWindow(
            $"AstraEngine Editor - {host.Config.AppName}",
            host.Config.WindowWidth,
            host.Config.WindowHeight);

        _window.Resized += OnWindowResized;
        _window.Closing += OnWindowClosing;
        _window.MouseMoved += OnMouseMoved;
        _window.KeyChanged += OnKeyChanged;
        _window.MouseButtonChanged += OnMouseButtonChanged;
        _window.MouseScrolled += OnMouseScrolled;

        _input = new InputManager();
        _assets = new AssetManager();

        try
        {
            _window.InitializeOpenGl();
            _device = new OpenGLRenderDevice();
        }
        catch
        {
            _device = new SoftwareRenderDevice();
        }

        _swapChain = _device.CreateSwapChain(_window);
        _commandList = _device.CreateCommandList();

        // Give the software renderer access to assets for texture resolution
        if (_commandList is SoftwareCommandList softwareCmd)
        {
            softwareCmd.SetAssetManager(_assets);
        }

        _scene = BuildDefaultScene();

        _editorState = new EditorState
        {
            Scene = _scene,
            SelectedObject = _scene.Objects.FirstOrDefault()
        };

        Logger.Info("Editor initialized.");
    }

    public void Update(in EngineTime time)
    {
        if (!_initialized || _window is null || _swapChain is null || _commandList is null || _input is null || _scene is null)
        {
            return;
        }

        _window.PollEvents();
        _input.BeginFrame();

        var dt = (float)time.DeltaTime;

        // Editor camera: orbit, pan, zoom, fly-through
        UpdateEditorCamera(_input.Current, _scene.Camera, dt);

        UpdateScene(_scene, dt);

        _timeAccumulator += dt;

        // --- Unified render path ---
        _commandList.Begin();
        _commandList.ClearColor(_swapChain, new Color4(0.12f, 0.12f, 0.14f, 1f));

        var aspect = (float)_swapChain.Width / System.Math.Max(1, _swapChain.Height);

        // Set up lighting
        var ambientLight = _scene.Lights.Lights.OfType<AmbientLight>().FirstOrDefault();
        if (ambientLight is not null)
        {
            _commandList.SetAmbientLight(ambientLight);
        }

        var nonAmbientLights = _scene.Lights.Lights.Where(l => l is not AmbientLight);
        _commandList.SetLights(nonAmbientLights);

        // Draw all visible meshes
        foreach (var obj in _scene.Objects)
        {
            if (obj is MeshEntity meshEntity && meshEntity.Visible)
            {
                _commandList.DrawMesh(meshEntity.Mesh, _scene.Camera, aspect);
            }
        }

        _commandList.End();
        _swapChain.Present();

        var selectedName = _editorState?.SelectedName ?? "None";
        _window.SetTitle($"AstraEngine Editor | {_scene.Objects.Count} objects | Selected: {selectedName}");
        _input.EndFrame();
    }

    public void Shutdown()
    {
        _commandList?.Dispose();
        _swapChain?.Dispose();
        _device?.Dispose();

        if (_window is not null)
        {
            _window.MouseScrolled -= OnMouseScrolled;
            _window.MouseButtonChanged -= OnMouseButtonChanged;
            _window.KeyChanged -= OnKeyChanged;
            _window.MouseMoved -= OnMouseMoved;
            _window.Resized -= OnWindowResized;
            _window.Closing -= OnWindowClosing;
            _window.Dispose();
            _window = null;
        }
    }

    private Scene.Scene BuildDefaultScene()
    {
        var scene = new Scene.Scene();
        scene.Camera.Position = new Vector3(0f, 0f, -3f);

        var cube = new MeshEntity(new MeshInstance(Mesh.CreateCube()))
        {
            Visible = true,
            Material = new Material
            {
                Name = "EditorCubeMaterial",
                BaseColor = new Color4(0.3f, 0.7f, 1f, 1f),
                Roughness = 0.8f,
                Metallic = 0.0f,
                Opacity = 1.0f
            }
        };

        scene.Add(cube);

        scene.Lights.Add(new DirectionalLight
        {
            Direction = Vector3.Normalize(new Vector3(-0.5f, -1f, -0.25f)),
            Color = new Color4(1f, 1f, 1f, 1f),
            Intensity = 0.9f
        });

        scene.Lights.Add(new AmbientLight
        {
            Color = new Color4(0.15f, 0.15f, 0.18f, 1f),
            Intensity = 1.0f
        });

        return scene;
    }

    private void UpdateScene(Scene.Scene scene, float dt)
    {
        if (scene.Objects.FirstOrDefault() is MeshEntity meshEntity)
        {
            meshEntity.Mesh.Rotation = Quaternion.CreateFromYawPitchRoll(
                _timeAccumulator * 0.6f,
                _timeAccumulator * 0.35f,
                _timeAccumulator * 0.2f);
        }
    }

    /// <summary>
    /// Full editor camera: 
    ///   Right-click + drag = orbit (yaw/pitch)
    ///   Middle-click + drag = pan (translate right/up)
    ///   Scroll wheel = zoom (move along forward)
    ///   Right-click + WASD/QE = fly-through
    ///   No button = nothing
    /// </summary>
    private static void UpdateEditorCamera(InputState input, Camera camera, float dt)
    {
        const float orbitSensitivity = 0.15f;
        const float panSensitivity = 0.005f;
        const float zoomSpeed = 1.5f;
        const float flySpeed = 3.0f;

        var rightHeld = input.IsKeyDown(KeyCode.MouseRight);
        var middleHeld = input.IsKeyDown(KeyCode.MouseMiddle);

        var dx = input.MouseDeltaX;
        var dy = input.MouseDeltaY;

        // Right-click + drag = orbit (rotate yaw/pitch)
        if (rightHeld && (dx != 0f || dy != 0f))
        {
            camera.Yaw += dx * orbitSensitivity;
            camera.Pitch -= dy * orbitSensitivity;
            camera.Pitch = System.Math.Clamp(camera.Pitch, -89f, 89f);
        }

        // Middle-click + drag = pan (translate along camera right/up)
        if (middleHeld && (dx != 0f || dy != 0f))
        {
            var right = camera.Right;
            var up = camera.Up;
            camera.Position += (right * (-dx * panSensitivity)) + (up * (dy * panSensitivity));
        }

        // Scroll wheel = zoom (move along forward axis)
        if (input.ScrollDelta != 0f)
        {
            camera.Position += camera.Forward * (input.ScrollDelta * zoomSpeed);
        }

        // Right-click + WASD/QE = fly-through movement
        if (rightHeld)
        {
            var forward = camera.Forward;
            var right = camera.Right;
            var up = new Vector3(0f, 1f, 0f);
            var movement = Vector3.Zero;

            if (input.IsKeyDown(KeyCode.W) || input.IsKeyDown(KeyCode.Up))
                movement += forward;

            if (input.IsKeyDown(KeyCode.S) || input.IsKeyDown(KeyCode.Down))
                movement -= forward;

            if (input.IsKeyDown(KeyCode.D) || input.IsKeyDown(KeyCode.Right))
                movement += right;

            if (input.IsKeyDown(KeyCode.A) || input.IsKeyDown(KeyCode.Left))
                movement -= right;

            if (input.IsKeyDown(KeyCode.E))
                movement += up;

            if (input.IsKeyDown(KeyCode.Q))
                movement -= up;

            camera.Position += movement * (flySpeed * dt);
        }
    }

    private void OnKeyChanged(int virtualKeyCode, bool isDown)
    {
        if (_input is null)
            return;

        var engineKey = WindowsWindow.MapVirtualKey(virtualKeyCode);
        if (engineKey != KeyCode.Unknown)
        {
            _input.HandleKeyEvent(new KeyEvent(engineKey, isDown));
        }
    }

    private void OnMouseButtonChanged(int button, bool isDown)
    {
        if (_input is null)
            return;

        var engineKey = WindowsWindow.MapMouseButton(button);
        if (engineKey != KeyCode.Unknown)
        {
            _input.HandleMouseButton(new MouseButtonEvent(engineKey, isDown));
        }
    }

    private void OnMouseScrolled(float delta)
    {
        _input?.HandleMouseScroll(new MouseScrollEvent(delta));
    }

    private void OnMouseMoved(float dx, float dy)
    {
        _input?.HandleMouseMove(new MouseMoveEvent(dx, dy));
    }

    private void OnWindowResized(WindowResizeEvent evt)
    {
        _swapChain?.Resize(evt.Width, evt.Height);
    }

    private void OnWindowClosing(WindowCloseEvent evt)
    {

    }
}