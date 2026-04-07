namespace SeleniumMStestProject.Constants
{
    public sealed class Timeout
    {
        public static TimeSpan Quick
        { get { return TimeSpan.FromSeconds(0.5); } }

        public static TimeSpan Standard
        { get { return TimeSpan.FromSeconds(2.5); } }

        public static TimeSpan Medium
        { get { return TimeSpan.FromSeconds(5); } }

        public static TimeSpan Long
        { get { return TimeSpan.FromSeconds(15); } }

        public static TimeSpan ExtraLong
        { get { return TimeSpan.FromSeconds(30); } }
    }
}