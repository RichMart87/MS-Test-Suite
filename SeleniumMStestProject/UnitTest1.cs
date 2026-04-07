using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumMStestProject.Pages;
using System.Configuration;
using TestManagement.Selenium;

namespace SeleniumMStestProject
{
    [TestClass]
    public class InitialTests
    {
        private ChromeDriver driver;
        private TestPageLanding testPage;

        [TestInitialize]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            testPage = new TestPageLanding(driver);
        }

        [TestMethod]
        public void GoToBaseTestPage()
        {
            var baseUrl = Config.BaseUrl;
            Console.WriteLine($"Base URL: {baseUrl}");

            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(baseUrl);

            var currentPage = driver.Url;

            Assert.AreEqual( baseUrl, currentPage );
        }

        [TestMethod]
        public void WhenUserGoToTestPageCanFillAllTextFields()
        {
            testPage.GoToTestPage();
            testPage.SelectOptionInMyDropdown("Set to 75%");

            testPage.EnterTextInMyInput("This is test text.");
        }

        [TestCleanup] public void Cleanup()
        {
            if (this.driver != null)
            {
                this.driver.Quit();
                this.driver.Dispose();
                this.driver = null;
            }
        }
    }
}