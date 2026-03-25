using AstraEngine.Math;
using AstraEngine.Platform.Windows;

namespace AstraEngine.Graphics.OpenGL
{
    public sealed class OpenGLCommandList : ICommandList
    {
        private bool _isRecording;
        private uint _program;
        private uint _vao;
        private uint _vbo;
        private bool _initialized;
        private OpenGLSwapChain? _activeSwapChain;

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

            if (glSwapChain.Window is not WindowsWindow window)
                return;

            _activeSwapChain = glSwapChain;
            EnsureInitialized(glSwapChain);

            var gl = OpenGLBindings.Instance;
            gl.Viewport(0, 0, glSwapChain.Width, glSwapChain.Height);
            gl.ClearColor(color.R, color.G, color.B, color.A);
            gl.Clear(OpenGLConstants.GL_COLOR_BUFFER_BIT | OpenGLConstants.GL_DEPTH_BUFFER_BIT);
            window.SwapOpenGlBuffers();
        }

        public void Draw(int vertexCount, int startVertexLocation = 0)
        {
            if (!_isRecording)
                return;

            var gl = OpenGLBindings.Instance;
            gl.UseProgram(_program);
            gl.BindVertexArray(_vao);
            gl.DrawArrays(OpenGLConstants.GL_TRANGLES, startVertexLocation, vertexCount);
        }

        public void DrawIndexed(int indexCount, int startIndexLocation = 0, int baseVertexLocation = 0)
        {
            Draw(indexCount, baseVertexLocation);
        }

        public void Dispose() 
        {
            _isRecording = false;
        }

        private void EnsureInitialized(OpenGLSwapChain swapChain)
        {
            if (_initialized)
                return;

            var gl = OpenGLBindings.Instance;

            var vertexShader = gl.CreateShader(OpenGLConstants.GL_VERTEX_SHADER);
            gl.ShaderSource(vertexShader, """
                #version 330 core
                layout (location = 0) in vec3 aPos;
                layout (location = 1) in vec3 aColor;
                out vec3 vColor;
                void main()
                {
                    gl_Position = vec4(aPos, 1.0);
                    vColor = aColor;
                }
                """);
            gl.CompileShader(vertexShader);
            gl.GetShaderiv(vertexShader, OpenGLConstants.GL_COMPILE_STATUS, out var vertexOk);
            if (vertexOk == 0)
                throw new InvalidOperationException($"Vertex shader compile failed: {gl.GetShaderInfoLog(vertexShader)}");

            var fragmentShader = gl.CreateShader(OpenGLConstants.GL_FRAGMENT_SHADER);
            gl.ShaderSource(fragmentShader, """
                #version 330 core
                in vec3 vColor;
                out vec4 FragColor;
                void main()
                {
                    FragColor = vec4(vColor, 1.0);
                }
                """);
            gl.CompileShader(fragmentShader);
            gl.GetShaderiv(fragmentShader, OpenGLConstants.GL_COMPILE_STATUS, out var fragmentOk);
            if (fragmentOk == 0)
                throw new InvalidOperationException($"Fragment shader compile failed: {gl.GetShaderInfoLog(fragmentShader)}");

            _program = gl.CreateProgram();
            gl.AttachShader(_program, vertexShader);
            gl.AttachShader(_program, fragmentShader);
            gl.LinkProgram(_program);
            gl.GetProgramiv(_program, OpenGLConstants.GL_LINK_STATUS, out var linkOk);
            if (linkOk == 0)
                throw new InvalidOperationException($"Program link failed: {gl.GetProgramInfoLog(_program)}");

            gl.DeleteShader(vertexShader);
            gl.DeleteShader(fragmentShader);

            gl.GenVertexArrays(1, out _vao);
            gl.GenBuffers(1, out _vbo);

            gl.BindVertexArray(_vao);
            gl.BindBuffer(OpenGLConstants.GL_ARRAY_BUFFER, _vbo);

            var vertices = new float[]
            {
                0.0f, 0.5f, 0.0f, 1f, 0f, 0f,
                -0.5f, -0.5f, 0.0f, 0f, 1f, 0f,
                0.5f, -0.5f, 0.0f, 0f, 0f, 1f
            };

            unsafe
            {
                fixed (float* ptr = vertices)
                    gl.BufferData(OpenGLConstants.GL_ARRAY_BUFFER, (nuint)(vertices.Length * sizeof(float)), (nint)ptr, OpenGLConstants.GL_STATIC_DRAW);
            }

            gl.VertexAttribPointer(0, 3, OpenGLConstants.GL_FLOAT, false, 6 * sizeof(float), 0);
            gl.EnableVertexAttribArray(0);
            gl.VertexAttribPointer(1, 3, OpenGLConstants.GL_FLOAT, false, 6 * sizeof(float), 3 * sizeof(float));
            gl.EnableVertexAttribArray(1);

            gl.UseProgram(_program);
            gl.BindVertexArray(_vao);
            gl.Enable(0x0B71);
            gl.DepthFunc(0x0203);

            _initialized = true;
        }
    }
}
