using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using SeleniumMStestProject.Enums;

namespace SeleniumMStestProject.Base
{
    public abstract class SeleniumTestBase
    {
        /* Populated automatically by MSTest before each test, including
         * through inheritance, as long as the property is public with a setter.
         */
        public TestContext TestContext { get; set; } = null!;

        private IWebDriver? driver;
        private BrowserType? initializedBrowser;

        /* creates a Chrome driver on first access if no test has
         * explicitly called InitializeDriver(browserType) yet
         * keep single-browser tests unchanged while letting cross-browser tests
         * opt in without ever launching an unwanted default browser
         */
        protected IWebDriver Driver
        {
            get
            {
                if (driver == null)
                {
                    InitializeDriver(BrowserType.Chrome);
                }

                return driver!;
            }
        }

        protected void InitializeDriver(BrowserType browserType)
        {
            if (driver != null && initializedBrowser == browserType)
            {
                return;
            }

            driver?.Quit();
            driver?.Dispose();

            driver = CreateDriver(browserType);
            initializedBrowser = browserType;
        }

        [TestCleanup]
        public void BaseTearDown()
        {
            if (driver != null && TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                CaptureFailureScreenshot();
            }

            driver?.Quit();
            driver?.Dispose();
            driver = null;
        }

        private static IWebDriver CreateDriver(BrowserType browserType)
        {
            var headless = Config.IsHeadless;

            return browserType switch
            {
                BrowserType.Default or BrowserType.Chrome => CreateChromeDriver(headless),
                BrowserType.Firefox => CreateFirefoxDriver(headless),
                BrowserType.Edge => CreateEdgeDriver(headless),
                _ => throw new NotSupportedException($"Browser '{browserType}' is not supported.")
            };
        }

        private static IWebDriver CreateChromeDriver(bool headless)
        {
            var options = new ChromeOptions();
            BlockAdDomains(options);
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            if (headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");
            }

            return new ChromeDriver(options);
        }

        private static IWebDriver CreateFirefoxDriver(bool headless)
        {
            var options = new FirefoxOptions();
            /* Firefox has no equivalent to Chromium's --host-resolver-rules, so
             * ad domains aren't null-routed here; "eager" is what keeps
             * GeckoDriver from hanging (observed: a 60s timeout on an
             * otherwise-successful click) on a slow/never-completing ad
             * resource after navigation, by returning once the DOM is
             * interactive instead of waiting for the full "load" event.
             */
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            if (headless)
            {
                options.AddArgument("-headless");
                options.AddArgument("--width=1920");
                options.AddArgument("--height=1080");
            }

            return new FirefoxDriver(options);
        }

        private static IWebDriver CreateEdgeDriver(bool headless)
        {
            var options = new EdgeOptions();
            BlockAdDomains(options);
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            if (headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");
            }

            return new EdgeDriver(options);
        }

        /* automationexercise.com serves live Google ad iframes that can grow to
         * cover most of the viewport and intercept clicks on real page content.
         * Null-routing the ad domains at the Chromium network layer is far more
         * reliable than trying to click around an ad that loads unpredictably.
         */
        private static void BlockAdDomains(ChromiumOptions options)
        {
            options.AddArgument(
                "--host-resolver-rules=" +
                "MAP googleads.g.doubleclick.net 0.0.0.0, " +
                "MAP pagead2.googlesyndication.com 0.0.0.0, " +
                "MAP tpc.googlesyndication.com 0.0.0.0, " +
                "MAP www.googletagservices.com 0.0.0.0, " +
                "MAP securepubads.g.doubleclick.net 0.0.0.0");
        }

        private void CaptureFailureScreenshot()
        {
            if (driver is not ITakesScreenshot screenshotDriver)
            {
                return;
            }

            try
            {
                var directory = Path.Combine(TestContext.TestResultsDirectory ?? Path.GetTempPath(), "Screenshots");
                Directory.CreateDirectory(directory);

                var fileName = $"{TestContext.TestName}_{initializedBrowser}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png";
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
    }
}
