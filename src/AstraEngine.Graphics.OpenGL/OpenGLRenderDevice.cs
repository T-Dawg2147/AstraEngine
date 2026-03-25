using AstraEngine.Graphics.Abstractions;
using AstraEngine.Platform;

namespace AstraEngine.Graphics.OpenGL
{
    public class OpenGLRenderDevice : IRenderDevice
    {
        public GraphicsBackend Backend => GraphicsBackend.OpenGL;

        public ISwapChain CreateSwapChain(IWindow window)
            => new OpenGLSwapChain(window);

        public IBuffer CreateBuffer(BufferDescription description)
            => new OpenGLBuffer(description);

        public ITexture CreateTexture(TextureDescription description)
            => new OpenGLTexture(description);

        public IShader CreateShader(ShaderDescription description)
            => new OpenGLShader(description);

        public IPipeline CreatePipeline(PipelineDescription description)
            => new OpenGLPipeline(description);

        public ICommandList CreateCommandList()
            => new OpenGLCommandList();

        public void Dispose() { }
    }
}
