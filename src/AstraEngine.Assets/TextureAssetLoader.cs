using System.Drawing;
using System.Drawing.Imaging;

namespace AstraEngine.Assets
{
    internal static class TextureAssetLoader
    {
        public static TextureAsset Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Texture asset not found.", path);

            using var bitmap = new Bitmap(path);
            var width = bitmap.Width;
            var height = bitmap.Height;
            var pixels = new uint[width * height];

            var rect = new Rectangle(0, 0, width, height);
            var data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                unsafe
                {
                    var src = (uint*)data.Scan0;
                    for (var y = 0; y < height; y++)
                    {
                        for (var x = 0; x < width; x++)
                        {
                            pixels[(y * width) + x] = src[(y * data.Stride / 4) + x];
                        }
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            return new TextureAsset(width, height, pixels);
        }
    }
}
