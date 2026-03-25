namespace AstraEngine.Assets
{
    public sealed class AssetRecord
    {
        public required string Path { get; init; }
        public required object Asset { get; init; }
        public required AssetMetadata Metadata { get; init; }
    }
}
