using AstraEngine.Graphics;
using AstraEngine.Graphics.Abstractions;
using AstraEngine.Graphics.DirectX;
using AstraEngine.Platform;

namespace Astra.Graphics.DirectX
{
    public sealed class DirectXRenderDevice : IRenderDevice
    {
        public GraphicsBackend Backend => GraphicsBackend.DirectX;

        public ISwapChain CreateSwapChain(IWindow window)
            => new DirectXSwapChain(window);

        public IBuffer CreateBuffer(BufferDescription description)
            => new DirectXBuffer(description);

        public ITexture CreateTexture(TextureDescription description)
            => new DirectXTexture(description);

        public IShader CreateShader(ShaderDescription description)
            => new DirectXShader(description);

        public IPipeline CreatePipeline(PipelineDescription description)
            => new DirectXPipeline(description);

        public ICommandList CreateCommandList()
            => new DirectXCommandList();

        public void Dispose() { }
    }
}
