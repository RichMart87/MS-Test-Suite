using OpenQA.Selenium;
using SeleniumMStestProject.Base;
using SeleniumMStestProject.Constants;
using SeleniumMStestProject.Pages;
using SeleniumMStestProject.Toggles;
using SeleniumMStestProject.Utilities;

namespace SeleniumMStestProject.Tests.Regression
{
    [TestClass]
    [TestCategory(TestCategories.Regression)]
    public class RegressionTests : SeleniumTestBase
    {
        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void LoginWithIncorrectCredentialsShowsError()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.GoTo();
            loginPage.Login(TestDataGenerator.UniqueEmail("no_such_user"), "WrongPassword123!");

            Assert.IsTrue(loginPage.LoginErrorMessage.Displayed, "An incorrect-credentials error should be shown.");
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.E2eMilliseconds)]
        public void RegisterWithExistingEmailShowsError()
        {
            var (_, email) = AccountFixture.Register(Driver, "regression_dup");

            var homePage = new HomePage(Driver);
            homePage.Logout();

            var loginPage = new LoginPage(Driver);
            loginPage.StartSignup(TestDataGenerator.UniqueName("regression_dup2"), email);

            Assert.IsTrue(loginPage.SignupErrorMessage.Displayed, "Signing up with an already-registered email should show an error.");

            loginPage.Login(email, AccountFixture.Password);
            AccountFixture.Delete(Driver);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void ContactUsFormSubmitsSuccessfully()
        {
            var contactUsPage = new ContactUsPage(Driver);
            contactUsPage.GoTo();

            var message = contactUsPage.SubmitContactForm(
                name: TestDataGenerator.UniqueName("regression_contact"),
                email: TestDataGenerator.UniqueEmail("regression_contact"),
                subject: "Automated regression check",
                message: "This is an automated Contact Us submission from the Regression suite.");

            Assert.IsTrue(message.Contains("Success", StringComparison.OrdinalIgnoreCase), $"Unexpected confirmation message: {message}");
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void SubscriptionOnCartPageSucceeds()
        {
            var cartPage = new CartPage(Driver);
            cartPage.GoTo();

            var message = cartPage.SubscribeToNewsletter(TestDataGenerator.UniqueEmail("regression_cart_subscribe"));

            Assert.AreEqual("You have been successfully subscribed!", message);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void ProductQuantitySetOnDetailsPageIsReflectedInCart()
        {
            var productsPage = new ProductsPage(Driver);
            productsPage.GoTo();
            productsPage.ViewProductByName("Blue Top");

            var detailsPage = new ProductDetailsPage(Driver);
            detailsPage.SetQuantity(4);
            detailsPage.AddToCart();
            detailsPage.ViewCartFromModal();

            var cartPage = new CartPage(Driver);
            Assert.AreEqual(4, cartPage.GetQuantity("Blue Top"));
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void RemovingProductLeavesCartEmpty()
        {
            var productsPage = new ProductsPage(Driver);
            productsPage.GoTo();
            productsPage.AddToCartByName("Blue Top");
            productsPage.ViewCartFromModal();

            var cartPage = new CartPage(Driver);
            cartPage.RemoveProduct("Blue Top");

            Assert.IsTrue(cartPage.EmptyCartMessage.Displayed, "Cart should be empty after removing the only product.");
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void ViewCategoryProductsShowsSelectedCategory()
        {
            var homePage = new HomePage(Driver);
            homePage.GoTo();
            homePage.SelectCategory("Women", "Dress");

            var productsPage = new ProductsPage(Driver);
            Assert.AreEqual("Women - Dress Products", productsPage.NormalizedGridHeading, ignoreCase: true);
            Assert.IsTrue(productsPage.ProductCards.Count > 0, "Category page should list at least one product.");
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void ViewAndCartBrandProductsAddsToCart()
        {
            var productsPage = new ProductsPage(Driver);
            productsPage.GoTo();
            productsPage.SelectBrand("Polo");

            Assert.AreEqual("Brand - Polo Products", productsPage.NormalizedGridHeading, ignoreCase: true);
            Assert.IsTrue(productsPage.ProductCards.Count > 0, "Brand page should list at least one product.");

            var firstProductName = productsPage.ProductCards.First().FindElement(By.TagName("p")).Text.Trim();
            productsPage.AddToCartByName(firstProductName);
            productsPage.ViewCartFromModal();

            var cartPage = new CartPage(Driver);
            Assert.IsTrue(cartPage.ContainsProduct(firstProductName), "Cart should contain the brand product that was added.");
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void AddReviewOnProduct()
        {
            if (!FeatureToggle.EnableProductReviewTest)
            {
                Assert.Inconclusive("Product review submission is disabled via FeatureToggle.EnableProductReviewTest.");
            }

            var productsPage = new ProductsPage(Driver);
            productsPage.GoTo();
            productsPage.ViewProductByName("Blue Top");

            var detailsPage = new ProductDetailsPage(Driver);
            var message = detailsPage.SubmitReview(
                TestDataGenerator.UniqueName("regression_reviewer"),
                TestDataGenerator.UniqueEmail("regression_reviewer"),
                "Great product, exactly as described. (Automated regression check.)");

            Assert.AreEqual("Thank you for your review.", message);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.E2eMilliseconds)]
        public void AddressDetailsInCheckoutMatchRegistration()
        {
            var (name, _) = AccountFixture.Register(Driver, "regression_checkout");
            var firstName = name.Split(' ')[0];

            var productsPage = new ProductsPage(Driver);
            productsPage.GoTo();
            productsPage.AddToCartByName("Blue Top");
            productsPage.ViewCartFromModal();

            var cartPage = new CartPage(Driver);
            cartPage.ProceedToCheckout();

            var checkoutPage = new CheckoutPage(Driver);
            var addressText = checkoutPage.DeliveryAddressText;

            Assert.IsTrue(addressText.Contains(firstName, StringComparison.OrdinalIgnoreCase), "Delivery address should show the registered first name.");
            Assert.IsTrue(addressText.Contains("123 Test Street", StringComparison.OrdinalIgnoreCase), "Delivery address should show the registered street address.");
            Assert.IsTrue(addressText.Contains("Testville", StringComparison.OrdinalIgnoreCase), "Delivery address should show the registered city.");

            AccountFixture.Delete(Driver);
        }
    }
}
