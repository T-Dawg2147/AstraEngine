namespace AstraEngine.Platform
{
    public interface IFileSystem
    {
        byte[] ReadAllBytes(string path);
        string ReadAllText(string path);
        void WriteAllBytes(string path, byte[] data);
        void WriteAllText(string path, string content);
    }
}
