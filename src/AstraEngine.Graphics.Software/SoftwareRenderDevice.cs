using AstraEngine.Graphics.Abstractions;
using AstraEngine.Platform;

namespace AstraEngine.Graphics.Software
{
    public sealed class SoftwareRenderDevice : IRenderDevice
    {
        public GraphicsBackend Backend => GraphicsBackend.Software;

        public ISwapChain CreateSwapChain(IWindow window)
            => new SoftwareSwapChain(window);

        public IBuffer CreateBuffer(BufferDescription description)
            => new SoftwareBuffer(description);

        public ITexture CreateTexture(TextureDescription description)
            => new SoftwareTexture(description);

        public IShader CreateShader(ShaderDescription description)
            => new SoftwareShader(description);

        public IPipeline CreatePipeline(PipelineDescription description)
            => new SoftwarePipeline(description);

        public ICommandList CreateCommandList()
            => new SoftwareCommandList();

        public void Dispose() { }
    }
}
