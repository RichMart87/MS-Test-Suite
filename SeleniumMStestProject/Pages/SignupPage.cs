using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Pages
{
    // Registration model for the account-information form at /signup.
    internal record AccountDetails(
        string Password,
        string FirstName,
        string LastName,
        string Address1,
        string Country,
        string State,
        string City,
        string Zipcode,
        string MobileNumber);

    internal class SignupPage
    {
        private readonly IWebDriver driver;
        private readonly WaitHelper wait;

        public SignupPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
        }

        // Account information form
        public IWebElement GenderMrRadio => wait.WaitForClickable(By.Id("id_gender1"));
        public IWebElement PasswordField => wait.WaitForVisible(By.Id("password"));
        public IWebElement DaysSelect => wait.WaitForVisible(By.Id("days"));
        public IWebElement MonthsSelect => wait.WaitForVisible(By.Id("months"));
        public IWebElement YearsSelect => wait.WaitForVisible(By.Id("years"));
        public IWebElement FirstNameField => wait.WaitForVisible(By.Id("first_name"));
        public IWebElement LastNameField => wait.WaitForVisible(By.Id("last_name"));
        public IWebElement Address1Field => wait.WaitForVisible(By.Id("address1"));
        public IWebElement CountrySelect => wait.WaitForVisible(By.Id("country"));
        public IWebElement StateField => wait.WaitForVisible(By.Id("state"));
        public IWebElement CityField => wait.WaitForVisible(By.Id("city"));
        public IWebElement ZipcodeField => wait.WaitForVisible(By.Id("zipcode"));
        public IWebElement MobileNumberField => wait.WaitForVisible(By.Id("mobile_number"));
        public IWebElement CreateAccountButton => wait.WaitForClickable(By.CssSelector("[data-qa='create-account']"));

        // Confirmation pages
        public IWebElement AccountCreatedHeading => wait.WaitForVisible(By.CssSelector("[data-qa='account-created']"));
        public IWebElement AccountDeletedHeading => wait.WaitForVisible(By.CssSelector("[data-qa='account-deleted']"));
        public IWebElement ContinueButton => wait.WaitForClickable(By.CssSelector("[data-qa='continue-button']"));

        public void FillAccountInformation(AccountDetails details)
        {
            GenderMrRadio.SafeClick(driver);

            PasswordField.Clear();
            PasswordField.SendKeys(details.Password);

            DaysSelect.SelectByValueViaJs(driver, "10");
            MonthsSelect.SelectByValueViaJs(driver, "5");
            YearsSelect.SelectByValueViaJs(driver, "1990");

            FirstNameField.Clear();
            FirstNameField.SendKeys(details.FirstName);
            LastNameField.Clear();
            LastNameField.SendKeys(details.LastName);

            Address1Field.Clear();
            Address1Field.SendKeys(details.Address1);

            CountrySelect.SelectByTextViaJs(driver, details.Country);

            StateField.Clear();
            StateField.SendKeys(details.State);
            CityField.Clear();
            CityField.SendKeys(details.City);
            ZipcodeField.Clear();
            ZipcodeField.SendKeys(details.Zipcode);
            MobileNumberField.Clear();
            MobileNumberField.SendKeys(details.MobileNumber);
        }

        public void SubmitAccountInformation()
        {
            CreateAccountButton.SafeClick(driver);
        }

        public void ContinueFromConfirmation()
        {
            ContinueButton.SafeClick(driver);
        }
    }
}
