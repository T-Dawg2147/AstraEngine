namespace AstraEngine.Assets
{
    public interface IAssetImporter<T> where T : class
    {
        T Import(string path);
    }
}
