using System.Security.Cryptography;

namespace AstraEngine.Assets
{
    public sealed class AssetManager
    {
        private readonly Dictionary<string, object> _assets = new(StringComparer.OrdinalIgnoreCase);

        public T Load<T>(string path) where T : class
        {
            if (_assets.TryGetValue(path, out var existing) && existing is T typed)
                return typed;

            var asset = ImportAsset<T>(path);

            var metadata = new AssetMetadata
            {
                SourcePath = path,
                AssetType = typeof(T).Name,
                LastImportedUtc = DateTime.UtcNow,
                ContentHash = ComputeHash(path)
            };

            _assets[path] = new AssetRecord
            {
                Path = path, 
                Asset = asset,
                Metadata = metadata
            };

            return (T)asset;
        }

        public bool TryGet<T>(string path, out T? asset) where T : class
        {
            if (_assets.TryGetValue(path, out var existing) && existing is T typed)
            {
                asset = typed;
                return true;
            }

            asset = null;
            return false;                
        }

        public bool Reimport<T>(string path) where T : class
        {
            if (!_assets.ContainsKey(path))
                return false;

            var asset = ImportAsset<T>(path);

            _assets[path] = new AssetRecord
            {
                Path = path,
                Asset = asset,
                Metadata = new AssetMetadata
                {
                    SourcePath = path,
                    AssetType = typeof(T).Name,
                    LastImportedUtc = DateTime.UtcNow,
                    ContentHash = ComputeHash(path)
                }
            };

            return true;
        }

        private static object ImportAsset<T>(string path) where T : class
        {
            object asset = typeof(T) switch
            {
                var t when t == typeof(MeshAsset) => MeshAssetLoader.Load(path),
                var t when t == typeof(TextureAsset) => TextureAssetLoader.Load(path),
                _ => throw new NotSupportedException($"Asset type '{typeof(T).Name}' is not supported yet.")
            };

            return asset;
        }

        private static bool IsUpToDate(AssetRecord record)
        {
            var currentHash = ComputeHash(record.Path);
            return string.Equals(currentHash, record.Metadata.ContentHash, StringComparison.OrdinalIgnoreCase);
        }

        private static string? ComputeHash(string path)
        {
            if (!File.Exists(path))
                return null;

            using var stream = File.OpenRead(path);
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(stream);
            return Convert.ToHexString(hash);
        }
    }
}
