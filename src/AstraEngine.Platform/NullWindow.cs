using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstraEngine.Platform
{
    public sealed class NullWindow : IWindow
    {
        public NullWindow(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
            State = WindowState.Normal;
            IsOpen = true;
        }

        public string Title { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public WindowState State { get; private set; }
        public bool IsOpen { get; private set; }

        public event Action<WindowResizeEvent>? Resized;
        public event Action<WindowCloseEvent>? Closing;
        public event Action<float, float>? MouseMoved;
        public event Action<int, bool>? MouseButtonChanged;
        public event Action<float>? MouseScrolled;

        public void PollEvents()
        {

        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void Present(ReadOnlySpan<int> pixels, int width, int height) { }

        public void Close()
        {
            if (!IsOpen)
                return;

            Closing?.Invoke(new WindowCloseEvent());
            IsOpen = false;
        }

        public void Dispose() => Close();
    }
}
