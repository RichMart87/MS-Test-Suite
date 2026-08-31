using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Pages
{
    internal class CartPage
    {
        private readonly IWebDriver driver;
        private readonly WaitHelper wait;

        public CartPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
        }

        public void GoTo()
        {
            driver.Navigate().GoToUrl($"{Config.BaseUrl}/view_cart");
        }

        public IWebElement ProceedToCheckoutButton => wait.WaitForClickable(By.CssSelector("a.check_out"));
        public IWebElement EmptyCartMessage => wait.WaitForVisible(By.Id("empty_cart"));

        public IWebElement SubscribeEmailField => wait.WaitForVisible(By.Id("susbscribe_email"));
        public IWebElement SubscribeButton => wait.WaitForClickable(By.Id("subscribe"));
        public IWebElement SubscriptionSuccessMessage => wait.WaitForVisible(By.CssSelector("#success-subscribe .alert-success"));

        public void ProceedToCheckout() => ProceedToCheckoutButton.SafeClick(driver);

        public string SubscribeToNewsletter(string email)
        {
            SubscribeEmailField.Clear();
            SubscribeEmailField.SendKeys(email);
            SubscribeButton.SafeClick(driver);

            return SubscriptionSuccessMessage.Text;
        }

        public bool ContainsProduct(string productName)
        {
            return driver.FindElements(RowLocator(productName)).Count > 0;
        }

        public int GetQuantity(string productName)
        {
            var quantityButton = RowFor(productName).FindElement(By.CssSelector("td.cart_quantity button"));
            return int.Parse(quantityButton.Text.Trim());
        }

        public void RemoveProduct(string productName)
        {
            RowFor(productName).FindElement(By.CssSelector("td.cart_delete a.cart_quantity_delete")).SafeClick(driver);
        }

        private IWebElement RowFor(string productName)
        {
            return wait.WaitForVisible(RowLocator(productName));
        }

        private static By RowLocator(string productName)
        {
            return By.XPath($"//td[contains(@class,'cart_description')][.//a[normalize-space(text())='{productName}']]/ancestor::tr");
        }
    }
}
