using System.Text.Json;
using SeleniumMStestProject.Base;
using SeleniumMStestProject.Constants;

namespace SeleniumMStestProject.Tests.Api
{
    // API under test: https://automationexercise.com/api_list
    // Note: this API always responds with HTTP 200 at the transport level.
    // The real status is embedded in the JSON body's "responseCode" field.
    [TestClass]
    [TestCategory(TestCategories.Api)]
    public class ApiTests : ApiTestBase
    {
        private const string ProductsListEndpoint = "/api/productsList";
        private const string BrandsListEndpoint = "/api/brandsList";
        private const string SearchProductEndpoint = "/api/searchProduct";
        private const string VerifyLoginEndpoint = "/api/verifyLogin";
        private const string CreateAccountEndpoint = "/api/createAccount";
        private const string DeleteAccountEndpoint = "/api/deleteAccount";
        private const string UpdateAccountEndpoint = "/api/updateAccount";
        private const string GetUserDetailByEmailEndpoint = "/api/getUserDetailByEmail";

        [TestMethod]
        public async Task GetProductsList_ReturnsAllProducts()
        {
            var response = await Client.GetAsync(ProductsListEndpoint);
            var json = await ParseResponseAsync(response);

            Assert.AreEqual(200, GetResponseCode(json));
            Assert.IsGreaterThan(0, json.RootElement.GetProperty("products").GetArrayLength());
        }

        [TestMethod]
        public async Task PostToProductsList_ReturnsMethodNotAllowed()
        {
            var response = await Client.PostAsync(ProductsListEndpoint, null);
            var json = await ParseResponseAsync(response);

            Assert.AreEqual(405, GetResponseCode(json));
        }

        [TestMethod]
        public async Task GetBrandsList_ReturnsAllBrands()
        {
            var response = await Client.GetAsync(BrandsListEndpoint);
            var json = await ParseResponseAsync(response);

            Assert.AreEqual(200, GetResponseCode(json));
            Assert.IsGreaterThan(0, json.RootElement.GetProperty("brands").GetArrayLength());
        }

        [TestMethod]
        public async Task PutToBrandsList_ReturnsMethodNotAllowed()
        {
            var response = await Client.PutAsync(BrandsListEndpoint, null);
            var json = await ParseResponseAsync(response);

            Assert.AreEqual(405, GetResponseCode(json));
        }

        [TestMethod]
        public async Task SearchProduct_WithValidTerm_ReturnsMatchingProducts()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["search_product"] = "top"
            });

            var response = await Client.PostAsync(SearchProductEndpoint, content);
            var json = await ParseResponseAsync(response);

            Assert.AreEqual(200, GetResponseCode(json));
            Assert.IsGreaterThan(0, json.RootElement.GetProperty("products").GetArrayLength());
        }

        [TestMethod]
        public async Task SearchProduct_WithoutSearchTerm_ReturnsBadRequest()
        {
            var response = await Client.PostAsync(SearchProductEndpoint, null);
            var json = await ParseResponseAsync(response);

            Assert.AreEqual(400, GetResponseCode(json));
        }

        [TestMethod]
        public async Task VerifyLogin_WithInvalidCredentials_ReturnsUserNotFound()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["email"] = "no_such_user_qa_regression@example.com",
                ["password"] = "not-the-right-password"
            });

            var response = await Client.PostAsync(VerifyLoginEndpoint, content);
            var json = await ParseResponseAsync(response);

            Assert.AreEqual(404, GetResponseCode(json));
        }

        [TestMethod]
        public async Task VerifyLogin_WithoutEmail_ReturnsBadRequest()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["password"] = "irrelevant"
            });

            var response = await Client.PostAsync(VerifyLoginEndpoint, content);
            var json = await ParseResponseAsync(response);

            Assert.AreEqual(400, GetResponseCode(json));
        }

        [TestMethod]
        public async Task VerifyLogin_UsingDeleteMethod_ReturnsMethodNotAllowed()
        {
            var response = await Client.DeleteAsync(VerifyLoginEndpoint);
            var json = await ParseResponseAsync(response);

            Assert.AreEqual(405, GetResponseCode(json));
        }

        [TestMethod]
        public async Task UserAccountLifecycle_CreateVerifyUpdateDelete_Succeeds()
        {
            var email = $"qa_regression_{Guid.NewGuid():N}@example.com";
            const string password = "Passw0rd!";
            var accountFields = new Dictionary<string, string>
            {
                ["name"] = "QA Regression",
                ["email"] = email,
                ["password"] = password,
                ["title"] = "Mr",
                ["birth_date"] = "1",
                ["birth_month"] = "1",
                ["birth_year"] = "1990",
                ["firstname"] = "QA",
                ["lastname"] = "Regression",
                ["company"] = "Acme",
                ["address1"] = "123 Test St",
                ["address2"] = "",
                ["country"] = "United States",
                ["zipcode"] = "12345",
                ["state"] = "CA",
                ["city"] = "Testville",
                ["mobile_number"] = "1234567890"
            };

            var createResponse = await Client.PostAsync(CreateAccountEndpoint, new FormUrlEncodedContent(accountFields));
            var createJson = await ParseResponseAsync(createResponse);
            Assert.AreEqual(201, GetResponseCode(createJson), "Account creation should succeed.");

            try
            {
                var verifyContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["email"] = email,
                    ["password"] = password
                });
                var verifyResponse = await Client.PostAsync(VerifyLoginEndpoint, verifyContent);
                var verifyJson = await ParseResponseAsync(verifyResponse);
                Assert.AreEqual(200, GetResponseCode(verifyJson), "Newly created account should be able to log in.");

                var getResponse = await Client.GetAsync($"{GetUserDetailByEmailEndpoint}?email={Uri.EscapeDataString(email)}");
                var getJson = await ParseResponseAsync(getResponse);
                Assert.AreEqual(200, GetResponseCode(getJson));
                Assert.AreEqual("Testville", getJson.RootElement.GetProperty("user").GetProperty("city").GetString());

                accountFields["city"] = "Updated City";
                var updateResponse = await Client.PutAsync(UpdateAccountEndpoint, new FormUrlEncodedContent(accountFields));
                var updateJson = await ParseResponseAsync(updateResponse);
                Assert.AreEqual(200, GetResponseCode(updateJson), "Account update should succeed.");

                var getAfterUpdateResponse = await Client.GetAsync($"{GetUserDetailByEmailEndpoint}?email={Uri.EscapeDataString(email)}");
                var getAfterUpdateJson = await ParseResponseAsync(getAfterUpdateResponse);
                Assert.AreEqual("Updated City", getAfterUpdateJson.RootElement.GetProperty("user").GetProperty("city").GetString());
            }
            finally
            {
                var deleteContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["email"] = email,
                    ["password"] = password
                });
                using var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, DeleteAccountEndpoint) { Content = deleteContent };
                await Client.SendAsync(deleteRequest);
            }
        }

        private static async Task<JsonDocument> ParseResponseAsync(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(body);
        }

        private static int GetResponseCode(JsonDocument json)
        {
            return json.RootElement.GetProperty("responseCode").GetInt32();
        }
    }
}
