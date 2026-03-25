namespace AstraEngine.Platform
{
    public interface IPlatform
    {
        IWindow CreateWindow(string title, int width, int height);
    }
}
