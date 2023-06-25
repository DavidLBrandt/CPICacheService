using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPICacheService.Utilities;

namespace CPICacheServiceTests
{
    [TestClass]
    public class PropertyValidatorTests
    {
        private PropertyValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new PropertyValidator();
        }

        [TestMethod]
        public void IsValidSeriesIdFormat_ValidSeriesId_ReturnsTrue()
        {
            // Arrange
            string seriesId = "LAUCN040010000000005";

            // Act
            bool isValid = _validator.IsValidSeriesIdFormat(seriesId);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsValidSeriesIdFormat_InvalidSeriesId_ReturnsFalse()
        {
            // Arrange
            string seriesId = "abcd_1234-5678#";

            // Act
            bool isValid = _validator.IsValidSeriesIdFormat(seriesId);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsValidYear_ValidYear_ReturnsTrue()
        {
            // Arrange
            string year = "2022";

            // Act
            bool isValid = _validator.IsValidYear(year);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsValidYear_InvalidYear_ReturnsFalse()
        {
            // Arrange
            string year = "abcd";

            // Act
            bool isValid = _validator.IsValidYear(year);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsValidMonth_ValidMonth_ReturnsTrue()
        {
            // Arrange
            string month = "January";

            // Act
            bool isValid = _validator.IsValidMonth(month);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsValidMonth_InvalidMonth_ReturnsFalse()
        {
            // Arrange
            string month = "InvalidMonth";

            // Act
            bool isValid = _validator.IsValidMonth(month);

            // Assert
            Assert.IsFalse(isValid);
        }
    }
}
