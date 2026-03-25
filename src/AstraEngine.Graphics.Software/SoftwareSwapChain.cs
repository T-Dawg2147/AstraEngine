using AstraEngine.Platform;
using AstraEngine.Platform.Windows;

namespace AstraEngine.Graphics.Software
{
    public sealed class SoftwareSwapChain : ISwapChain
    {
        private readonly IWindow _window;
        private readonly SoftwareFrameBuffer _frameBuffer;

        public SoftwareSwapChain(IWindow window)
        {
            if (window is not WindowsWindow windowsWindow)
                throw new ArgumentException("Software backend requires a WindowsWindow.", nameof(window));

            _window = window ?? throw new ArgumentNullException(nameof(window));
            _frameBuffer = new SoftwareFrameBuffer(window.Width, window.Height);
            Width = window.Width;
            Height = window.Height;

            _window.Resized += OnResized;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public SoftwareFrameBuffer FrameBuffer => _frameBuffer;

        public void Present()
            => _frameBuffer.PresentToWindow(_window);

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
            _frameBuffer.Resize(width, height);
        }

        public void Dispose()
            => _window.Resized -= OnResized;

        private void OnResized(WindowResizeEvent e)
            => Resize(e.Width, e.Height);
    }
}
