using SeleniumMStestProject.Base;
using SeleniumMStestProject.Constants;
using SeleniumMStestProject.Pages;

namespace SeleniumMStestProject.Tests.E2E
{
    [TestClass]
    [TestCategory(TestCategories.E2E)]
    public class E2ETests : SeleniumTestBase
    {
        private TestPageLanding testPage = null!;

        [TestInitialize]
        public void Setup()
        {
            testPage = new TestPageLanding(Driver);
        }

        [TestMethod]
        public void UserCanCompleteFullDemoPageJourney()
        {
            testPage.GoToTestPage();

            testPage.SelectOptionInMyDropdown("Set to 75%");
            testPage.EnterTextInMyInput("End-to-end journey text.");
            testPage.CheckAllCheckboxes();
            testPage.HoverOverNavigationDropdownAndSelectLinkThree();

            Assert.IsTrue(testPage.IsMyCheckboxSelected());
        }
    }
}
