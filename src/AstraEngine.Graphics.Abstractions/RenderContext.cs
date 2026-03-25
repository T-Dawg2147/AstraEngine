namespace AstraEngine.Graphics
{
    public sealed class RenderContext
    {
        public RenderContext(IRenderDevice device, ISwapChain swapChain)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            SwapChain = swapChain ?? throw new ArgumentNullException(nameof(swapChain));
        }

        public IRenderDevice Device { get; }
        public ISwapChain SwapChain { get; }
    }
}
