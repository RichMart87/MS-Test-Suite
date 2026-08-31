using Microsoft.Extensions.Configuration;


namespace SeleniumMStestProject
{
    internal class Config
    {
        private static readonly IConfigurationRoot AppConfig = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        internal static string? GetSetting(string key)
        {
            return AppConfig[key];
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

        internal static bool IsHeadless
        {
            get { return IsEnvVarTrue("CI") || IsEnvVarTrue("HEADLESS"); }
        }

        private static bool IsEnvVarTrue(string name)
        {
            return string.Equals(Environment.GetEnvironmentVariable(name), "true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
