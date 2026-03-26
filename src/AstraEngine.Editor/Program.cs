using AstraEngine.Core;

namespace AstraEngine.Editor
{
    public static class Program
    {
        public static void Main()
        {
            var config = new EngineConfig
            {
                AppName = "AstraEngine",
                WindowWidth = 1280,
                WindowHeight = 720,
                VSync = true,
                TargetFrameRate = 60.0,
                EnableDebugLogging = true
            };

            using var host = new EngineHost(config);
            var editor = new EditorApplication();
            host.Run(editor);
        }
    }
}
