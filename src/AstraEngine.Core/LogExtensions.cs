namespace AstraEngine.Core
{
    public static class LogExtensions
    {
        public static void InfoSection(string title)
        {
            Logger.Info($"=== {title} ===");
        }

        public static void WarnIf(bool condition, string message)
        {
            if (condition)
                Logger.Warn(message);
        }
    }
}
