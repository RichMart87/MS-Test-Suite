using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Pages
{
    internal class ContactUsPage
    {
        private readonly IWebDriver driver;
        private readonly WaitHelper wait;

        public ContactUsPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
        }

        public void GoTo()
        {
            driver.Navigate().GoToUrl($"{Config.BaseUrl}/contact_us");
        }

        public IWebElement NameField => wait.WaitForVisible(By.CssSelector("[data-qa='name']"));
        public IWebElement EmailField => wait.WaitForVisible(By.CssSelector("[data-qa='email']"));
        public IWebElement SubjectField => wait.WaitForVisible(By.CssSelector("[data-qa='subject']"));
        public IWebElement MessageField => wait.WaitForVisible(By.CssSelector("[data-qa='message']"));
        public IWebElement SubmitButton => wait.WaitForClickable(By.CssSelector("[data-qa='submit-button']"));
        public IWebElement SuccessMessage => wait.WaitForVisible(By.CssSelector("div.status.alert-success"));

        public string SubmitContactForm(string name, string email, string subject, string message)
        {
            NameField.SendKeys(name);
            EmailField.SendKeys(email);
            SubjectField.SendKeys(subject);
            MessageField.SendKeys(message);

            // The submit button triggers a confirm dialog before posting
            SubmitButton.SafeClick(driver);
            driver.SwitchTo().Alert().Accept();

            return SuccessMessage.Text;
        }
    }
}
