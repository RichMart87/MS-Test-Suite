using SeleniumMStestProject.Base;
using SeleniumMStestProject.Constants;
using SeleniumMStestProject.Pages;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Tests.Smoke
{
    [TestClass]
    [TestCategory(TestCategories.Smoke)]
    public class SmokeTests : SeleniumTestBase
    {
        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void HomePageLoads()
        {
            var homePage = new HomePage(Driver);
            homePage.GoTo();

            Assert.AreEqual("Automation Exercise", Driver.Title);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void TestCasesPageIsReachableFromNav()
        {
            var homePage = new HomePage(Driver);
            homePage.GoTo();
            homePage.GoToTestCases();

            Assert.IsTrue(Driver.Url.Contains("/test_cases"), "Should navigate to the Test Cases page.");
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void SubscriptionOnHomePageSucceeds()
        {
            var homePage = new HomePage(Driver);
            homePage.GoTo();

            var message = homePage.SubscribeToNewsletter(TestDataGenerator.UniqueEmail("smoke_subscribe"));

            Assert.AreEqual("You have been successfully subscribed!", message);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void SearchProductReturnsMatchingResults()
        {
            var productsPage = new ProductsPage(Driver);
            productsPage.GoTo();
            productsPage.Search("Top");

            Assert.AreEqual("Searched Products", productsPage.NormalizedGridHeading, ignoreCase: true);
            Assert.IsTrue(productsPage.ProductCards.Count > 0, "Search should return at least one product.");
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void AddProductToCartShowsConfirmation()
        {
            var productsPage = new ProductsPage(Driver);
            productsPage.GoTo();
            productsPage.AddToCartByName("Blue Top");

            Assert.IsTrue(productsPage.CartModal.Displayed, "Add-to-cart confirmation modal should appear.");

            productsPage.ViewCartFromModal();

            var cartPage = new CartPage(Driver);
            Assert.IsTrue(cartPage.ContainsProduct("Blue Top"), "Cart should contain the added product.");
        }
    }
}
