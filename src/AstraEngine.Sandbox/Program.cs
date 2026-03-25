using AstraEngine.Core;

namespace AstraEngine.Sandbox
{
    public static class Program
    {
        public static void Main()
        {
            var config = new EngineConfig
            {
                AppName = "AstraEngine Sandbox",
                WindowWidth = 1280,
                WindowHeight = 720,
                VSync = true,
                TargetFrameRate = 60.0,
                EnableDebugLogging = true
            };

            using var host = new EngineHost(config);
            var app = new SandboxApplication();
            host.Run(app);
        }
    }
}
