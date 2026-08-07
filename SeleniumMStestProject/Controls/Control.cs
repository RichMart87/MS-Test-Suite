using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;
using System.Threading;

namespace SeleniumMStestProject.Controls
{
    internal enum FindItBy
    {
        Id,
        Name,
        DataTerm,
        DataSysTestId,
        PartialCssClass
    }

    // Reusable base for page elements: resolves a locator once per FindItBy
    // strategy and exposes wait-backed access to the element.
    internal class Control
    {
        private readonly IWebDriver driver;
        private readonly WaitHelper wait;

        protected By Locator { get; }

        public TimeSpan JqueryWaitTime { get; set; } = Constants.Timeout.Medium;

        // Opt-in: only relevant on pages that use jQuery for async rendering.
        // Off by default since seleniumbase.io/demo_page doesn't use jQuery.
        public bool WaitForJquery { get; set; } = false;

        public Control(IWebDriver driver, FindItBy locatorType, string locatorValue)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
            Locator = BuildLocator(locatorType, locatorValue);
        }

        protected IWebElement Element => wait.WaitForVisible(Locator);

        protected IWebElement ClickableElement
        {
            get
            {
                if (WaitForJquery)
                {
                    WaitForJqueryToSettle();
                }

                return wait.WaitForClickable(Locator);
            }
        }

        public bool IsDisplayed => Element.Displayed;

        public void Click()
        {
            ClickableElement.Click();
        }

        public string GetText()
        {
            return Element.Text;
        }

        private void WaitForJqueryToSettle()
        {
            if (driver is not IJavaScriptExecutor js)
            {
                return;
            }

            var deadline = DateTime.UtcNow + JqueryWaitTime;
            while (DateTime.UtcNow < deadline)
            {
                var idle = js.ExecuteScript("return (typeof jQuery === 'undefined') || jQuery.active === 0;");
                if (idle is true)
                {
                    return;
                }

                Thread.Sleep(100);
            }
        }

        private static By BuildLocator(FindItBy locatorType, string locatorValue)
        {
            return locatorType switch
            {
                FindItBy.Id => By.Id(locatorValue),
                FindItBy.Name => By.Name(locatorValue),
                FindItBy.DataTerm => By.CssSelector($"[data-term='{locatorValue}']"),
                FindItBy.DataSysTestId => By.CssSelector($"[data-sys-test-id='{locatorValue}']"),
                FindItBy.PartialCssClass => By.CssSelector($"[class*='{locatorValue}']"),
                _ => throw new ArgumentOutOfRangeException(nameof(locatorType), locatorType, "Unsupported locator type.")
            };
        }
    }
}
