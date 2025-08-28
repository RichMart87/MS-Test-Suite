using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Pages
{
    internal class TestPageLanding
    {
        private IWebDriver driver;

        public TestPageLanding(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement Button => driver.FindElement(By.XPath("//*[@id='myButton']"));

        public IWebElement TextInputField => driver.FindElement(By.XPath("//*[@id='myTextInput']"));

        public IWebElement DropDownSelect => driver.FindElement(By.XPath("//*[@id='mySelect']"));

        public IWebElement SingleCheckbox => driver.FindElement(By.Id("checkBox1"));

        public IWebElement UrlLink => driver.FindElement(By.Id("myLink1"));

        public void ClickMyButton()
        {
            Button.Click();
        }

        public void EnterTextInMyInput(string text)
        {
            TextInputField.Clear();
            TextInputField.SendKeys(text);

            Assert.AreEqual(text, TextInputField.GetAttribute("value"));
        }

        public void SelectOptionInMyDropdown(string optionText)
        {
            // You can use different logic based on your dropdown implementation
            // Example using SelectElement:
            var select = new SelectElement(DropDownSelect);
            select.SelectByText(optionText);

            Assert.AreEqual(optionText, select.SelectedOption.Text);
        }

        public bool IsMyCheckboxSelected()
        {
            return SingleCheckbox.Selected;
        }

        public string GetMyLinkText()
        {
            return UrlLink.Text;
        }
    }
}