using AstraEngine.Assets;
using AstraEngine.Core;
using AstraEngine.Graphics;
using AstraEngine.Graphics.DirectX;
using AstraEngine.Graphics.OpenGL;
using AstraEngine.Graphics.Software;
using AstraEngine.Graphics.Vulkan;
using AstraEngine.Input;
using AstraEngine.Math;
using AstraEngine.Platform;
using AstraEngine.Platform.Windows;
using AstraEngine.Scene;
using AstraEngine.Threading;

namespace AstraEngine.Sandbox;

public sealed class SandboxApplication : IGameApplication
{
    private bool _initialized;
    private WindowsWindow? _window;
    private InputManager? _input;
    private IRenderDevice? _device;
    private ISwapChain? _swapChain;
    private ICommandList? _commandList;
    private AssetManager? _assets;
    private ContentDatabase? _contentDatabase;
    private ContentPipeline? _contentPipeline;
    private Scene.Scene? _scene;
    private readonly DirectionalLight _directionalLight = new();
    private readonly PointLight _pointLight = new();
    private readonly SpotLight _spotLight = new();
    private float _timeAccumulator;
    private JobScheduler? _jobs;
    private Dispatcher? _dispatcher;
    private FrameTimer? _frameTimer;
    private AppSettings _settings = new();

    public void Initialize(EngineHost host)
    {
        _initialized = true;

        _settings = SettingsManager.Load(Paths.SettingsFile);
        _settings.AppName = host.Config.AppName;

        PlatformServices.Initialize(new WindowsPlatform(), new NullFileSystem());

        _window = (WindowsWindow)PlatformServices.Platform.CreateWindow(
            _settings.AppName,
            _settings.WindowWidth,
            _settings.WindowHeight);

        _window.Resized += OnWindowResized;
        _window.Closing += OnWindowClosing;
        _window.MouseMoved += OnMouseMoved;

        _input = new InputManager();
        _assets = new AssetManager();
        _contentDatabase = new ContentDatabase();
        _contentPipeline = new ContentPipeline(_assets, _contentDatabase);
        _jobs = new JobScheduler();
        _dispatcher = new Dispatcher();
        _frameTimer = new FrameTimer();

        try
        {
            _window.InitializeOpenGl();
            _device = new OpenGLRenderDevice();
            _settings.GraphicsBackend = "OpenGL";
        }
        catch
        {
            _device = new SoftwareRenderDevice();
            _settings.GraphicsBackend = "Software";
        }

        _swapChain = _device.CreateSwapChain(_window);
        _commandList = _device.CreateCommandList();

        _scene = new Scene.Scene();
        _scene.Camera.Position = new Vector3(0f, 0f, -3f);

        var mesh = LoadMesh();
        var material = LoadMaterial();

        var entity = new MeshEntity(mesh)
        {
            Visible = true,
            Material = material
        };

        _scene.Add(entity);

        _directionalLight.Direction = Vector3.Normalize(new Vector3(-0.5f, -1f, -0.25f));
        _directionalLight.Color = new Color4(1f, 1f, 1f, 1f);
        _directionalLight.Intensity = 0.7f;

        _pointLight.Position = new Vector3(1f, 1f, -1f);
        _pointLight.Color = new Color4(1f, 0.8f, 0.6f, 1f);
        _pointLight.Intensity = 1.0f;
        _pointLight.Range = 6f;
        _pointLight.Attenuation = 1.5f;

        _spotLight.Position = new Vector3(-1.5f, 1.5f, -2f);
        _spotLight.Direction = Vector3.Normalize(new Vector3(0.5f, -0.8f, 1f));
        _spotLight.Color = new Color4(0.6f, 0.7f, 1f, 1f);
        _spotLight.Intensity = 0.9f;
        _spotLight.InnerConeAngle = 12f;
        _spotLight.OuterConeAngle = 24f;
        _spotLight.Range = 8f;

        _scene.Lights.Add(_directionalLight);
        _scene.Lights.Add(_pointLight);
        _scene.Lights.Add(_spotLight);

        if (_commandList is SoftwareCommandList software)
        {
            software.SetLights(_scene.Lights.Lights);
            software.SetAmbientLight(new AmbientLight
            {
                Color = new Color4(0.18f, 0.18f, 0.18f, 1f),
                Intensity = 1.0f
            });
        }

        Logger.Info($"Started with backend: {_settings.GraphicsBackend}");
    }

