namespace SeleniumMStestProject.Controls
{
    internal class TextFieldControl : Control
    {
        ///<summary>
        ///Constructor
        /// </summary>
        /// <param name="pLocator">ID||Name||XPath</param>
        /// <param name="pControlLocation">Location based on ID,Name, or XPath</param>
        public TextFieldControl(FindItBy pLocator, string pControlLocation)
            : base(pLocator, pControlLocation) { }
    }
}