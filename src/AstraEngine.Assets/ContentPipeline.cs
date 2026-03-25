namespace AstraEngine.Assets
{
    public sealed class ContentPipeline
    {
        private readonly AssetManager _assets;
        private readonly ContentDatabase _database;

        public ContentPipeline(AssetManager assets, ContentDatabase database)
        {
            _assets = assets;
            _database = database;
        }

        public T Import<T>(string path) where T : class
        {
            var asset = _assets.Load<T>(path);

            if (_assets.TryGet(path, out T? _))
            {
                _database.Store(new AssetMetadata
                {
                    SourcePath = path,
                    AssetType = typeof(T).Name,
                    LastImportedUtc = DateTime.UtcNow
                });
            }
            return asset;
        }

        public bool Reimport<T>(string path) where T : class
        {
            var success = _assets.Reimport<T>(path);

            if (success && _assets.TryGet(path, out T? _))
            {
                _database.Store(new AssetMetadata
                {
                    SourcePath = path,
                    AssetType = typeof(T).Name,
                    LastImportedUtc = DateTime.UtcNow
                });
            }
            return success;
        }
    }
}
