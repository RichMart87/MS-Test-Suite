namespace SeleniumMStestProject.Toggles
{
    internal static class FeatureToggle
    {
        // Gates the navigation-dropdown step in the E2E suite. Defaults to
        // enabled if the key is missing or not a valid bool.
        public static bool EnableNavigationDropdownTest
        {
            get
            {
                var raw = Config.GetSetting("FeatureToggle.EnableNavigationDropdownTest");
                return !bool.TryParse(raw, out var isEnabled) || isEnabled;
            }
        }
    }
}
