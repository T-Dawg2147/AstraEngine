namespace AstraEngine.Core
{
    public sealed class EngineConfig
    {
        public string AppName { get; set; } = "AstraEngine";
        public int WindowWidth { get; set; } = 1280;
        public int WindowHeight { get; set; } = 720;
        public bool VSync { get; set; } = true;
        public double TargetFrameRate { get; set; } = 60.0;
        public bool EnableDebugLogging { get; set; } = true;
    }
}
