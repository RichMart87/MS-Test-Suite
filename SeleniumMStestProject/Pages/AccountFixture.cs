using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Pages
{
    internal static class AccountFixture
    {
        public const string Password = "TestPass123!";

        public static (string Name, string Email) Register(IWebDriver driver, string prefix)
        {
            var name = TestDataGenerator.UniqueName(prefix);
            var email = TestDataGenerator.UniqueEmail(prefix);

            var loginPage = new LoginPage(driver);
            loginPage.GoTo();
            loginPage.StartSignup(name, email);

            var signupPage = new SignupPage(driver);
            signupPage.FillAccountInformation(new AccountDetails(
                Password: Password,
                FirstName: name,
                LastName: "Tester",
                Address1: "123 Test Street",
                Country: "United States",
                State: "CA",
                City: "Testville",
                Zipcode: "90210",
                MobileNumber: "5555555555"));
            signupPage.SubmitAccountInformation();

            Assert.IsTrue(signupPage.AccountCreatedHeading.Displayed, "Account should be created.");
            signupPage.ContinueFromConfirmation();

            return (name, email);
        }

        public static void Delete(IWebDriver driver)
        {
            var homePage = new HomePage(driver);
            homePage.DeleteAccount();

            var signupPage = new SignupPage(driver);
            Assert.IsTrue(signupPage.AccountDeletedHeading.Displayed, "Account should be deleted.");
            signupPage.ContinueFromConfirmation();
        }
    }
}
