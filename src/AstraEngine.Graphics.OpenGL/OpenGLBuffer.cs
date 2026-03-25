namespace AstraEngine.Graphics.OpenGL
{
    public sealed class OpenGLBuffer : IBuffer
    {
        public OpenGLBuffer(BufferDescription description)
        {
            Description = description;
        }

        public BufferDescription Description { get; }

        public ulong SizeInBytes => Description.SizeInBytes;

        public void Dispose() { }
    }
}
