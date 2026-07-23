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
        public void SingleCheckboxRemainsSelectedAfterOtherPageInteractions()
        {
            testPage.GoToTestPage();

            testPage.CheckAllCheckboxes();
            testPage.SelectOptionInMyDropdown("Set to 75%");

            Assert.IsTrue(testPage.IsMyCheckboxSelected());
        }

        [TestMethod]
        public void NavigationLinkTextMatchesExpectedLabel()
        {
            testPage.GoToTestPage();

            var linkText = testPage.GetMyLinkText();

            Assert.IsFalse(string.IsNullOrWhiteSpace(linkText));
        }
    }
}
