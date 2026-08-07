using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumMStestProject.Utilities
{
    public class WaitHelper
    {
        private readonly WebDriverWait wait;

        public WaitHelper(IWebDriver driver)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Config.ExplicitWait));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        }

        public IWebElement WaitForVisible(By locator)
        {
            return wait.Until(drv =>
            {
                var element = drv.FindElement(locator);
                return element.Displayed ? element : null;
            });
        }

        public IWebElement WaitForClickable(By locator)
        {
            return wait.Until(drv =>
            {
                var element = drv.FindElement(locator);
                return element.Displayed && element.Enabled ? element : null;
            });
        }
    }
}
