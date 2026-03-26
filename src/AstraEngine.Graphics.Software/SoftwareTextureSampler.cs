using AstraEngine.Assets;
using AstraEngine.Math;

namespace AstraEngine.Graphics.Software
{
    /// <summary>
    /// Wraps texture sampling for the software renderer.
    /// Supports wrap modes and filter modes.
    /// </summary>
    public sealed class SoftwareTextureSampler
    {
        public enum FilterMode
        {
            Nearest,
            Bilinear
        }

        public enum WrapMode
        {
            Repeat,
            Clamp
        }

        public FilterMode Filter { get; set; } = FilterMode.Bilinear;
        public WrapMode Wrap { get; set; } = WrapMode.Repeat;

        /// <summary>
        /// Samples the given texture at UV coordinates.
        /// </summary>
        public Color4 Sample(TextureAsset texture, float u, float v)
        {
            u = ApplyWrap(u);
            v = ApplyWrap(v);

            return Filter == FilterMode.Bilinear
                ? texture.Sample(u, v)
                : texture.SampleNearest(u, v);
        }

        private float ApplyWrap(float coord)
        {
            return Wrap switch
            {
                WrapMode.Repeat => coord - MathF.Floor(coord),
                WrapMode.Clamp => System.Math.Clamp(coord, 0f, 1f),
                _ => coord - MathF.Floor(coord)
            };
        }
    }
}