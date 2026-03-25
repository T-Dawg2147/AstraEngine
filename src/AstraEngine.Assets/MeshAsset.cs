using AstraEngine.Scene;

namespace AstraEngine.Assets
{
    public sealed class MeshAsset
    {
        public MeshAsset(Mesh mesh)
        {
            Mesh = mesh;
        }

        public Mesh Mesh { get; }
    }
}
