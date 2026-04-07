

using System.Reflection.Metadata.Ecma335;

namespace SeleniumMStestProject.Controls
{
    internal enum FindItBy
    {
        Id,
        Name,
        DataTerm,
        DataSysTestId,
        PartialCssClass
    }

    internal class Control
    {
        public TimeSpan JqueryWaitTime = Constants.Timeout.Medium;

        public Control(FindItBy pLocator, string pControlLocation)
        {
            WaitForJquery = false;
            var controlMetadata = new MetadataBuilder();

            //this.Name = controlMetadata.ControlName;
            // PageName = controlMetadata.PageName;

            //Locator = pLocator;
            //Location = pControlLocation;

            //TimeOut = Constants.Timeout.Medium;
        }

        public TimeSpan Timeout { get; set; }
        public bool WaitForJquery { get; set; }
    }
}