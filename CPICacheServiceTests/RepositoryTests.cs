using CPICacheService.Models;
using CPICacheService.Utilities;
using Moq;

namespace CPICacheServiceTests
{
    [TestClass]
    public class RepositoryTests
    {
        private Repository _repository;
        private Mock<IApiClient> _apiClientMock;
        private Mock<ICacheClient> _cacheClientMock;
        private Mock<IPropertyValidator> _validatorMock;
        private Mock<IApiResponseConverter> _responseConverterMock;

        [TestInitialize]
        public void Initialize()
        {
            _apiClientMock = new Mock<IApiClient>();
            _cacheClientMock = new Mock<ICacheClient>();
            _validatorMock = new Mock<IPropertyValidator>();
            _responseConverterMock = new Mock<IApiResponseConverter>();

            _repository = new Repository(
                _apiClientMock.Object,
                _cacheClientMock.Object,
                _validatorMock.Object,
                _responseConverterMock.Object
            );
        }

        [TestMethod]
        public async Task GetCpi_ValidArguments_CpiFromCacheReturned()
        {
            // Arrange
            string seriesId = "LAUCN040010000000005";
            string year = "2021";
            string month = "January";
            var expectedCpi = new Cpi();

            _validatorMock.Setup(v => v.IsValidSeriesIdFormat(seriesId)).Returns(true);
            _validatorMock.Setup(v => v.IsValidYear(year)).Returns(true);
            _validatorMock.Setup(v => v.IsValidMonth(month)).Returns(true);
            _cacheClientMock.Setup(c => c.GetCpi(seriesId, year, month)).Returns(expectedCpi);

            // Act
            var result = await _repository.GetCpi(seriesId, year, month);

            // Assert
            Assert.AreEqual(expectedCpi, result);
            _cacheClientMock.Verify(c => c.GetCpi(seriesId, year, month), Times.Once);
            _apiClientMock.Verify(c => c.GetCpiJson(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IPropertyValidator>()), Times.Never);
            _cacheClientMock.Verify(c => c.CacheCpi(It.IsAny<Cpi>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetCpi_InvalidSeriesId_ThrowsArgumentException()
        {
            // Arrange
            string seriesId = "LAUCN040010000000005";
            string year = "2021";
            string month = "January";

            _validatorMock.Setup(v => v.IsValidSeriesIdFormat(seriesId)).Returns(false);

            // Act
            await _repository.GetCpi(seriesId, year, month);

            // Assert - Expects ArgumentException to be thrown
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetCpi_InvalidYear_ThrowsArgumentException()
        {
            // Arrange
            string seriesId = "LAUCN040010000000005";
            string year = "2021";
            string month = "January";

            _validatorMock.Setup(v => v.IsValidSeriesIdFormat(seriesId)).Returns(true);
            _validatorMock.Setup(v => v.IsValidYear(year)).Returns(false);

            // Act
            await _repository.GetCpi(seriesId, year, month);

            // Assert - Expects ArgumentException to be thrown
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetCpi_InvalidMonth_ThrowsArgumentException()
        {
            // Arrange
            string seriesId = "LAUCN040010000000005";
            string year = "2021";
            string month = "January";

            _validatorMock.Setup(v => v.IsValidSeriesIdFormat(seriesId)).Returns(true);
            _validatorMock.Setup(v => v.IsValidYear(year)).Returns(true);
            _validatorMock.Setup(v => v.IsValidMonth(month)).Returns(false);

            // Act
            await _repository.GetCpi(seriesId, year, month);

            // Assert - Expects ArgumentException to be thrown
        }

        [TestMethod]
        public async Task GetCpi_CpiNotInCache_CpiImportedAndCached()
        {
            // Arrange
            string seriesId = "LAUCN040010000000005";
            string year = "2021";
            string month = "January";
            Cpi expectedCpi = null;

            _validatorMock.Setup(v => v.IsValidSeriesIdFormat(seriesId)).Returns(true);
            _validatorMock.Setup(v => v.IsValidYear(year)).Returns(true);
            _validatorMock.Setup(v => v.IsValidMonth(month)).Returns(true);
            _cacheClientMock.Setup(c => c.GetCpi(seriesId, year, month)).Returns((Cpi)null);
            _apiClientMock.Setup(c => c.GetCpiJson(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IPropertyValidator>())).ReturnsAsync("json");
            _responseConverterMock.Setup(c => c.ConvertFromJson("json")).Returns(new List<ApiResponse> { new ApiResponse { Cpis = new List<Cpi> { expectedCpi } } });

            // Act
            var result = await _repository.GetCpi(seriesId, year, month);

            // Assert
            Assert.AreEqual(expectedCpi, result);
            _cacheClientMock.Verify(c => c.GetCpi(seriesId, year, month), Times.Exactly(2));
            _apiClientMock.Verify(c => c.GetCpiJson(seriesId, "2020", "2029", It.IsAny<IPropertyValidator>()), Times.Once);
        }
    }
}
