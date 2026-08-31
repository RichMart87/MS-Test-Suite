namespace SeleniumMStestProject.Base
{
    public abstract class ApiTestBase
    {
        protected HttpClient Client { get; private set; } = null!;

        [TestInitialize]
        public void BaseSetUp()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(Config.ApiBaseUrl)
            };

            // Some hosts (this API is fronted by Cloudflare) treat a request
            // with no User-Agent as more bot-like than one with a normal
            // browser UA. HttpClient sends none by default.
            Client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        }

        [TestCleanup]
        public void BaseTearDown()
        {
            Client?.Dispose();
        }
    }
}
