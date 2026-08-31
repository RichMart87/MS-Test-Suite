using System.Text.Json;
using SeleniumMStestProject.Base;
using SeleniumMStestProject.Constants;
using SeleniumMStestProject.Tests.Api.Models;

namespace SeleniumMStestProject.Tests.Api
{
    /* API under test: https://automationexercise.com/api_list
     * Note: this API always responds with HTTP 200 at the transport level.
     * The real status is embedded in the JSON body's "responseCode" field.
     */
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

        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
        public async Task GetProductsList_ReturnsAllProducts()
        {
            var json = JsonDocument.Parse(await GetJsonBodyAsync(() => Client.GetAsync(ProductsListEndpoint)));

            Assert.AreEqual(200, GetResponseCode(json));
            Assert.IsGreaterThan(0, json.RootElement.GetProperty("products").GetArrayLength());
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
        public async Task PostToProductsList_ReturnsMethodNotAllowed()
        {
            var json = JsonDocument.Parse(await GetJsonBodyAsync(() => Client.PostAsync(ProductsListEndpoint, null)));

            Assert.AreEqual(405, GetResponseCode(json));
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
        public async Task GetBrandsList_ReturnsAllBrands()
        {
            var json = JsonDocument.Parse(await GetJsonBodyAsync(() => Client.GetAsync(BrandsListEndpoint)));

            Assert.AreEqual(200, GetResponseCode(json));
            Assert.IsGreaterThan(0, json.RootElement.GetProperty("brands").GetArrayLength());
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
        public async Task GetBrandsList_DeserializesToTypedBrand()
        {
            var body = await GetJsonBodyAsync(() => Client.GetAsync(BrandsListEndpoint));
            var brandsResponse = JsonSerializer.Deserialize<BrandsListResponse>(body, JsonOptions);

            Assert.IsNotNull(brandsResponse);
            Assert.AreEqual(200, brandsResponse.ResponseCode);

            var polo = brandsResponse.Brands.FirstOrDefault(b => b.Id == 1);
            Assert.IsNotNull(polo, "Expected seeded brand with id 1 to exist.");
            Assert.AreEqual("Polo", polo.Name);
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
        public async Task PutToBrandsList_ReturnsMethodNotAllowed()
        {
            var json = JsonDocument.Parse(await GetJsonBodyAsync(() => Client.PutAsync(BrandsListEndpoint, null)));

            Assert.AreEqual(405, GetResponseCode(json));
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
        public async Task SearchProduct_WithValidTerm_ReturnsMatchingProducts()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["search_product"] = "top"
            });

            var json = JsonDocument.Parse(await GetJsonBodyAsync(() => Client.PostAsync(SearchProductEndpoint, content)));

            Assert.AreEqual(200, GetResponseCode(json));
            Assert.IsGreaterThan(0, json.RootElement.GetProperty("products").GetArrayLength());
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
        public async Task SearchProduct_WithoutSearchTerm_ReturnsBadRequest()
        {
            var json = JsonDocument.Parse(await GetJsonBodyAsync(() => Client.PostAsync(SearchProductEndpoint, null)));

            Assert.AreEqual(400, GetResponseCode(json));
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
        public async Task VerifyLogin_WithInvalidCredentials_ReturnsUserNotFound()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["email"] = "no_such_user_qa_regression@example.com",
                ["password"] = "not-the-right-password"
            });

            var json = JsonDocument.Parse(await GetJsonBodyAsync(() => Client.PostAsync(VerifyLoginEndpoint, content)));

            Assert.AreEqual(404, GetResponseCode(json));
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
        public async Task VerifyLogin_WithoutEmail_ReturnsBadRequest()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["password"] = "irrelevant"
            });

            var json = JsonDocument.Parse(await GetJsonBodyAsync(() => Client.PostAsync(VerifyLoginEndpoint, content)));

            Assert.AreEqual(400, GetResponseCode(json));
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
        public async Task VerifyLogin_UsingDeleteMethod_ReturnsMethodNotAllowed()
        {
            var json = JsonDocument.Parse(await GetJsonBodyAsync(() => Client.DeleteAsync(VerifyLoginEndpoint)));

            Assert.AreEqual(405, GetResponseCode(json));
        }

        [TestMethod]
        [Timeout(TestExecutionTimeouts.ApiMilliseconds)]
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

            var createJson = JsonDocument.Parse(await GetJsonBodyAsync(
                () => Client.PostAsync(CreateAccountEndpoint, new FormUrlEncodedContent(accountFields))));
            Assert.AreEqual(201, GetResponseCode(createJson), "Account creation should succeed.");

            try
            {
                var verifyContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["email"] = email,
                    ["password"] = password
                });
                var verifyJson = JsonDocument.Parse(await GetJsonBodyAsync(() => Client.PostAsync(VerifyLoginEndpoint, verifyContent)));
                Assert.AreEqual(200, GetResponseCode(verifyJson), "Newly created account should be able to log in.");

                var userDetail = await GetUserDetailAsync(email);
                Assert.AreEqual(200, userDetail.ResponseCode);
                Assert.AreEqual(email, userDetail.User.Email);
                Assert.AreEqual("Testville", userDetail.User.City);

                accountFields["city"] = "Updated City";
                var updateJson = JsonDocument.Parse(await GetJsonBodyAsync(
                    () => Client.PutAsync(UpdateAccountEndpoint, new FormUrlEncodedContent(accountFields))));
                Assert.AreEqual(200, GetResponseCode(updateJson), "Account update should succeed.");

                var userDetailAfterUpdate = await GetUserDetailAsync(email);
                Assert.AreEqual("Updated City", userDetailAfterUpdate.User.City);
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

        private async Task<UserDetailResponse> GetUserDetailAsync(string email)
        {
            var body = await GetJsonBodyAsync(() => Client.GetAsync($"{GetUserDetailByEmailEndpoint}?email={Uri.EscapeDataString(email)}"));
            var userDetail = JsonSerializer.Deserialize<UserDetailResponse>(body, JsonOptions);

            Assert.IsNotNull(userDetail);
            return userDetail;
        }

        /* automationexercise.com is fronted by Cloudflare, which occasionally
         * serves an HTML challenge/block page instead of the real API
         * response to requests from shared CI IP ranges. That HTML starts
         * with '<', which fails JSON parsing with a cryptic error. Retry a
         * couple of times with backoff, and if it's still not JSON, fail
         * with a message that says what actually happened.*/
        private static async Task<string> GetJsonBodyAsync(Func<Task<HttpResponseMessage>> sendRequest, int maxAttempts = 3)
        {
            var body = string.Empty;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                using var response = await sendRequest();
                body = await response.Content.ReadAsStringAsync();

                if (LooksLikeJson(body))
                {
                    return body;
                }

                if (attempt < maxAttempts)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2 * attempt));
                }
            }

            var preview = body.Length > 200 ? body[..200] : body;
            Assert.Fail(
                $"Expected a JSON response from {Config.ApiBaseUrl} but received non-JSON content after " +
                $"{maxAttempts} attempts - likely a Cloudflare challenge/block page for this network. " +
                $"Response preview: {preview}");
            return body; // unreachable, Assert.Fail throws
        }

        private static bool LooksLikeJson(string body)
        {
            var trimmed = body.TrimStart();
            return trimmed.StartsWith('{') || trimmed.StartsWith('[');
        }

        private static int GetResponseCode(JsonDocument json)
        {
            return json.RootElement.GetProperty("responseCode").GetInt32();
        }
    }
}
