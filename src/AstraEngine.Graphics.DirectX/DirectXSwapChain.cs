using AstraEngine.Platform;

namespace AstraEngine.Graphics.DirectX
{
    public sealed class DirectXSwapChain : ISwapChain
    {
        private readonly IWindow _window;

        public DirectXSwapChain(IWindow window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            Width = window.Width;
            Height = window.Height;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public void Present()
        {

        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Dispose() { }
    }
}
