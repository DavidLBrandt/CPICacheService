using CPICacheService.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace CPICacheService.Utilities
{
    public static class ApiJsonConverter
    {
        public static List<ApiResponse> ConvertFromJson(string json)
        {
            List<ApiResponse> apiResponses = new List<ApiResponse>();
            //string json = MockApi.Response;
            JObject jsonObject = JObject.Parse(json);

            string status = (string?)jsonObject["status"] ?? "";

            if (status == "REQUEST_SUCCEEDED")
            {
                JToken? seriesToken = jsonObject["Results"]?["series"];
                if (seriesToken != null && seriesToken.Type == JTokenType.Array)
                {
                    foreach (JToken series in seriesToken)
                    {
                        ApiResponse apiResponse = new ApiResponse { 
                            status = status,
                        };

                        JToken? dataToken = series["data"];
                        if (dataToken != null && dataToken.Type == JTokenType.Array)
                        {

                            foreach (JToken data in dataToken)
                            {
                                Cpi cpi = new Cpi { 
                                    SeriesId = (string?)series["seriesID"] ?? string.Empty,
                                    month = (string?)data["periodName"] ?? string.Empty,
                                    year = (string?)data["year"] ?? string.Empty,
                                    value = (int?)data["year"] ?? 0
                                };

                                throw new Exception("Need to harvest any notes for the CPI.");

                                apiResponses.Add(apiResponse);
                            }
                        }
                    }
                }

            }

            return apiResponses;
        }
    }
}
