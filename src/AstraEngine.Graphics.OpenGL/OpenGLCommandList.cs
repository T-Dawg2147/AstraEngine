using AstraEngine.Math;
using AstraEngine.Platform.Windows;
using AstraEngine.Scene;

namespace AstraEngine.Graphics.OpenGL
{
    public sealed class OpenGLCommandList : ICommandList
    {
        private bool _isRecording;
        private OpenGLSwapChain? _activeSwapChain;

        // Shader program
        private uint _program;
        private bool _initialized;

        // Uniform locations
        private int _locModel;
        private int _locView;
        private int _locProjection;
        private int _locBaseColor;
        private int _locOpacity;
        private int _locAmbientColor;
        private int _locAmbientIntensity;
        private int _locDirLightDirection;
        private int _locDirLightColor;
        private int _locDirLightIntensity;
        private int _locHasDirLight;

        // Per-mesh GPU resources
        private uint _vao;
        private uint _vbo;
        private uint _ebo;

        // Lighting state
        private readonly List<Light> _lights = [];
        private AmbientLight _ambientLight = new();

        public void Begin()
        {
            _isRecording = true;
        }

        public void End()
        {
            _isRecording = false;
            _activeSwapChain = null;
        }

        public void ClearColor(ISwapChain swapChain, Color4 color)
        {
            if (swapChain is not OpenGLSwapChain glSwapChain)
                return;

            if (glSwapChain.Window is not WindowsWindow)
                return;

            _activeSwapChain = glSwapChain;
            EnsureInitialized();

            var gl = OpenGLBindings.Instance;
            gl.Viewport(0, 0, glSwapChain.Width, glSwapChain.Height);
            gl.ClearColor(color.R, color.G, color.B, color.A);
            gl.Clear(OpenGLConstants.GL_COLOR_BUFFER_BIT | OpenGLConstants.GL_DEPTH_BUFFER_BIT);
        }

        public void SetLights(IEnumerable<Light> lights)
        {
            _lights.Clear();
            _lights.AddRange(lights);
        }

        public void SetAmbientLight(AmbientLight light)
        {
            _ambientLight = light;
        }

        public void DrawMesh(MeshInstance meshInstance, Camera camera, float aspectRatio)
        {
            if (!_isRecording || _activeSwapChain is null)
                return;

            var gl = OpenGLBindings.Instance;
            var mesh = meshInstance.Mesh;
            var material = meshInstance.Material;

            // Build vertex data: Position(3) + Normal(3) + Color(4) + UV(2) = 12 floats per vertex
            var floatsPerVertex = 12;
            var vertexData = new float[mesh.Vertices.Length * floatsPerVertex];

            for (var i = 0; i < mesh.Vertices.Length; i++)
            {
                var v = mesh.Vertices[i];
                var offset = i * floatsPerVertex;

                vertexData[offset + 0] = v.Position.X;
                vertexData[offset + 1] = v.Position.Y;
                vertexData[offset + 2] = v.Position.Z;

                vertexData[offset + 3] = v.Normal.X;
                vertexData[offset + 4] = v.Normal.Y;
                vertexData[offset + 5] = v.Normal.Z;

                vertexData[offset + 6] = v.Color.R;
                vertexData[offset + 7] = v.Color.G;
                vertexData[offset + 8] = v.Color.B;
                vertexData[offset + 9] = v.Color.A;

                vertexData[offset + 10] = v.UV.X;
                vertexData[offset + 11] = v.UV.Y;
            }

            // Upload vertex data
            gl.BindVertexArray(_vao);
            gl.BindBuffer(OpenGLConstants.GL_ARRAY_BUFFER, _vbo);
            unsafe
            {
                fixed (float* ptr = vertexData)
                    gl.BufferData(OpenGLConstants.GL_ARRAY_BUFFER,
                        (nuint)(vertexData.Length * sizeof(float)),
                        (nint)ptr,
                        OpenGLConstants.GL_STATIC_DRAW);
            }

            // Upload index data
            gl.BindBuffer(OpenGLConstants.GL_ELEMENT_ARRAY_BUFFER, _ebo);
            unsafe
            {
                fixed (int* ptr = mesh.Indices)
                    gl.BufferData(OpenGLConstants.GL_ELEMENT_ARRAY_BUFFER,
                        (nuint)(mesh.Indices.Length * sizeof(int)),
                        (nint)ptr,
                        OpenGLConstants.GL_STATIC_DRAW);
            }

            // Set up vertex attributes
            var stride = floatsPerVertex * sizeof(float);

            // Position: location 0, 3 floats, offset 0
            gl.VertexAttribPointer(0, 3, OpenGLConstants.GL_FLOAT, false, stride, 0);
            gl.EnableVertexAttribArray(0);

            // Normal: location 1, 3 floats, offset 3
            gl.VertexAttribPointer(1, 3, OpenGLConstants.GL_FLOAT, false, stride, 3 * sizeof(float));
            gl.EnableVertexAttribArray(1);

            // Color: location 2, 4 floats, offset 6
            gl.VertexAttribPointer(2, 4, OpenGLConstants.GL_FLOAT, false, stride, 6 * sizeof(float));
            gl.EnableVertexAttribArray(2);

            // UV: location 3, 2 floats, offset 10
            gl.VertexAttribPointer(3, 2, OpenGLConstants.GL_FLOAT, false, stride, 10 * sizeof(float));
            gl.EnableVertexAttribArray(3);

            // Use shader program
            gl.UseProgram(_program);

            // Set MVP matrices
            var model = meshInstance.WorldMatrix;
            var view = camera.ViewMatrix;
            var projection = camera.ProjectionMatrix(aspectRatio);

            SetUniformMatrix4(_locModel, model);
            SetUniformMatrix4(_locView, view);
            SetUniformMatrix4(_locProjection, projection);

            // Set material
            gl.Uniform4f(_locBaseColor,
                material.BaseColor.R,
                material.BaseColor.G,
                material.BaseColor.B,
                material.BaseColor.A);
            gl.Uniform1f(_locOpacity, material.Opacity);

            // Set ambient light
            gl.Uniform3f(_locAmbientColor,
                _ambientLight.Color.R,
                _ambientLight.Color.G,
                _ambientLight.Color.B);
            gl.Uniform1f(_locAmbientIntensity, _ambientLight.Intensity);

            // Set directional light (first one found, or disable)
            var dirLight = _lights.OfType<DirectionalLight>().FirstOrDefault();
            if (dirLight is not null && dirLight.Enabled)
            {
                gl.Uniform1i(_locHasDirLight, 1);
                gl.Uniform3f(_locDirLightDirection,
                    dirLight.Direction.X,
                    dirLight.Direction.Y,
                    dirLight.Direction.Z);
                gl.Uniform3f(_locDirLightColor,
                    dirLight.Color.R,
                    dirLight.Color.G,
                    dirLight.Color.B);
                gl.Uniform1f(_locDirLightIntensity, dirLight.Intensity);
            }
            else
            {
                gl.Uniform1i(_locHasDirLight, 0);
            }

            // Draw!
            gl.DrawElements(
                OpenGLConstants.GL_TRIANGLES,
                mesh.Indices.Length,
                OpenGLConstants.GL_UNSIGNED_INT,
                0);
        }

