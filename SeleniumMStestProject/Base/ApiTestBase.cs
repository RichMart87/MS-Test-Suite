using System.Net.Http;

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
        }

        [TestCleanup]
        public void BaseTearDown()
        {
            Client?.Dispose();
        }
    }
}
