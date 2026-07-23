using SeleniumMStestProject.Base;
using SeleniumMStestProject.Constants;
using SeleniumMStestProject.Pages;

namespace SeleniumMStestProject.Tests.Smoke
{
    [TestClass]
    [TestCategory(TestCategories.Smoke)]
    public class SmokeTests : SeleniumTestBase
    {
        private TestPageLanding testPage = null!;

        [TestInitialize]
        public void Setup()
        {
            testPage = new TestPageLanding(Driver);
        }

        [TestMethod]
        public void GoToBaseTestPage()
        {
            var baseUrl = Config.BaseUrl;

            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl(baseUrl);

            Assert.AreEqual(baseUrl, Driver.Url);
        }

        [TestMethod]
        public void WhenUserGoToTestPageCanFillAllTextFields()
        {
            testPage.GoToTestPage();
            testPage.SelectOptionInMyDropdown("Set to 75%");
            testPage.EnterTextInMyInput("This is test text.");
        }
    }
}
