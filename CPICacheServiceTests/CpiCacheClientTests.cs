using CPICacheService.Models;
using CPICacheService.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CPICacheServiceTests
{

    [TestClass]
    public class CpiCacheClientTests
    {
        private readonly MemoryCache _memoryCache;
        private readonly PropertyValidator _validator;

        public CpiCacheClientTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _validator = new PropertyValidator();
        }

        [TestMethod]
        public void GetCpiFromCache_CachedCpiExists_ReturnsCpiObject()
        {
            // Arrange
            var seriesId = "LAUCN040010000000005";
            var year = "2022";
            var month = "June";

            var cachedCpi = new Cpi { SeriesId = seriesId, year = year, month = month };
            var cacheKey = $"CPI_{seriesId}_{year}_{month}";

            var cpiCacheClient = new CacheClient(_memoryCache, _validator);
            var cpi = new Cpi { SeriesId = seriesId, year = year, month = month };

            cpiCacheClient.CacheCpi(cpi);

            // Act
            var result = cpiCacheClient.GetCpi(seriesId, year, month);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(seriesId, result.SeriesId);
            Assert.AreEqual(year, result.year);
            Assert.AreEqual(month, result.month);
        }

        [TestMethod]
        public void GetCpiFromCache_CachedCpiDoesNotExist_ReturnsNull()
        {
            // Arrange
            var seriesId = "LAUCN040010000000005";
            var year = "2022";
            var month = "June";

            var cacheKey = $"CPI_{seriesId}_{year}_{month}";

            var cpiCacheClient = new CacheClient(_memoryCache, _validator);

            // Act
            var result = cpiCacheClient.GetCpi(seriesId, year, month);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CacheCpi_StoresCpiInMemoryCache()
        {
            // Arrange
            var seriesId = "LAUCN040010000000005";
            var year = "2022";
            var month = "June";

            var cpi = new Cpi { SeriesId = seriesId, year = year, month = month };
            var cacheKey = $"CPI_{seriesId}_{year}_{month}";

            var cpiCacheClient = new CacheClient(_memoryCache, _validator);

            // Act
            cpiCacheClient.CacheCpi(cpi);
            var result = cpiCacheClient.GetCpi(seriesId, year, month);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetCpiCacheKey_ValidInputs_ReturnsCorrectCacheKey()
        {
            // Arrange
            var seriesId = "LAUCN040010000000005";
            var year = "2022";
            var month = "June";
            var expectedCacheKey = $"CPI_{seriesId}_{year}_{month}";

            var cpiCacheClient = new CacheClient(_memoryCache, _validator);

            // Act
            var result = cpiCacheClient.GetCacheKey(seriesId, year, month);

            // Assert
            Assert.AreEqual(expectedCacheKey, result);
        }

        [TestMethod]
        public void GetCpiCacheKey_InvalidSeriesId_ThrowsArgumentException()
        {
            // Arrange
            var seriesId = "invalid!";
            var year = "2022";
            var month = "June";

            var cpiCacheClient = new CacheClient(_memoryCache, _validator);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => cpiCacheClient.GetCacheKey(seriesId, year, month));
        }

        [TestMethod]
        public void GetCpiCacheKey_InvalidYear_ThrowsArgumentException()
        {
            // Arrange
            var seriesId = "LAUCN040010000000005";
            var year = "invalid";
            var month = "June";

            var cpiCacheClient = new CacheClient(_memoryCache, _validator);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => cpiCacheClient.GetCacheKey(seriesId, year, month));
        }

        [TestMethod]
        public void GetCpiCacheKey_InvalidMonth_ThrowsArgumentException()
        {
            // Arrange
            var seriesId = "LAUCN040010000000005";
            var year = "2022";
            var month = "invalid";

            var cpiCacheClient = new CacheClient(_memoryCache, _validator);

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => cpiCacheClient.GetCacheKey(seriesId, year, month));
        }
    }
}
