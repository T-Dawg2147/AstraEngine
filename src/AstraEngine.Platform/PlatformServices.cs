namespace AstraEngine.Platform
{
    public sealed class PlatformServices
    {
        public PlatformServices(IPlatform platform, IFileSystem fileSystem)
        {
            Platform = platform ?? throw new ArgumentNullException(nameof(platform));
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public static IPlatform Platform { get; private set; } = default!;
        public static IFileSystem FileSystem { get; private set; } = default!;

        public static void Initialize(IPlatform platform, IFileSystem fileSystem)
        {
            Platform = platform ?? throw new ArgumentNullException(nameof(platform));
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }
    }
}
