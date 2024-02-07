using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Configuration;
using TestManagement.Selenium;

namespace SeleniumMStestProject
{
    [TestClass]
    public class InitialTests
    {
        private ChromeDriver driver;
        [TestInitialize] public void Setup()
        {
            this.driver = new ChromeDriver();

        }



        [TestMethod]
        public void GoToTestPage()
        {
            var baseUrl = Config.BaseUrl;
            Console.WriteLine($"Base URL: {baseUrl}");
            
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl( baseUrl );

            var currentPage = driver.Url;

            Assert.AreEqual( baseUrl, currentPage );
            
        }

        [TestMethod]
        public void WhenUserGoToTestPageCanFillAllTextFields()
        {

        }

        [TestCleanup] public void Cleanup()
        {
            if ( this.driver != null )
            {
                this.driver.Quit();
            }
        }
    }
}