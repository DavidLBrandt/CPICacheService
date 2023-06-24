namespace CPICacheService.Utilities
{
    public interface IApiClient
    {
        Task<string> GetCpiJson(string seriesIds, string startYear, string endYear, IPropertyValidator validator);
    }
}