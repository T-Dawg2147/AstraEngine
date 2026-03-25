using AstraEngine.Platform;

namespace AstraEngine.Graphics.DirectX
{
    public sealed class DirectXContext 
    {
        public DirectXContext(IWindow window)
        {
            Window = window;
        }

        public IWindow Window { get; }

        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            IsInitialized = true;
        }
    }
}
