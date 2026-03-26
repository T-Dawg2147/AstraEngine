using AstraEngine.Math;

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

        /// <summary>
        /// Samples a color from this texture at the given UV coordinates.
        /// UV is in [0,1] range with wrapping. Uses bilinear filtering.
        /// Pixel data is expected in ARGB format (as loaded by System.Drawing).
        /// </summary>
        public Color4 Sample(float u, float v)
        {
            u = u - MathF.Floor(u);
            v = v - MathF.Floor(v);

            var fx = u * (Width - 1);
            var fy = v * (Height - 1);

            var x0 = (int)fx;
            var y0 = (int)fy;
            var x1 = System.Math.Min(x0 + 1, Width - 1);
            var y1 = System.Math.Min(y0 + 1, Height - 1);

            var xFrac = fx - x0;
            var yFrac = fy - y0;

            var c00 = DecodeArgb(Pixels[y0 * Width + x0]);
            var c10 = DecodeArgb(Pixels[y0 * Width + x1]);
            var c01 = DecodeArgb(Pixels[y1 * Width + x0]);
            var c11 = DecodeArgb(Pixels[y1 * Width + x1]);

            // Bilinear interpolation
            var top = Lerp(c00, c10, xFrac);
            var bottom = Lerp(c01, c11, xFrac);
            return Lerp(top, bottom, yFrac);
        }

        public Color4 SampleNearest(float u, float v)
        {
            u = u - MathF.Floor(u);
            v = v - MathF.Floor(v);

            var x = System.Math.Clamp((int)(u * Width), 0, Width - 1);
            var y = System.Math.Clamp((int)(v * Height), 0, Height - 1);

            return DecodeArgb(Pixels[y * Width + x]);
        }

        private static Color4 DecodeArgb(uint pixel)
        {
            var a = ((pixel >> 24) & 0xFF) / 255f;
            var r = ((pixel >> 16) & 0xFF) / 255f;
            var g = ((pixel >> 8) & 0xFF) / 255f;
            var b = (pixel & 0xFF) / 255f;
            return new Color4(r, g, b, a);
        }

        private static Color4 Lerp(Color4 a, Color4 b, float t)
            => new Color4(
                a.R + (b.R - a.R) * t,
                a.G + (b.G - a.G) * t,
                a.B + (b.B - a.B) * t,
                a.A + (b.A - a.A) * t);
    }
}
