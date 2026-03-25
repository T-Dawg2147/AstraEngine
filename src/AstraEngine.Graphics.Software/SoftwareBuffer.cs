namespace AstraEngine.Graphics.Software
{
    public sealed class SoftwareBuffer : IBuffer
    {
        public SoftwareBuffer(BufferDescription description)
        {
            Description = description;
        }

        public BufferDescription Description { get; }

        public ulong SizeInBytes => Description.SizeInBytes;

        public void Dispose() { }
    }
}
