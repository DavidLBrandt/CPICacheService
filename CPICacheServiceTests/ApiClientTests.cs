using CPICacheService.Models;
using CPICacheService.Utilities;

namespace CPICacheServiceTests
{
    [TestClass]
    public class ApiClientTests
    {
        [TestMethod]
        public void CallApi_RequestSucceeded_ReturnsApiResponse()
        {
            IApiClient apiClient = new MockApiClient(); // Test using mock data without hitting the BLS Public Data API
            //IApiClient apiClient = new ApiClient(); // Test using real data from the BLS Public Data API

            string seriesIds = "LAUCN040010000000005";
            string startYear = "2014";
            string endYear = "2023";
            string json = apiClient.GetCpiJson(
                seriesIds: seriesIds,
                startYear: startYear,
                endYear: endYear,
                validator: new PropertyValidator()
                ).GetAwaiter().GetResult();

            List<ApiResponse> apiResponses =  new ApiResponseConverter().ConvertFromJson(json);

            Assert.IsTrue(json.Contains("REQUEST_SUCCEEDED")); // Check that the BLS Public Data API returned success
            Assert.IsTrue(apiResponses.Any(r => r.Cpis.Count > 0)); // Check that any CPI data was returned
        }
    }
}
