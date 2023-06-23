using CPICacheService.Models;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace CPICacheService.Utilities
{
    public class ApiResponseConverter : IApiResponseConverter
    {
        public List<ApiResponse> ConvertFromJson(string json)
        {
            List<ApiResponse> apiResponses = new List<ApiResponse>();

            JObject jsonObject = JObject.Parse(json);

            string status = (string?)jsonObject["status"] ?? "";
            if (status == "REQUEST_SUCCEEDED")
            {
                JToken? seriesToken = jsonObject["Results"]?["series"];
                if (seriesToken != null && seriesToken.Type == JTokenType.Array)
                {
                    foreach (JToken series in seriesToken)
                    {
                        ApiResponse apiResponse = new ApiResponse
                        {
                            status = status,
                        };

                        JToken? dataToken = series["data"];
                        if (dataToken != null && dataToken.Type == JTokenType.Array)
                        {

                            foreach (JToken data in dataToken)
                            {
                                Cpi cpi = new Cpi
                                {
                                    SeriesId = (string?)series["seriesID"] ?? string.Empty,
                                    month = (string?)data["periodName"] ?? string.Empty,
                                    year = (string?)data["year"] ?? string.Empty,
                                    value = (int?)data["value"] ?? 0
                                };

                                JToken? notesToken = data["footnotes"];
                                if (notesToken != null && notesToken.Type == JTokenType.Array)
                                {
                                    foreach (JToken note in notesToken)
                                    {
                                        string? text = (string?)note["text"];
                                        if (!string.IsNullOrEmpty(text))
                                        {
                                            cpi.Notes.Add(text);
                                        }
                                    }
                                }

                                apiResponse.Cpis.Add(cpi);
                            }
                        }

                        apiResponses.Add(apiResponse);
                    }
                }

            }

            return apiResponses;
        }
    }
}
