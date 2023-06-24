using CPICacheService.Models;

namespace CPICacheService.Utilities
{
    public interface ICacheClient
    {
        void CacheCpi(Cpi cpi);
        string GetCacheKey(string seriesId, string year, string month);
        Cpi GetCpi(string seriesId, string year, string month);
    }
}