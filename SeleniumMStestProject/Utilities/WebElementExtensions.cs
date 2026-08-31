using OpenQA.Selenium;

namespace SeleniumMStestProject.Utilities
{
    internal static class WebElementExtensions
    {
        // automationexercise.com serves live ad iframes that can overlap page
        // content (e.g. the footer subscribe button) and intercept a normal
        // click. Falling back to a JS click sidesteps the overlap without
        // affecting the outcome of the click itself.
        public static void SafeClick(this IWebElement element, IWebDriver driver)
        {
            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                if (driver is not IJavaScriptExecutor js)
                {
                    throw;
                }

                js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'}); arguments[0].click();", element);
            }
        }

        // Selenium's SelectElement opens a native <select> by clicking it, then
        // clicks the target <option> - two more click targets an ad iframe can
        // sit on top of. Setting selectedIndex via JS and firing 'change'
        // achieves the same result without any click, on every browser (this
        // is the account-information form's country/day/month/year selects).
        public static void SelectByTextViaJs(this IWebElement selectElement, IWebDriver driver, string text)
        {
            if (driver is not IJavaScriptExecutor js)
            {
                throw new NotSupportedException("Selecting an option via JS requires a JavaScript-capable driver.");
            }

            js.ExecuteScript(
                """
                var select = arguments[0];
                var text = arguments[1];
                for (var i = 0; i < select.options.length; i++) {
                    if (select.options[i].text === text) {
                        select.selectedIndex = i;
                        select.dispatchEvent(new Event('change', { bubbles: true }));
                        return;
                    }
                }
                throw new Error("No option with text '" + text + "' found.");
                """,
                selectElement,
                text);
        }

        public static void SelectByValueViaJs(this IWebElement selectElement, IWebDriver driver, string value)
        {
            if (driver is not IJavaScriptExecutor js)
            {
                throw new NotSupportedException("Selecting an option via JS requires a JavaScript-capable driver.");
            }

            js.ExecuteScript(
                "arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('change', { bubbles: true }));",
                selectElement,
                value);
        }
    }
}
