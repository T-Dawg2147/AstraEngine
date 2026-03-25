using AstraEngine.Platform;

namespace AstraEngine.Graphics.Vulkan
{
    public sealed class VulkanContext
    {
        public VulkanContext(IWindow window)
        {
            Window = window;
        }

        public IWindow Window { get; }
        public bool IsInitialized { get; private set; }
        
        public void Initialized()
        {
            IsInitialized = true;
        }
    }
}
