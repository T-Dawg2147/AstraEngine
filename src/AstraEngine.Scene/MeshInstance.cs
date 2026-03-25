using AstraEngine.Math;

namespace AstraEngine.Scene
{
    public sealed class MeshInstance
    {
        public MeshInstance(Mesh mesh)
        {
            Mesh = mesh;
        }

        public Mesh Mesh { get; }
        public Material Material { get; set; } = new();

        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; } = Vector3.One;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;

        public Matrix4x4 WorldMatrix
        {
            get
            {
                var translation = Matrix4x4.CreateTranslation(Position);
                var rotation = Matrix4x4.CreateRotation(Rotation);
                var scale = Matrix4x4.CreateScale(Scale);
                return scale * rotation * translation;
            }
        }
    }
}
