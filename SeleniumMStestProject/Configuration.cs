using System.Configuration;

namespace SeleniumMStestProject
{
    internal class Config
    {
        // ConfigurationManager.AppSettings resolves against the *entry process's*
        // config file. Under `dotnet test` that's the VSTest test host, which has
        // no matching .config, so the ambient AppSettings lookup silently returns
        // null for every key. Load our own assembly's shipped .dll.config
        // explicitly instead so App.config values are actually honored.
        private static readonly Configuration AppConfig = LoadAppConfig();

        private static Configuration LoadAppConfig()
        {
            var configPath = Path.Combine(AppContext.BaseDirectory, $"{typeof(Config).Assembly.GetName().Name}.dll.config");
            var map = new ExeConfigurationFileMap { ExeConfigFilename = configPath };
            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }

        internal static string? GetSetting(string key)
        {
            return AppConfig.AppSettings.Settings[key]?.Value;
        }

        public static string BaseUrl
        {
            get { return GetSetting("BaseUrl") ?? "https://seleniumbase.io/demo_page"; }
        }

        public static string ApiBaseUrl
        {
            get { return GetSetting("ApiBaseUrl") ?? "https://automationexercise.com"; }
        }

        public static int ImplicitWait
        {
            get { return int.TryParse(GetSetting("ImplicitWaitSeconds"), out var seconds) ? seconds : 10; }
        }

        public static int ExplicitWait
        {
            get { return int.TryParse(GetSetting("ExplicitWaitSeconds"), out var seconds) ? seconds : 30; }
        }
    }
}
