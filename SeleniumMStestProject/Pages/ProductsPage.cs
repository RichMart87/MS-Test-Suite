using OpenQA.Selenium;
using SeleniumMStestProject.Utilities;
using System.Text.RegularExpressions;

namespace SeleniumMStestProject.Pages
{
    internal class ProductsPage
    {
        private readonly IWebDriver driver;
        private readonly WaitHelper wait;

        public ProductsPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WaitHelper(driver);
        }

        public void GoTo()
        {
            driver.Navigate().GoToUrl($"{Config.BaseUrl}/products");
        }

        public IWebElement SearchField => wait.WaitForVisible(By.Id("search_product"));
        public IWebElement SearchButton => wait.WaitForClickable(By.Id("submit_search"));
        public IWebElement GridHeading => wait.WaitForVisible(By.CssSelector(".features_items .title"));

        /* The heading mixes an always uppercase template ("BRAND -", "PRODUCTS")
         with  embedded category/brand name that keeps its own case and can
         carry stray whitespace from the source link text (ex. "Dress "), so
         exact text assertions are normalized here instead of at every call site*/
        public string NormalizedGridHeading => Regex.Replace(GridHeading.Text, @"\s+", " ").Trim();
        public IReadOnlyCollection<IWebElement> ProductCards => driver.FindElements(By.CssSelector(".features_items .product-image-wrapper"));

        public IWebElement CartModal => wait.WaitForVisible(By.Id("cartModal"));
        public IWebElement ViewCartLinkInModal => wait.WaitForClickable(By.CssSelector("#cartModal a[href='/view_cart']"));
        public IWebElement ContinueShoppingButton => wait.WaitForClickable(By.CssSelector("#cartModal .close-modal"));

        public void Search(string term)
        {
            SearchField.Clear();
            SearchField.SendKeys(term);
            SearchButton.SafeClick(driver);
        }

        public void AddToCartByName(string productName)
        {
            AddToCartLinkFor(productName).SafeClick(driver);
        }

        public void ViewProductByName(string productName)
        {
            ViewProductLinkFor(productName).SafeClick(driver);
        }

        public void ContinueShoppingFromModal() => ContinueShoppingButton.SafeClick(driver);

        public void ViewCartFromModal() => ViewCartLinkInModal.SafeClick(driver);

        public void SelectBrand(string brandName)
        {
            wait.WaitForClickable(By.CssSelector($"a[href='/brand_products/{brandName}']")).SafeClick(driver);
        }

        public bool HasProductNamed(string productName)
        {
            return ProductCards.Any(card => card.FindElements(By.TagName("p")).Any(p => string.Equals(p.Text.Trim(), productName, StringComparison.Ordinal)));
        }

        private IWebElement AddToCartLinkFor(string productName)
        {
            return wait.WaitForClickable(By.XPath(
                $"//div[contains(@class,'product-image-wrapper')][.//p[normalize-space(text())='{productName}']]//a[contains(@class,'add-to-cart')]"));
        }

        private IWebElement ViewProductLinkFor(string productName)
        {
            return wait.WaitForClickable(By.XPath(
                $"//div[contains(@class,'product-image-wrapper')][.//p[normalize-space(text())='{productName}']]//a[contains(text(),'View Product')]"));
        }
    }
}
