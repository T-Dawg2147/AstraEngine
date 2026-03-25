namespace AstraEngine.Platform
{
    public sealed class NullPlatform : IPlatform
    {
        public IWindow CreateWindow(string title, int width, int height)
            => new NullWindow(title, width, height);
    }
}
