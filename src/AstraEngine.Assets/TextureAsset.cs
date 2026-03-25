namespace AstraEngine.Assets
{
    public sealed class TextureAsset
    {
        public TextureAsset(int width, int height, uint[] pixels)
        {
            Width = width;
            Height = height;
            Pixels = pixels;
        }

        public int Width { get; }
        public int Height { get; }
        public uint[] Pixels { get; }
    }
}
