namespace AstraEngine.Platform
{
    public sealed class NullFileSystem : IFileSystem
    {
        private string placeholderText = "File system is not configured yet.";

        public byte[] ReadAllBytes(string path) => throw new NotImplementedException(placeholderText);
        public string ReadAllText(string path) => throw new NotImplementedException(placeholderText);
        public void WriteAllBytes(string path, byte[] data) => throw new NotImplementedException(placeholderText);
        public void WriteAllText(string path, string content) => throw new NotImplementedException(placeholderText);
    }
}
