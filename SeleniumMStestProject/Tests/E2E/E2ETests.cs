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
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
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
            testPage.DragAndDropAToB();

            //added this to test the feature toggle for the navigation dropdown
            if (!FeatureToggle.EnableNavigationDropdownTest)
            {
                Assert.Inconclusive("Navigation dropdown step is disabled via FeatureToggle.EnableNavigationDropdownTest.");
            }

            testPage.HoverOverNavigationDropdownAndSelectLinkThree();

            // Re-check every earlier step's outcome now that the full journey has
            // run, to prove later interactions didn't clobber earlier ones.
            Assert.IsTrue(testPage.IsLogoInDropZone(testPage.DragAndDropB), "Logo should remain in DragAndDropB after the rest of the journey.");
            Assert.IsTrue(testPage.IsMyCheckboxSelected(), "Single checkbox should remain selected after the rest of the journey.");
            Assert.IsTrue(testPage.CheckBoxesA.Selected, "CheckBoxesA should remain selected after the rest of the journey.");
            Assert.IsTrue(testPage.CheckBoxesB.Selected, "CheckBoxesB should remain selected after the rest of the journey.");
            Assert.IsTrue(testPage.CheckBoxesC.Selected, "CheckBoxesC should remain selected after the rest of the journey.");
            Assert.AreEqual("Set to 75%", testPage.SelectedDropdownOptionText, "Dropdown selection should remain after the rest of the journey.");
            Assert.AreEqual("End-to-end journey text.", testPage.TypedText, "Typed text should remain after the rest of the journey.");
            Assert.AreEqual("Link Three Selected", testPage.NavigationText.Text, "Navigation text should reflect Link Three after the rest of the journey.");
        }
    }
}
