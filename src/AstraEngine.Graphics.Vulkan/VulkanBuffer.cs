namespace AstraEngine.Graphics.Vulkan
{
    public sealed class VulkanBuffer : IBuffer
    {
        public VulkanBuffer(BufferDescription description)
        {
            Description = description;
        }

        public BufferDescription Description { get; }

        public ulong SizeInBytes => Description.SizeInBytes;

        public void Dispose() { }
    }
}
