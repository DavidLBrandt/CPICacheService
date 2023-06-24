using CPICacheService.Models;

namespace CPICacheService.Utilities
{
    public class Repository : IRepository
    {
        private readonly IApiClient _apiClient;
        private readonly ICacheClient _cacheClient;
        private readonly IPropertyValidator _validator;
        private readonly IApiResponseConverter _responseConverter;

        public Repository(
            IApiClient apiClient,
            ICacheClient cacheClient,
            IPropertyValidator validator,
            IApiResponseConverter responseConverter)
        {

            _apiClient = apiClient;
            _cacheClient = cacheClient;
            _validator = validator;
            _responseConverter = responseConverter;
        }

        public async Task<Cpi> GetCpi(string seriesId, string year, string month)
        {
            Cpi cpi;

            // Validate arguments
            ValidateArguments(seriesId, year, month);

            // Try to get Cpi from cache, return it if found
            cpi = _cacheClient.GetCpi(seriesId, year, month);
            if (cpi != null) return cpi;

            // Import Cpi from API and cache it
            await ImportCpiToCacheFromApi(seriesId, year);

            // Return Cpi from cache
            return _cacheClient.GetCpi(seriesId, year, month);
        }

        private void ValidateArguments(string seriesId, string year, string month)
        {
            if (!_validator.IsValidSeriesIdFormat(seriesId))
                throw new ArgumentException("Invalid seriesId.");

            if (!_validator.IsValidYear(year))
                throw new ArgumentException("Invlaid year.");

            if (!_validator.IsValidMonth(month))
                throw new ArgumentException("Invlaid month.");
        }

        private async Task ImportCpiToCacheFromApi(string seriesId, string year)
        {
            string startYear = GetStartOfDecade(year);
            string endYear = GetEndOfDecade(year);

            // This pulls the entire decade for the series anytime we need to get a Cpi.
            // A decade is the maximum we can pull.  This reduces the number of calls to the Api.
            string json = await _apiClient.GetCpiJson(seriesId, startYear, endYear, _validator);

            // Convert raw JSON to a list of ApiResponse objects
            List<ApiResponse> apiResponses = _responseConverter.ConvertFromJson(json);

            // Cache the Cpi data from ApiResponse objects
            CacheApiResponses(apiResponses);
        }

        private void CacheApiResponses(List<ApiResponse> apiResponses)
        {
            foreach (ApiResponse apiResponse in apiResponses)
            {
                if (apiResponse.RequestSucceeded)
                {
                    foreach (Cpi cpi in apiResponse.Cpis)
                    {
                        _cacheClient.CacheCpi(cpi);
                    }
                }
            }
        }

        private string GetStartOfDecade(string year)
        {
            int parsedYear = int.Parse(year);
            int startYear = (parsedYear / 10) * 10;
            return startYear.ToString();
        }

        private string GetEndOfDecade(string year)
        {
            int parsedYear = int.Parse(year);
            int startYear = (parsedYear / 10) * 10;
            int endYear = startYear + 9;
            return endYear.ToString();
        }
    }
}
