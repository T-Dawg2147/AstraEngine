using AstraEngine.Platform;
using AstraEngine.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstraEngine.Graphics.OpenGL
{
    public sealed class OpenGLSwapChain : ISwapChain
    {
        private readonly IWindow _window;

        public OpenGLSwapChain(IWindow window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            Width = window.Width;
            Height = window.Height;
        }

        public WindowsWindow Window => (WindowsWindow)_window;

        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public void Present() 
        {
            Window.SwapOpenGlBuffers();
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Dispose() { }
    }
}
