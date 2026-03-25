namespace AstraEngine.Assets
{
    public sealed class AssetMetadata
    {
        public string SourcePath { get; set; } = string.Empty;
        public string AssetType { get; set; } = string.Empty;
        public DateTime LastImportedUtc { get; set; } 
        public string? ContentHash { get; set; }
    }
}
