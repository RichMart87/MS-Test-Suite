using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Toggles
{
    public enum FeatureToggles
    {
        // Add your feature toggles here
        FeatureA,

        FeatureB,
        FeatureC,
        FeatureD,
        FeatureE,
        FeatureF,
        FeatureG,
        FeatureH,
        FeatureI,
        FeatureJ
    }

    internal class Details
    {
        //Setup TestEnvironmentType in Enums class
        //public TestEnvironmentType EnvironmentType { get; set; }
        private bool IsOnOverride = false;

        private DateTime toggleOffDate = default(DateTime);
        private DateTime toggleOnDate = default(DateTime);

        public Details()
        {
            // Initialize the toggle states
            // This could be loaded from a configuration file or database
            IsOnOverride = false;
            toggleOffDate = DateTime.MinValue;
            toggleOnDate = DateTime.MaxValue;
        }

        public bool IsOn(FeatureToggles feature)
        {
            // Check if the feature is enabled in the configuration
            // This could be a database call, config file read, etc.
            // For now, we'll just return true for demonstration purposes
            return IsOnOverride || IsFeatureEnabled(feature);
        }

        private bool IsFeatureEnabled(FeatureToggles feature)
        {
            // Simulate checking if the feature is enabled
            // In a real application, this would check a config file, database, etc.
            return feature switch
            {
                FeatureToggles.FeatureA => true,
                FeatureToggles.FeatureB => false,
                FeatureToggles.FeatureC => true,
                FeatureToggles.FeatureD => false,
                FeatureToggles.FeatureE => true,
                FeatureToggles.FeatureF => false,
                FeatureToggles.FeatureG => true,
                FeatureToggles.FeatureH => false,
                FeatureToggles.FeatureI => true,
                FeatureToggles.FeatureJ => false,
                _ => false
            };
        }

        public void SetOverride(bool isOn)
        {
            IsOnOverride = isOn;
        }
    }

    internal class FeatureDetails
    {
        public FeatureToggles Feature { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime ToggleOffDate { get; set; }
        public DateTime ToggleOnDate { get; set; }

        public FeatureDetails(FeatureToggles feature, bool isEnabled, DateTime toggleOffDate, DateTime toggleOnDate)
        {
            Feature = feature;
            IsEnabled = isEnabled;
            ToggleOffDate = toggleOffDate;
            ToggleOnDate = toggleOnDate;
        }
    }

    internal class Features
    {
        private List<FeatureDetails> featureDetailsList;

        public Features()
        {
            featureDetailsList = new List<FeatureDetails>
            {
                new FeatureDetails(FeatureToggles.FeatureA, true, DateTime.MinValue, DateTime.MaxValue),
                new FeatureDetails(FeatureToggles.FeatureB, false, DateTime.MinValue, DateTime.MaxValue),
                new FeatureDetails(FeatureToggles.FeatureC, true, DateTime.MinValue, DateTime.MaxValue),
                new FeatureDetails(FeatureToggles.FeatureD, false, DateTime.MinValue, DateTime.MaxValue),
                new FeatureDetails(FeatureToggles.FeatureE, true, DateTime.MinValue, DateTime.MaxValue),
                new FeatureDetails(FeatureToggles.FeatureF, false, DateTime.MinValue, DateTime.MaxValue),
                new FeatureDetails(FeatureToggles.FeatureG, true, DateTime.MinValue, DateTime.MaxValue),
                new FeatureDetails(FeatureToggles.FeatureH, false, DateTime.MinValue, DateTime.MaxValue),
                new FeatureDetails(FeatureToggles.FeatureI, true, DateTime.MinValue, DateTime.MaxValue),
                new FeatureDetails(FeatureToggles.FeatureJ, false, DateTime.MinValue, DateTime.MaxValue)
            };
        }

        public bool IsOn(FeatureToggles feature)
        {
            var featureDetail = featureDetailsList.FirstOrDefault(f => f.Feature == feature);
            return featureDetail != null && featureDetail.IsEnabled;
        }
    }
}