using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Pages
{
    internal class CheckoutPage
    {
        private readonly IWebDriver driver;
        private readonly WaitHelper wait;

        public CheckoutPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
        }

        public IWebElement DeliveryAddressBlock => wait.WaitForVisible(By.Id("address_delivery"));
        public IWebElement CommentField => wait.WaitForVisible(By.CssSelector("textarea[name='message']"));
        public IWebElement PlaceOrderButton => wait.WaitForClickable(By.XPath("//a[contains(text(),'Place Order')]"));

        public string DeliveryAddressText => DeliveryAddressBlock.Text;

        public void AddOrderComment(string comment)
        {
            CommentField.Clear();
            CommentField.SendKeys(comment);
        }

        public void PlaceOrder()
        {
            PlaceOrderButton.SafeClick(driver);
        }
    }
}
