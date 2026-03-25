using AstraEngine.Graphics.Abstractions;

namespace AstraEngine.Graphics.DirectX
{
    public sealed class DirectXTexture : ITexture
    {
        public DirectXTexture(TextureDescription description)
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
