using CPICacheService.Controllers;
using CPICacheService.Models;
using CPICacheService.Utilities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CPICacheServiceTests
{
    [TestClass]
    public class CpiDataControllerTests
    {
        private Mock<IRepository> _mockRepository;
        private CpiDataController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IRepository>();
            _controller = new CpiDataController(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_ValidInputs_ReturnsOkResult()
        {
            // Arrange
            string seriesId = "LAUCN040010000000005";
            string year = "2022";
            string month = "January";
            Cpi expectedCpi = new Cpi();

            _mockRepository.Setup(r => r.GetCpi(seriesId, year, month))
                .ReturnsAsync(expectedCpi);

            // Act
            ActionResult<Cpi> result = await _controller.GetAsync(seriesId, year, month);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(expectedCpi, ((OkObjectResult)result.Result).Value);
        }

        [TestMethod]
        public async Task GetAsync_InvalidSeriesId_ReturnsBadRequest()
        {
            // Arrange
            string seriesId = "InvalidSeriesId";
            string year = "2022";
            string month = "January";

            // Act
            ActionResult<Cpi> result = await _controller.GetAsync(seriesId, year, month);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual("Invalid series id format.", ((BadRequestObjectResult)result.Result).Value);
        }

        [TestMethod]
        public async Task GetAsync_InvalidYear_ReturnsBadRequest()
        {
            // Arrange
            string seriesId = "LAUCN040010000000005";
            string year = "InvalidYear";
            string month = "January";

            // Act
            ActionResult<Cpi> result = await _controller.GetAsync(seriesId, year, month);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual("Invalid year format.", ((BadRequestObjectResult)result.Result).Value);
        }

        [TestMethod]
        public async Task GetAsync_InvalidMonth_ReturnsBadRequest()
        {
            // Arrange
            string seriesId = "LAUCN040010000000005";
            string year = "2022";
            string month = "InvalidMonth";

            // Act
            ActionResult<Cpi> result = await _controller.GetAsync(seriesId, year, month);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Assert.AreEqual("Invalid month format.", ((BadRequestObjectResult)result.Result).Value);
        }

        [TestMethod]
        public async Task GetAsync_DataNotFound_ReturnsNotFound()
        {
            // Arrange
            string seriesId = "LAUCN040010000000005";
            string year = "2022";
            string month = "January";

            _mockRepository.Setup(r => r.GetCpi(seriesId, year, month))
                .ReturnsAsync((Cpi)null);

            // Act
            ActionResult<Cpi> result = await _controller.GetAsync(seriesId, year, month);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            Assert.AreEqual("Data not found.", ((NotFoundObjectResult)result.Result).Value);
        }
    }
}
