using SeleniumMStestProject.Base;
using SeleniumMStestProject.Constants;
using SeleniumMStestProject.Enums;
using SeleniumMStestProject.Pages;

namespace SeleniumMStestProject.Tests.E2E
{
    [TestClass]
    [TestCategory(TestCategories.E2E)]
    public class E2ETests : SeleniumTestBase
    {
        [TestMethod]
        [Timeout(TestExecutionTimeouts.E2eMilliseconds)]
        [DataRow(BrowserType.Chrome)]
        [DataRow(BrowserType.Firefox)]
        [DataRow(BrowserType.Edge)]
        public void UserCanRegisterAndDeleteAccount(BrowserType browserType)
        {
            InitializeDriver(browserType);

            var (name, _) = AccountFixture.Register(Driver, "e2e_register");

            var homePage = new HomePage(Driver);
            Assert.IsTrue(homePage.IsLoggedInAs(name), "Should be logged in as the newly registered user.");

            AccountFixture.Delete(Driver);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.E2eMilliseconds)]
        public void UserCanLoginThenLogout()
        {
            var (name, email) = AccountFixture.Register(Driver, "e2e_login");

            var homePage = new HomePage(Driver);
            homePage.Logout();

            var loginPage = new LoginPage(Driver);
            loginPage.Login(email, AccountFixture.Password);

            Assert.IsTrue(homePage.IsLoggedInAs(name), "Should be logged in after supplying valid credentials.");

            homePage.Logout();
            Assert.IsTrue(loginPage.LoginEmailField.Displayed, "Should return to the login page after logging out.");

            loginPage.Login(email, AccountFixture.Password);
            AccountFixture.Delete(Driver);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.E2eMilliseconds)]
        public void PlaceOrderRegisterBeforeCheckout()
        {
            AccountFixture.Register(Driver, "e2e_order_register");

            var productsPage = new ProductsPage(Driver);
            productsPage.GoTo();
            productsPage.AddToCartByName("Blue Top");
            productsPage.ViewCartFromModal();

            var cartPage = new CartPage(Driver);
            cartPage.ProceedToCheckout();

            var checkoutPage = new CheckoutPage(Driver);
            checkoutPage.AddOrderComment("Automated E2E order - Test Case 15.");
            checkoutPage.PlaceOrder();

            var paymentPage = new PaymentPage(Driver);
            paymentPage.PayWith(new CardDetails(
                NameOnCard: "QA Tester",
                CardNumber: "4111111111111111",
                Cvc: "123",
                ExpiryMonth: "12",
                ExpiryYear: "2030"));

            Assert.IsTrue(paymentPage.OrderPlacedHeading.Displayed, "Order should be placed successfully.");
            Assert.IsTrue(paymentPage.DownloadInvoiceLink.Displayed, "A download-invoice link should be available after the order is placed.");

            AccountFixture.Delete(Driver);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.E2eMilliseconds)]
        public void PlaceOrderLoginBeforeCheckout()
        {
            var (_, email) = AccountFixture.Register(Driver, "e2e_order_login");

            var homePage = new HomePage(Driver);
            homePage.Logout();

            var loginPage = new LoginPage(Driver);
            loginPage.Login(email, AccountFixture.Password);

            var productsPage = new ProductsPage(Driver);
            productsPage.GoTo();
            productsPage.AddToCartByName("Blue Top");
            productsPage.ViewCartFromModal();

            var cartPage = new CartPage(Driver);
            cartPage.ProceedToCheckout();

            var checkoutPage = new CheckoutPage(Driver);
            checkoutPage.AddOrderComment("Automated E2E order - Test Case 16.");
            checkoutPage.PlaceOrder();

            var paymentPage = new PaymentPage(Driver);
            paymentPage.PayWith(new CardDetails(
                NameOnCard: "QA Tester",
                CardNumber: "4111111111111111",
                Cvc: "123",
                ExpiryMonth: "12",
                ExpiryYear: "2030"));

            Assert.IsTrue(paymentPage.OrderPlacedHeading.Displayed, "Order should be placed successfully.");

            AccountFixture.Delete(Driver);
        }
    }
}
