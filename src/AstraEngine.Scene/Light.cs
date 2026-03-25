using AstraEngine.Math;

namespace AstraEngine.Scene
{
    public abstract class Light
    {
        public Color4 Color { get; set; } = new(1f, 1f, 1f, 1f);
        public float Intensity { get; set; } = 1f;
        public bool Enabled { get; set; } = true;
    }

    public sealed class AmbientLight : Light { }

    public sealed class DirectionalLight : Light
    {
        public Vector3 Direction { get; set; } = Vector3.Normalize(new Vector3(-1f, -1f, -1f));
    }

    public sealed class PointLight : Light
    {
        public Vector3 Position { get; set; }
        public float Range { get; set; } = 10f;
        public float Attenuation { get; set; } = 1f;
    }

    public sealed class SpotLight : Light
    {
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; } = Vector3.Normalize(new Vector3(0f, -1f, 0f));
        public float InnerConeAngle { get; set; } = 15f;
        public float OuterConeAngle { get; set; } = 25f;
        public float Range { get; set; } = 10f;
    }
}
