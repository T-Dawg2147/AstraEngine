using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstraEngine.Platform.Windows
{
    public sealed class WindowsPlatform : IPlatform
    {
        public IWindow CreateWindow(string title, int width, int height)
            => new WindowsWindow(title, width, height);
    }
}
