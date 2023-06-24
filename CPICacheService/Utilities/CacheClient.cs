using CPICacheService.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CPICacheService.Utilities
{
    public class CacheClient : ICacheClient
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IPropertyValidator _validator;

        public CacheClient(IMemoryCache memoryCache, IPropertyValidator validator)
        {
            _memoryCache = memoryCache;
            _validator = validator;
        }

        public Cpi GetCpi(string seriesId, string year, string month)
        {
            string cacheKey = GetCacheKey(seriesId, year, month);
            if (_memoryCache.TryGetValue(cacheKey, out Cpi cachedCpi))
            {
                return cachedCpi;
            }

            return null;
        }

        public void CacheCpi(Cpi cpi)
        {
            string cacheKey = GetCacheKey(cpi.SeriesId, cpi.year, cpi.month);
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(1)); // Cache entry expires after 1 day

            _memoryCache.Set(cacheKey, cpi, cacheOptions);
        }

        public string GetCacheKey(string seriesId, string year, string month)
        {
            if (!_validator.IsValidSeriesIdFormat(seriesId))
                throw new ArgumentException("Invalid seriesIds.");

            if (!_validator.IsValidYear(year))
                throw new ArgumentException("Invlaid year.");

            if (!_validator.IsValidMonth(month))
                throw new ArgumentException("Invlaid month.");

            return $"CPI_{seriesId}_{year}_{month}";
        }
    }
}
