using System;
using System.Text;

namespace CPICacheService.Utilities
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient httpClient;

        public ApiClient()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> GetCpiJson(string seriesId, string startYear, string endYear, IPropertyValidator validator)
        {
            if (!validator.IsValidSeriesIdFormat(seriesId))
                throw new ArgumentException("Invalid seriesId.");

            if (!validator.IsValidYear(startYear))
                throw new ArgumentException("Invlaid startYear.");

            if (!validator.IsValidYear(endYear))
                throw new ArgumentException("Invlaid endYear.");

            string apiUrl = "https://api.bls.gov/publicAPI/v1/timeseries/data/";

            var payload = new
            {
                seriesid = new string[] { seriesId },
                startyear = startYear,
                endyear = endYear
            };

            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return string.Empty;
            }
        }
    }
}