using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumMStestProject.Controls;
using SeleniumMStestProject.Utilities;
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
        private WaitHelper wait;
        private TextFieldControl textInputField;

        public TestPageLanding(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
            textInputField = new TextFieldControl(driver, FindItBy.Id, "myTextInput");
        }

        public IWebElement Button => wait.WaitForClickable(By.Id("myButton"));

        public IWebElement DropDownSelect => wait.WaitForClickable(By.Id("mySelect"));

        public IWebElement NavigationDropdownMenu => wait.WaitForVisible(By.Id("myDropdown"));
        public IWebElement NavigationLinkOne => wait.WaitForClickable(By.Id("dropOption1"));
        public IWebElement NavigationLinkTwo => wait.WaitForClickable(By.Id("dropOption2"));
        public IWebElement NavigationLinkThree => wait.WaitForClickable(By.Id("dropOption3"));

        // The demo page has exactly one <h3> (its own JS looks it up via
        // document.querySelector("h3")), so this is as stable as an id.
        public IWebElement NavigationText => wait.WaitForVisible(By.CssSelector("h3"));

        public IWebElement SingleCheckbox => wait.WaitForClickable(By.Id("checkBox1"));

        public IWebElement CheckBoxesA => wait.WaitForClickable(By.Id("checkBox2"));
        public IWebElement CheckBoxesB => wait.WaitForClickable(By.Id("checkBox3"));
        public IWebElement CheckBoxesC => wait.WaitForClickable(By.Id("checkBox4"));

        public IWebElement UrlLink => wait.WaitForVisible(By.Id("myLink1"));

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
            textInputField.EnterText(text);

            Assert.AreEqual(text, textInputField.Value);
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