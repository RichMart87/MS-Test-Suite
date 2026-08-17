using SeleniumMStestProject.Base;
using SeleniumMStestProject.Constants;
using SeleniumMStestProject.Pages;

namespace SeleniumMStestProject.Tests.Regression
{
    [TestClass]
    [TestCategory(TestCategories.Regression)]
    public class RegressionTests : SeleniumTestBase
    {
        private TestPageLanding testPage = null!;

        [TestInitialize]
        public void Setup()
        {
            testPage = new TestPageLanding(Driver);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void SingleCheckboxRemainsSelectedAfterOtherPageInteractions()
        {
            testPage.GoToTestPage();

            testPage.CheckAllCheckboxes();
            testPage.SelectOptionInMyDropdown("Set to 75%");

            Assert.IsTrue(testPage.IsMyCheckboxSelected());
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void NavigationLinkTextMatchesExpectedLabel()
        {
            testPage.GoToTestPage();

            var linkText = testPage.GetMyLinkText();

            Assert.IsFalse(string.IsNullOrWhiteSpace(linkText));
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void SelectingCheckboxExpandsDragAndDropArea()
        {
            testPage.GoToTestPage();
            testPage.SelectCheckbox();
            Assert.IsTrue(testPage.DragAndDropA.Displayed, "DragAndDropA should be displayed after selecting the checkbox.");
            Assert.IsTrue(testPage.DragAndDropB.Displayed, "DragAndDropB should be displayed after selecting the checkbox.");
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void ExpandingDragAndDropAreaCanBeMovedAtoB()
        {
            testPage.GoToTestPage();
            testPage.SelectCheckbox();
            testPage.DragAndDropAToB();

            Assert.IsTrue(testPage.IsLogoInDropZone(testPage.DragAndDropB), "Logo should be inside DragAndDropB after dragging from A to B.");
            Assert.IsFalse(testPage.IsLogoInDropZone(testPage.DragAndDropA), "Logo should no longer be inside DragAndDropA after dragging from A to B.");
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void HoverOverDropdownMenuSelectLinkOne()
        {
            testPage.GoToTestPage();
            testPage.HoverOverNavigationDropdownAndSelectLinkOne();
            Assert.AreEqual("Link One Selected", testPage.NavigationText.Text);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void HoverOverDropdownMenuSelectLinkTwo()
        {
            testPage.GoToTestPage();
            testPage.HoverOverNavigationDropdownAndSelectLinkTwo();
            Assert.AreEqual("Link Two Selected", testPage.NavigationText.Text);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void HoverOverDropdownMenuSelectLinkThree()
        {
            testPage.GoToTestPage();
            testPage.HoverOverNavigationDropdownAndSelectLinkThree();
            Assert.AreEqual("Link Three Selected", testPage.NavigationText.Text);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.UiMilliseconds)]
        public void SliderValueIncreasesToTargetValue()
        {
            testPage.GoToTestPage();
            testPage.IncreaseSliderValueTo(80);

            Assert.AreEqual(80, testPage.GetSliderValue(), "Slider value should be 80 after increasing.");
        }
    }
}
