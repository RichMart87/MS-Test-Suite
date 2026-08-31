namespace SeleniumMStestProject.Toggles
{
    internal static class FeatureToggle
    {
        /* Gates the product-review submission step in the Regression suite, so
         * CI can be switched off from posting reviews to the public review form
         * without touching test code. Defaults to enabled if the key is missing
         * or not a valid bool.
         */
        public static bool EnableProductReviewTest
        {
            get
            {
                var raw = Config.GetSetting("FeatureToggle.EnableProductReviewTest");
                return !bool.TryParse(raw, out var isEnabled) || isEnabled;
            }
        }
    }
}
