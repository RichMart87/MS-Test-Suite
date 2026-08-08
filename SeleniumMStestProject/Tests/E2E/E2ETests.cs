using SeleniumMStestProject.Base;
using SeleniumMStestProject.Constants;
using SeleniumMStestProject.Enums;
using SeleniumMStestProject.Pages;
using SeleniumMStestProject.Toggles;

namespace SeleniumMStestProject.Tests.E2E
{
    [TestClass]
    [TestCategory(TestCategories.E2E)]
    public class E2ETests : SeleniumTestBase
    {
        [TestMethod]
        [DataRow(BrowserType.Chrome)]
        [DataRow(BrowserType.Firefox)]
        [DataRow(BrowserType.Edge)]
        public void UserCanCompleteFullDemoPageJourney(BrowserType browserType)
        {
            InitializeDriver(browserType);
            var testPage = new TestPageLanding(Driver);

            testPage.GoToTestPage();

            testPage.SelectOptionInMyDropdown("Set to 75%");
            testPage.EnterTextInMyInput("End-to-end journey text.");
            testPage.CheckAllCheckboxes();

            //added this to test the feature toggle for the navigation dropdown
            if (!FeatureToggle.EnableNavigationDropdownTest)
            {
                Assert.Inconclusive("Navigation dropdown step is disabled via FeatureToggle.EnableNavigationDropdownTest.");
            }

            testPage.HoverOverNavigationDropdownAndSelectLinkThree();

            Assert.IsTrue(testPage.IsMyCheckboxSelected());
        }
    }
}
