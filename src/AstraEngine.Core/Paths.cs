namespace AstraEngine.Core
{
    public static class Paths
    {
        public static string AppDataDirectory =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AstraEngine");

        public static string SettingsFile =>
            Path.Combine(AppDataDirectory, "settings.json");

        public static string AssetsDirectory =>
            Path.Combine(AppContext.BaseDirectory, "Assets");
    }
}
