using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Pages
{
    /* Test only payment data, automationexercise.com's payment gateway is a
    sandbox that never charges any card and accepts arbitrary values
     */
    internal record CardDetails(string NameOnCard, string CardNumber, string Cvc, string ExpiryMonth, string ExpiryYear);

    internal class PaymentPage
    {
        private readonly IWebDriver driver;
        private readonly WaitHelper wait;

        public PaymentPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
        }

        public IWebElement NameOnCardField => wait.WaitForVisible(By.CssSelector("[data-qa='name-on-card']"));
        public IWebElement CardNumberField => wait.WaitForVisible(By.CssSelector("[data-qa='card-number']"));
        public IWebElement CvcField => wait.WaitForVisible(By.CssSelector("[data-qa='cvc']"));
        public IWebElement ExpiryMonthField => wait.WaitForVisible(By.CssSelector("[data-qa='expiry-month']"));
        public IWebElement ExpiryYearField => wait.WaitForVisible(By.CssSelector("[data-qa='expiry-year']"));
        public IWebElement PayButton => wait.WaitForClickable(By.CssSelector("[data-qa='pay-button']"));

        public IWebElement OrderPlacedHeading => wait.WaitForVisible(By.CssSelector("[data-qa='order-placed']"));
        public IWebElement DownloadInvoiceLink => wait.WaitForClickable(By.XPath("//a[contains(text(),'Download Invoice')]"));

        public void PayWith(CardDetails card)
        {
            NameOnCardField.SendKeys(card.NameOnCard);
            CardNumberField.SendKeys(card.CardNumber);
            CvcField.SendKeys(card.Cvc);
            ExpiryMonthField.SendKeys(card.ExpiryMonth);
            ExpiryYearField.SendKeys(card.ExpiryYear);
            PayButton.SafeClick(driver);
        }
    }
}
