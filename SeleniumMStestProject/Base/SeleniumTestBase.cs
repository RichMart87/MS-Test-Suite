using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumMStestProject.Base
{
    public abstract class SeleniumTestBase
    {
        // Populated automatically by MSTest before each test, including
        // through inheritance, as long as the property is public with a setter.
        public TestContext TestContext { get; set; } = null!;

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
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                CaptureFailureScreenshot();
            }

            Driver?.Quit();
            Driver?.Dispose();
        }

        private void CaptureFailureScreenshot()
        {
            if (Driver is not ITakesScreenshot screenshotDriver)
            {
                return;
            }

            try
            {
                var directory = Path.Combine(TestContext.TestResultsDirectory ?? Path.GetTempPath(), "Screenshots");
                Directory.CreateDirectory(directory);

                var fileName = $"{TestContext.TestName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png";
                var filePath = Path.Combine(directory, fileName);

                screenshotDriver.GetScreenshot().SaveAsFile(filePath);
                TestContext.AddResultFile(filePath);
                TestContext.WriteLine($"Screenshot captured on failure: {filePath}");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Failed to capture failure screenshot: {ex.Message}");
            }
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
