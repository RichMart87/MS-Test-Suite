using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Pages
{
    internal class LoginPage
    {
        private readonly IWebDriver driver;
        private readonly WaitHelper wait;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
        }

        public void GoTo()
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl($"{Config.BaseUrl}/login");
        }

        // Login form
        public IWebElement LoginEmailField => wait.WaitForVisible(By.CssSelector("[data-qa='login-email']"));
        public IWebElement LoginPasswordField => wait.WaitForVisible(By.CssSelector("[data-qa='login-password']"));
        public IWebElement LoginButton => wait.WaitForClickable(By.CssSelector("[data-qa='login-button']"));
        public IWebElement LoginErrorMessage => wait.WaitForVisible(By.XPath("//p[contains(text(),'incorrect')]"));

        // Signup (name + email) form
        public IWebElement SignupNameField => wait.WaitForVisible(By.CssSelector("[data-qa='signup-name']"));
        public IWebElement SignupEmailField => wait.WaitForVisible(By.CssSelector("[data-qa='signup-email']"));
        public IWebElement SignupButton => wait.WaitForClickable(By.CssSelector("[data-qa='signup-button']"));
        public IWebElement SignupErrorMessage => wait.WaitForVisible(By.XPath("//p[contains(text(),'already exist')]"));

        public void Login(string email, string password)
        {
            LoginEmailField.Clear();
            LoginEmailField.SendKeys(email);
            LoginPasswordField.Clear();
            LoginPasswordField.SendKeys(password);
            LoginButton.SafeClick(driver);
        }

        public void StartSignup(string name, string email)
        {
            SignupNameField.Clear();
            SignupNameField.SendKeys(name);
            SignupEmailField.Clear();
            SignupEmailField.SendKeys(email);
            SignupButton.SafeClick(driver);
        }
    }
}
