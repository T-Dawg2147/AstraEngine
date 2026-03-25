using AstraEngine.Graphics.Abstractions;
using AstraEngine.Platform;

namespace AstraEngine.Graphics.Vulkan
{
    public sealed class VulkanRenderDevice : IRenderDevice
    {
        public GraphicsBackend Backend => GraphicsBackend.Vulkan;

        public ISwapChain CreateSwapChain(IWindow window)
            => new VulkanSwapChain(window);

        public IBuffer CreateBuffer(BufferDescription description)
            => new VulkanBuffer(description);

        public ITexture CreateTexture(TextureDescription description)
            => new VulkanTexture(description);

        public IShader CreateShader(ShaderDescription description)
            => new VulkanShader(description);

        public IPipeline CreatePipeline(PipelineDescription description)
            => new VulkanPipeline(description);

        public ICommandList CreateCommandList()
            => new VulkanCommandList();

        public void Dispose() { }
    }
}
