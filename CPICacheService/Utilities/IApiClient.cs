namespace CPICacheService.Utilities
{
    public interface IApiClient
    {
        Task<string> CallApi(string seriesIds, string startYear, string endYear, IPropertyValidator validator);
    }
}