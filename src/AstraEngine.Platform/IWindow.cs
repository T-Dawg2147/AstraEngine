namespace AstraEngine.Platform
{
    public enum WindowState
    {
        Normal,
        Minimized,
        Maximized,
        Fullscreen
    }

    public readonly struct WindowResizeEvent
    {
        public WindowResizeEvent(int width, int height)
        {
            Width = width; Height = height;
        }

        public int Width { get; }
        public int Height { get; }
    }

    public readonly struct WindowCloseEvent { }

    public interface IWindow : IDisposable
    {
        string Title { get; set; }
        int Width { get; }
        int Height { get; }
        WindowState State { get; }
        bool IsOpen { get; }

        event Action<WindowResizeEvent>? Resized;
        event Action<WindowCloseEvent>? Closing;
        event Action<float, float>? MouseMoved;
        event Action<int, bool>? MouseButtonChanged;
        event Action<float>? MouseScrolled;

        void PollEvents();
        void SetTitle(string title);
        void Present(ReadOnlySpan<int> pixels, int width, int height);
        void Close();
    }
}
