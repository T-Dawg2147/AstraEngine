namespace AstraEngine.Assets
{
    internal static class MeshAssetLoader
    {
        public static MeshAsset Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Mesh asset not found.", path);

            if (Path.GetExtension(path).Equals(".obj", StringComparison.OrdinalIgnoreCase))
                return new MeshAsset(ObjMeshLoader.Load(path));

            throw new NotSupportedException($"Unsupported mesh format: {Path.GetExtension(path)}");
        }
    }
}
