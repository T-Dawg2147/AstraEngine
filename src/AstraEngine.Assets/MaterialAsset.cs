using AstraEngine.Math;

namespace AstraEngine.Assets
{
    /// <summary>
    /// Serializable material definition loaded from asset files.
    /// Used by the content pipeline to create runtime Material instances.
    /// </summary>
    public sealed class MaterialAsset
    {
        public string Name { get; set; } = "DefaultMaterial";
        public Color4 BaseColor { get; set; } = new(1f, 1f, 1f, 1f);
        public float Metallic { get; set; } = 0f;
        public float Roughness { get; set; } = 1f;
        public float Opacity { get; set; } = 1f;
        public string? AlbedoTexturePath { get; set; }

        public Scene.Material ToRuntimeMaterial(AssetManager? assets = null)
        {
            return new Scene.Material
            {
                Name = Name,
                BaseColor = BaseColor,
                Metallic = Metallic,
                Roughness = Roughness,
                Opacity = Opacity,
                AlbedoTexturePath = AlbedoTexturePath
            };
        }
    }
}
