namespace AstraEngine.Core
{
    public sealed class AppSettings
    {
        public string AppName { get; set; } = "AstraEngine";
        public int WindowWidth { get; set; } = 1280;
        public int WindowHeight { get; set; } = 720;
        public bool Fullscreen { get; set; } = false;
        public bool VSync { get; set; } = true;
        public string GraphicsBackend { get; set; } = "OpenGL";
    }
}
