using OpenQA.Selenium;
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

        public IWebElement NavigationDropdownMenu => driver.FindElement(By.Id("myDropdown"));
        public IWebElement NavigationLinkOne => driver.FindElement(By.Id("dropOption1"));
        public IWebElement NavigationLinkTwo => driver.FindElement(By.Id("dropOption2"));
        public IWebElement NavigationLinkThree => driver.FindElement(By.Id("dropOption3"));
        public IWebElement NavigationText => driver.FindElement(By.XPath("//*[@id='tbodyId']/tr[1]/td[4]/h3"));

        public IWebElement SingleCheckbox => driver.FindElement(By.Id("checkBox1"));

        public IWebElement CheckBoxesA => driver.FindElement(By.Id("checkbox2"));
        public IWebElement CheckBoxesB => driver.FindElement(By.Id("checkbox3"));
        public IWebElement CheckBoxesC => driver.FindElement(By.Id("checkbox4"));

        public IWebElement UrlLink => driver.FindElement(By.Id("myLink1"));

        public void ClickMyButton()
        {
            Button.Click();
        }

        public void GoToTestPage()
        {
            var baseUrl = Config.BaseUrl;
            Console.WriteLine($"Base URL: {baseUrl}");

            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(baseUrl);

            var currentPage = driver.Url;

            Assert.AreEqual(baseUrl, currentPage);
        }

        public void CheckAllCheckboxes()
        {
            if (!SingleCheckbox.Selected)
            {
                SingleCheckbox.Click();
            }
            if (!CheckBoxesA.Selected)
            {
                CheckBoxesA.Click();
            }
            if (!CheckBoxesB.Selected)
            {
                CheckBoxesB.Click();
            }
            if (!CheckBoxesC.Selected)
            {
                CheckBoxesC.Click();
            }
            Assert.IsTrue(SingleCheckbox.Selected);
            Assert.IsTrue(CheckBoxesA.Selected);
            Assert.IsTrue(CheckBoxesB.Selected);
            Assert.IsTrue(CheckBoxesC.Selected);
        }

        public void HoverOverNavigationDropdownAndSelectLinkThree()
        {
            var actions = new OpenQA.Selenium.Interactions.Actions(driver);
            actions.MoveToElement(NavigationDropdownMenu).Perform();
            NavigationLinkThree.Click();
            Assert.AreEqual("Link Three Selected", NavigationText.Text);
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