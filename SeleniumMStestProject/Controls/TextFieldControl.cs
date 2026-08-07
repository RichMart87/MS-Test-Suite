using OpenQA.Selenium;

namespace SeleniumMStestProject.Controls
{
    internal class TextFieldControl : Control
    {
        ///<summary>
        ///Constructor
        /// </summary>
        /// <param name="driver">The active WebDriver session</param>
        /// <param name="pLocator">ID||Name||XPath</param>
        /// <param name="pControlLocation">Location based on ID,Name, or XPath</param>
        public TextFieldControl(IWebDriver driver, FindItBy pLocator, string pControlLocation)
            : base(driver, pLocator, pControlLocation) { }

        public string Value => ClickableElement.GetAttribute("value") ?? string.Empty;

        public void EnterText(string text)
        {
            var element = ClickableElement;
            element.Clear();
            element.SendKeys(text);
        }
    }
}
