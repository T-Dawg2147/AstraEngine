namespace AstraEngine.Graphics.DirectX
{
    public sealed class DirectXBuffer : IBuffer
    {
        public DirectXBuffer(BufferDescription description)
        {
            Description = description;
        }

        public BufferDescription Description { get; }

        public ulong SizeInBytes => Description.SizeInBytes;

        public void Dispose() { }
    }
}
