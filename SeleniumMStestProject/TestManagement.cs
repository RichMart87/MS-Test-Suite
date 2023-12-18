using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestManagement.Selenium
{
    [TestClass]
    public class TestClass
    {
        private IWebDriver driver;


        [TestInitialize]
        public void TestInit()
        {
            this.driver = new ChromeDriver();
        }

        [TestCleanup]
        public void TearDown()
        {
            if (this.driver != null)
            {
                this.driver.Quit();
            }
        }


    }
}

