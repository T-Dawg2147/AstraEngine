namespace AstraEngine.Scene
{
    public sealed class MeshEntity : ISceneObject
    {
        public MeshEntity(MeshInstance mesh)
        {
            Mesh = mesh;
        }

        public MeshInstance Mesh { get; }
        public bool Visible { get; set; } = true;

        public Material Material
        {
            get => Mesh.Material;
            set => Mesh.Material = value;
        }

        public void Update(float deltaTime)
        {

        }
    }
}
