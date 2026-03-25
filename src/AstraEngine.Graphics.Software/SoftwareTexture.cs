using AstraEngine.Graphics.Abstractions;

namespace AstraEngine.Graphics.Software
{
    public sealed class SoftwareTexture : ITexture
    {
        public SoftwareTexture(TextureDescription description)
        {
            Description = description;
        }

        public TextureDescription Description { get; }

        public int Width => Description.Width;
        public int Height => Description.Height;
        public PixelFormat Format => Description.Format;

        public void Dispose() { }
    }
}
