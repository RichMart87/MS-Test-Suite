using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumMStestProject.Controls;
using SeleniumMStestProject.Utilities;
using OpenQA.Selenium.Interactions;
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
        public IWebElement DragAndDropA => wait.WaitForVisible(By.Id("drop1"));
        public IWebElement DragAndDropB => wait.WaitForVisible(By.Id("drop2"));
        public IWebElement DragAndDropLogo => wait.WaitForVisible(By.Id("logo"));
        public IWebElement InputSliderControl => wait.WaitForVisible(By.Id("mySlider"));
        public IWebElement SliderProgressBar => wait.WaitForVisible(By.Id("progressBar"));
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

        public void HoverOverNavigationDropdownAndSelectLinkOne()
        {
            var actions = new OpenQA.Selenium.Interactions.Actions(driver);
            actions.MoveToElement(NavigationDropdownMenu).Perform();
            NavigationLinkOne.Click();
        }

        public void HoverOverNavigationDropdownAndSelectLinkTwo()
        {
            var actions = new OpenQA.Selenium.Interactions.Actions(driver);
            actions.MoveToElement(NavigationDropdownMenu).Perform();
            NavigationLinkTwo.Click();
        }
        public void HoverOverNavigationDropdownAndSelectLinkThree()
        {
            var actions = new OpenQA.Selenium.Interactions.Actions(driver);
            actions.MoveToElement(NavigationDropdownMenu).Perform();
            NavigationLinkThree.Click();
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

        public void IncreaseSliderValueTo(int targetValue)
        {
            var slider = InputSliderControl;
            int currentValue = int.Parse(slider.GetAttribute("value"));

            while (currentValue < targetValue)
            {
                slider.SendKeys(Keys.ArrowRight);
                currentValue = int.Parse(slider.GetAttribute("value"));
            }

            Assert.AreEqual(targetValue, currentValue);
        }

        public int GetSliderValue()
        {
            return int.Parse(SliderProgressBar.GetAttribute("value"));
        }

        public bool IsMyCheckboxSelected()
        {
            return SingleCheckbox.Selected;
        }

        public string GetMyLinkText()
        {
            return UrlLink.Text;
        }

        public string TypedText => textInputField.Value;

        public string SelectedDropdownOptionText => new SelectElement(DropDownSelect).SelectedOption.Text;

        public void SelectCheckbox()
        {
            if (!SingleCheckbox.Selected)
            {
                SingleCheckbox.Click();
            }
            Assert.IsTrue(SingleCheckbox.Selected);
        }

        // Firefox/geckodriver doesn't reliably fire native HTML5 dragstart/dragover/drop
        // DOM events from Actions.DragAndDrop's synthetic mouse sequence, even though the
        // same call works fine in Chrome/Edge. Dispatching the events directly via JS
        // sidesteps that gap and works identically across all three browsers.
        private const string SimulateHtml5DragAndDropScript = @"
            function createEvent(typeOfEvent) {
                var event = new CustomEvent(typeOfEvent, { bubbles: true, cancelable: true });
                event.dataTransfer = {
                    data: {},
                    setData: function (key, value) { this.data[key] = value; },
                    getData: function (key) { return this.data[key]; }
                };
                return event;
            }

            function dispatchEvent(element, event, transferData) {
                if (transferData !== undefined) {
                    event.dataTransfer = transferData;
                }
                element.dispatchEvent(event);
            }

            var source = arguments[0];
            var target = arguments[1];

            var dragStartEvent = createEvent('dragstart');
            dispatchEvent(source, dragStartEvent);

            var dragEnterEvent = createEvent('dragenter');
            dispatchEvent(target, dragEnterEvent, dragStartEvent.dataTransfer);

            var dragOverEvent = createEvent('dragover');
            dispatchEvent(target, dragOverEvent, dragStartEvent.dataTransfer);

            var dropEvent = createEvent('drop');
            dispatchEvent(target, dropEvent, dragStartEvent.dataTransfer);

            var dragEndEvent = createEvent('dragend');
            dispatchEvent(source, dragEndEvent, dropEvent.dataTransfer);
        ";

        public void DragAndDropAToB()
        {
            if (driver is not IJavaScriptExecutor js)
            {
                throw new NotSupportedException("Drag-and-drop simulation requires a JavaScript-capable driver.");
            }

            js.ExecuteScript(SimulateHtml5DragAndDropScript, DragAndDropLogo, DragAndDropB);
        }

        public bool IsLogoInDropZone(IWebElement dropZone)
        {
            return dropZone.FindElements(By.Id("logo")).Count > 0;
        }
    }
}