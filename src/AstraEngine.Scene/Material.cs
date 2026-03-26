using AstraEngine.Math;

namespace AstraEngine.Scene
{
    public sealed class Material
    {
        public string Name { get; set; } = "DefaultMaterial";
        public Color4 BaseColor { get; set; } = new(1f, 1f, 1f, 1f);
        public float Metallic { get; set; } = 0f;
        public float Roughness { get; set; } = 1;
        public float Opacity { get; set; } = 1f;

        /// <summary>
        /// File path to the albedo texture for asset loading.
        /// </summary>
        public string? AlbedoTexturePath { get; set; }
    }
}
