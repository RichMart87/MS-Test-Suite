using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumMStestProject
{
    internal class TestPage
    {
        private IWebDriver driver = new ChromeDriver();
        private IWebElement TextInputField => driver.FindElement(By.Id("myTextInput"));
    }
}