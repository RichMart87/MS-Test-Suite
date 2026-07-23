using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumMStestProject.Base
{
    public abstract class SeleniumTestBase
    {
        protected IWebDriver Driver { get; private set; } = null!;

        [TestInitialize]
        public void BaseSetUp()
        {
            var options = new ChromeOptions();

            if (IsHeadlessRequested())
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");
            }

            Driver = new ChromeDriver(options);
        }

        [TestCleanup]
        public void BaseTearDown()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }

        private static bool IsHeadlessRequested()
        {
            return IsEnvVarTrue("CI") || IsEnvVarTrue("HEADLESS");
        }

        private static bool IsEnvVarTrue(string name)
        {
            return string.Equals(Environment.GetEnvironmentVariable(name), "true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
