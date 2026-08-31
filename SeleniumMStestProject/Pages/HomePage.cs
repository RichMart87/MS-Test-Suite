using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Pages
{
    internal class HomePage
    {
        private readonly IWebDriver driver;
        private readonly WaitHelper wait;

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
        }

        public void GoTo()
        {
            var baseUrl = Config.BaseUrl;
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(baseUrl);

            Assert.AreEqual(baseUrl.TrimEnd('/'), driver.Url.TrimEnd('/'), "Should land on the home page.");
        }

        public IWebElement ProductsLink => wait.WaitForClickable(By.CssSelector("a[href='/products']"));
        public IWebElement CartLink => wait.WaitForClickable(By.CssSelector("a[href='/view_cart']"));
        public IWebElement SignupLoginLink => wait.WaitForClickable(By.CssSelector("a[href='/login']"));
        public IWebElement TestCasesLink => wait.WaitForClickable(By.CssSelector("a[href='/test_cases']"));
        public IWebElement ContactUsLink => wait.WaitForClickable(By.CssSelector("a[href='/contact_us']"));
        public IWebElement LogoutLink => wait.WaitForClickable(By.CssSelector("a[href='/logout']"));
        public IWebElement DeleteAccountLink => wait.WaitForClickable(By.CssSelector("a[href='/delete_account']"));
        public IWebElement LoggedInAsLink => wait.WaitForVisible(By.XPath("//a[contains(., 'Logged in as')]"));

        public IWebElement SubscribeEmailField => wait.WaitForVisible(By.Id("susbscribe_email"));
        public IWebElement SubscribeButton => wait.WaitForClickable(By.Id("subscribe"));
        public IWebElement SubscriptionSuccessMessage => wait.WaitForVisible(By.CssSelector("#success-subscribe .alert-success"));

        public void GoToProducts() => ProductsLink.SafeClick(driver);

        public void GoToCart() => CartLink.SafeClick(driver);

        public void GoToSignupLogin() => SignupLoginLink.SafeClick(driver);

        public void GoToTestCases() => TestCasesLink.SafeClick(driver);

        public void GoToContactUs() => ContactUsLink.SafeClick(driver);

        public void Logout() => LogoutLink.SafeClick(driver);

        public void DeleteAccount() => DeleteAccountLink.SafeClick(driver);

        public bool IsLoggedInAs(string name)
        {
            return LoggedInAsLink.Text.Contains(name, StringComparison.OrdinalIgnoreCase);
        }

        public string SubscribeToNewsletter(string email)
        {
            SubscribeEmailField.Clear();
            SubscribeEmailField.SendKeys(email);
            SubscribeButton.SafeClick(driver);

            return SubscriptionSuccessMessage.Text;
        }

        public void ExpandCategoryPanel(string departmentName)
        {
            wait.WaitForClickable(By.CssSelector($"a[href='#{departmentName}']")).SafeClick(driver);
        }

        public void SelectCategory(string departmentName, string categoryName)
        {
            ExpandCategoryPanel(departmentName);
            wait.WaitForClickable(By.XPath($"//div[@id='{departmentName}']//a[contains(text(),'{categoryName}')]")).SafeClick(driver);
        }

        public void SelectBrand(string brandName)
        {
            wait.WaitForClickable(By.CssSelector($"a[href='/brand_products/{brandName}']")).SafeClick(driver);
        }
    }
}
