namespace AstraEngine.Core
{
    public static class Logger
    {
        public static bool Enabled { get; set; } = true;

        public static void Info(string message) => Write("INFO", message);
        public static void Warn(string message) => Write("WARN", message);
        public static void Error(string message) => Write("ERROR", message);
        public static void Debug(string message) => Write("DEBUG", message);

        private static void Write(string level, string message)
        {
            if (!Enabled) return;

            var line = $"[{DateTime.UtcNow:HH:mm:ss}] [{level}] {message}";
            System.Diagnostics.Debug.WriteLine(line);
            Console.WriteLine(line);
        }
    }
}
