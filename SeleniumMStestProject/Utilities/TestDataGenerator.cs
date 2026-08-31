namespace SeleniumMStestProject.Utilities
{
    // Generates unique signup data per test run so repeated runs against the
    // live site never collide on "email already exists".
    internal static class TestDataGenerator
    {
        public static string UniqueEmail(string prefix)
        {
            return $"{prefix}_{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}@example.com";
        }

        public static string UniqueName(string prefix)
        {
            return $"{prefix} {Guid.NewGuid():N}".Substring(0, prefix.Length + 9);
        }
    }
}
