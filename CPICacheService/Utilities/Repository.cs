using CPICacheService.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace CPICacheService.Utilities
{
    public class Repository
    {
        private readonly IApiClient _apiClient;
        private readonly ICacheClient _cacheClient;
        private readonly IPropertyValidator _validator;
        private readonly IApiResponseConverter _responseConverter;

        public Repository(
            IApiClient apiClient, 
            ICacheClient cacheClient, 
            IPropertyValidator validator,
            IApiResponseConverter responseConverter) {

            _apiClient = apiClient;
            _cacheClient = cacheClient;
            _validator = validator;
            _responseConverter = responseConverter;
        }

        public async Task<Cpi> GetCpi(string seriesId, string year, string month)
        {
            if (!_validator.IsValidSeriesIdFormat(seriesId))
                throw new ArgumentException("Invalid seriesIds.");

            if (!_validator.IsValidYear(year))
                throw new ArgumentException("Invlaid startYear.");

            if (!_validator.IsValidYear(month))
                throw new ArgumentException("Invlaid endYear.");
            
            Cpi cpi;

            // Try to get Cpi from cache, return if found
            cpi = _cacheClient.GetCpi(seriesId, year, month);
            if (cpi != null)
            {
                return cpi;
            }

            // Get Cpi from Api and store in cache
            await ImportCpiToCacheFromApi(seriesId, year);

            return cpi;
        }

        private async Task ImportCpiToCacheFromApi(string seriesId, string year)
        {
            string startYear = GetStartOfDecade(year);
            string endYear = GetEndOfDecade(year);

            // This pulls the entire decade for the series anytime we need to get a Cpi.
            // A decade is the maximum we can pull.  This reduces the number of calls to the Api.
            string json = await _apiClient.GetCpiJson(seriesId, startYear, endYear, _validator);

            List<ApiResponse> apiResponses = _responseConverter.ConvertFromJson(json);
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