    public void Update(in EngineTime time)
    {
        if (!_initialized || _window is null || _swapChain is null || _commandList is null || _input is null || _scene is null)
        {
            return;
        }

        _frameTimer?.Tick();
        _dispatcher?.Pump();

        _window.PollEvents();
        _input.BeginFrame();

        UpdateCamera(_input.Current, _scene.Camera, (float)time.DeltaTime);
        _scene.Update((float)time.DeltaTime);

        _timeAccumulator += (float)time.DeltaTime;

        if (_scene.Objects.FirstOrDefault() is MeshEntity meshEntity)
        {
            meshEntity.Mesh.Rotation = Quaternion.CreateFromYawPitchRoll(_timeAccumulator * 0.8f, _timeAccumulator * 0.5f, _timeAccumulator * 0.2f);
        }

        _pointLight.Position = new Vector3(System.MathF.Sin(_timeAccumulator) * 1.5f, 1f, -1f);
        _spotLight.Position = new Vector3(-1.5f, 1.5f, -2f);

        var r = 0.10f + 0.10f * System.MathF.Sin(_timeAccumulator * 1.5f);
        var g = 0.12f + 0.10f * System.MathF.Sin(_timeAccumulator * 0.9f + 1.0f);
        var b = 0.18f + 0.10f * System.MathF.Sin(_timeAccumulator * 1.2f + 2.0f);

        _commandList.Begin();
        _commandList.ClearColor(_swapChain, new Color4(r, g, b, 1f));

        if (_commandList is SoftwareCommandList software)
        {
            var aspect = (float)_swapChain.Width / System.MathF.Max(1, _swapChain.Height);

            foreach (var obj in _scene.Objects)
            {
                if (obj is MeshEntity meshEntityObj && meshEntityObj.Visible)
                {
                    software.DrawMesh(meshEntityObj.Mesh, _scene.Camera, aspect);
                }
            }
        }
        else if (_commandList is OpenGLCommandList gl)
        {
            gl.Draw(3);
        }

        _commandList.End();
        _swapChain.Present();

        _window.SetTitle($"{_window.Title} | t={time.TotalTime:0.00}s");
        _input.EndFrame();
    }

    public void Shutdown()
    {
        if (_window is not null)
        {
            _settings.WindowWidth = _window.Width;
            _settings.WindowHeight = _window.Height;
            SettingsManager.Save(Paths.SettingsFile, _settings);
        }

        _jobs?.Dispose();
        _commandList?.Dispose();
        _swapChain?.Dispose();
        _device?.Dispose();

        if (_window is not null)
        {
            _window.MouseMoved -= OnMouseMoved;
            _window.Resized -= OnWindowResized;
            _window.Closing -= OnWindowClosing;
            _window.Dispose();
            _window = null;
        }
    }

    private MeshInstance LoadMesh()
    {
        try
        {
            if (_contentPipeline is not null)
            {
                var meshAsset = _contentPipeline.Import<MeshAsset>(Path.Combine(Paths.AssetsDirectory, "model.obj"));
                return new MeshInstance(meshAsset.Mesh);
            }
        }
        catch
        {
        }

        return new MeshInstance(Mesh.CreateCube());
    }

    private Material LoadMaterial()
    {
        return new Material
        {
            Name = "DefaultMaterial",
            BaseColor = new Color4(0.85f, 0.65f, 0.25f, 1f),
            Metallic = 0.0f,
            Roughness = 0.9f,
            Opacity = 1.0f
        };
    }

    private void UpdateCamera(InputState input, Camera camera, float dt)
    {
        const float speed = 3.0f;

        var forward = camera.Forward;
        var right = camera.Right;
        var movement = Vector3.Zero;

        if (input.IsKeyDown(KeyCode.W) || input.IsKeyDown(KeyCode.Up))
        {
            movement += forward;
        }

        if (input.IsKeyDown(KeyCode.S) || input.IsKeyDown(KeyCode.Down))
        {
            movement -= forward;
        }

        if (input.IsKeyDown(KeyCode.D) || input.IsKeyDown(KeyCode.Right))
        {
            movement += right;
        }

        if (input.IsKeyDown(KeyCode.A) || input.IsKeyDown(KeyCode.Left))
        {
            movement -= right;
        }

        camera.Position += movement * (speed * dt);
    }

    private void OnMouseMoved(float dx, float dy)
    {
        if (_scene is null)
        {
            return;
        }

        _scene.Camera.Yaw += dx * 0.10f;
        _scene.Camera.Pitch -= dy * 0.10f;
        _scene.Camera.Pitch = System.Math.Clamp(_scene.Camera.Pitch, -89f, 89f);
    }

    private void OnWindowResized(WindowResizeEvent evt)
    {
        _swapChain?.Resize(evt.Width, evt.Height);
    }

    private void OnWindowClosing(WindowCloseEvent evt)
    {
    }
}