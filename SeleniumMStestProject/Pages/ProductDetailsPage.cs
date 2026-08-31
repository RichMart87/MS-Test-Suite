using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Pages
{
    internal class ProductDetailsPage
    {
        private readonly IWebDriver driver;
        private readonly WaitHelper wait;

        public ProductDetailsPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
        }

        public IWebElement QuantityField => wait.WaitForVisible(By.Id("quantity"));
        public IWebElement AddToCartButton => wait.WaitForClickable(By.CssSelector("button.cart"));
        public IWebElement CartModal => wait.WaitForVisible(By.Id("cartModal"));
        public IWebElement ViewCartLinkInModal => wait.WaitForClickable(By.CssSelector("#cartModal a[href='/view_cart']"));

        public IWebElement ReviewNameField => wait.WaitForVisible(By.Id("name"));
        public IWebElement ReviewEmailField => wait.WaitForVisible(By.Id("email"));
        public IWebElement ReviewTextField => wait.WaitForVisible(By.Id("review"));
        public IWebElement ReviewSubmitButton => wait.WaitForClickable(By.Id("button-review"));
        public IWebElement ReviewSuccessMessage => wait.WaitForVisible(By.CssSelector("#review-section .alert-success"));

        public void SetQuantity(int quantity)
        {
            QuantityField.Clear();
            QuantityField.SendKeys(quantity.ToString());
        }

        public void AddToCart()
        {
            AddToCartButton.SafeClick(driver);
        }

        public void ViewCartFromModal() => ViewCartLinkInModal.SafeClick(driver);

        public string SubmitReview(string name, string email, string review)
        {
            ReviewNameField.SendKeys(name);
            ReviewEmailField.SendKeys(email);
            ReviewTextField.SendKeys(review);
            ReviewSubmitButton.SafeClick(driver);

            return ReviewSuccessMessage.Text;
        }
    }
}
