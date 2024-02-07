using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace SeleniumMStestProject
{

    internal class TestPage
    {
        IWebDriver driver = new ChromeDriver();
        IWebElement TextInputField => driver.FindElement(By.Id("myTextInput"));
    }
}