        public void Draw(int vertexCount, int startVertexLocation = 0)
        {
            // Kept for interface compatibility, but DrawMesh is now the primary path
            if (!_isRecording)
                return;

            var gl = OpenGLBindings.Instance;
            gl.DrawArrays(OpenGLConstants.GL_TRIANGLES, startVertexLocation, vertexCount);
        }

        public void DrawIndexed(int indexCount, int startIndexLocation = 0, int baseVertexLocation = 0)
        {
            if (!_isRecording)
                return;

            var gl = OpenGLBindings.Instance;
            gl.DrawElements(OpenGLConstants.GL_TRIANGLES, indexCount, OpenGLConstants.GL_UNSIGNED_INT, 0);
        }

        public void Dispose()
        {
            if (_initialized)
            {
                var gl = OpenGLBindings.Instance;
                gl.DeleteProgram(_program);
                gl.DeleteVertexArrays(1, ref _vao);
                gl.DeleteBuffers(1, ref _vbo);
                gl.DeleteBuffers(1, ref _ebo);
            }

            _isRecording = false;
        }

        private void EnsureInitialized()
        {
            if (_initialized)
                return;

            var gl = OpenGLBindings.Instance;

            // --- Compile vertex shader ---
            var vertexShader = gl.CreateShader(OpenGLConstants.GL_VERTEX_SHADER);
            gl.ShaderSource(vertexShader, VertexShaderSource);
            gl.CompileShader(vertexShader);
            gl.GetShaderiv(vertexShader, OpenGLConstants.GL_COMPILE_STATUS, out var vertexOk);
            if (vertexOk == 0)
                throw new InvalidOperationException(
                    $"Vertex shader compile failed: {gl.GetShaderInfoLog(vertexShader)}");

            // --- Compile fragment shader ---
            var fragmentShader = gl.CreateShader(OpenGLConstants.GL_FRAGMENT_SHADER);
            gl.ShaderSource(fragmentShader, FragmentShaderSource);
            gl.CompileShader(fragmentShader);
            gl.GetShaderiv(fragmentShader, OpenGLConstants.GL_COMPILE_STATUS, out var fragmentOk);
            if (fragmentOk == 0)
                throw new InvalidOperationException(
                    $"Fragment shader compile failed: {gl.GetShaderInfoLog(fragmentShader)}");

            // --- Link program ---
            _program = gl.CreateProgram();
            gl.AttachShader(_program, vertexShader);
            gl.AttachShader(_program, fragmentShader);
            gl.LinkProgram(_program);
            gl.GetProgramiv(_program, OpenGLConstants.GL_LINK_STATUS, out var linkOk);
            if (linkOk == 0)
                throw new InvalidOperationException(
                    $"Program link failed: {gl.GetProgramInfoLog(_program)}");

            gl.DeleteShader(vertexShader);
            gl.DeleteShader(fragmentShader);

            // --- Cache uniform locations ---
            _locModel = gl.GetUniformLocation(_program, "uModel");
            _locView = gl.GetUniformLocation(_program, "uView");
            _locProjection = gl.GetUniformLocation(_program, "uProjection");
            _locBaseColor = gl.GetUniformLocation(_program, "uBaseColor");
            _locOpacity = gl.GetUniformLocation(_program, "uOpacity");
            _locAmbientColor = gl.GetUniformLocation(_program, "uAmbientColor");
            _locAmbientIntensity = gl.GetUniformLocation(_program, "uAmbientIntensity");
            _locDirLightDirection = gl.GetUniformLocation(_program, "uDirLightDirection");
            _locDirLightColor = gl.GetUniformLocation(_program, "uDirLightColor");
            _locDirLightIntensity = gl.GetUniformLocation(_program, "uDirLightIntensity");
            _locHasDirLight = gl.GetUniformLocation(_program, "uHasDirLight");

            // --- Create GPU buffers ---
            gl.GenVertexArrays(1, out _vao);
            gl.GenBuffers(1, out _vbo);
            gl.GenBuffers(1, out _ebo);

            // --- Enable depth test and face culling ---
            gl.Enable(OpenGLConstants.GL_DEPTH_TEST);
            gl.DepthFunc(OpenGLConstants.GL_LEQUAL);
            gl.Enable(OpenGLConstants.GL_CULL_FACE);
            gl.CullFace(OpenGLConstants.GL_BACK);
            gl.FrontFace(OpenGLConstants.GL_CW);

            _initialized = true;
        }

