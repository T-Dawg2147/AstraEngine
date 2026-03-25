namespace AstraEngine.Assets
{
    public sealed class ContentDatabase
    {
        private readonly Dictionary<string, AssetMetadata> _metadata = new(StringComparer.OrdinalIgnoreCase);

        public void Store(AssetMetadata metadata)
            => _metadata[metadata.SourcePath] = metadata;

        public bool TryGet(string sourcePath, out AssetMetadata? metadata)
        {
            if (_metadata.TryGetValue(sourcePath, out var existing))
            {
                metadata = existing;
                return true;
            }

            metadata = null;
            return false;
        }
    }
}