        private void SetUniformMatrix4(int location, Matrix4x4 matrix)
        {
            // Your Matrix4x4 is row-major. GLSL expects column-major.
            // We pass transpose: true so OpenGL does the conversion for us.
            var data = new float[]
            {
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
            };

            var gl = OpenGLBindings.Instance;
            unsafe
            {
                fixed (float* ptr = data)
                    gl.UniformMatrix4fv(location, 1, true, (nint)ptr);
            }
        }

        // =====================================================================
        // GLSL Shader Sources
        // =====================================================================

        private const string VertexShaderSource = """
            #version 330 core

            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec3 aNormal;
            layout (location = 2) in vec4 aColor;
            layout (location = 3) in vec2 aUV;

            uniform mat4 uModel;
            uniform mat4 uView;
            uniform mat4 uProjection;

            out vec3 vWorldPosition;
            out vec3 vWorldNormal;
            out vec4 vColor;
            out vec2 vUV;

            void main()
            {
                vec4 worldPos = uModel * vec4(aPosition, 1.0);
                vWorldPosition = worldPos.xyz;
                vWorldNormal = mat3(uModel) * aNormal;
                vColor = aColor;
                vUV = aUV;
                gl_Position = uProjection * uView * worldPos;
            }
            """;

        private const string FragmentShaderSource = """
            #version 330 core

            in vec3 vWorldPosition;
            in vec3 vWorldNormal;
            in vec4 vColor;
            in vec2 vUV;

            uniform vec4 uBaseColor;
            uniform float uOpacity;

            uniform vec3 uAmbientColor;
            uniform float uAmbientIntensity;

            uniform int uHasDirLight;
            uniform vec3 uDirLightDirection;
            uniform vec3 uDirLightColor;
            uniform float uDirLightIntensity;

            out vec4 FragColor;

            void main()
            {
                vec3 normal = normalize(vWorldNormal);
                vec3 surfaceColor = vColor.rgb * uBaseColor.rgb;

                // Ambient
                vec3 result = surfaceColor * uAmbientColor * uAmbientIntensity;

                // Directional light
                if (uHasDirLight == 1)
                {
                    vec3 lightDir = normalize(-uDirLightDirection);
                    float ndotl = max(0.0, dot(normal, lightDir));
                    result += surfaceColor * uDirLightColor * uDirLightIntensity * ndotl;
                }

                result = min(result, vec3(1.0));
                FragColor = vec4(result, uOpacity);
            }
            """;
    }
}