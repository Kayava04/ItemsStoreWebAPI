using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Validators;
using Moq;


namespace ItemsStoreWebAPITests
{
    public class TVValidatorTests
    {
        private readonly Mock<ITVRequestValidator> _mockTVRequestValidator;

        public TVValidatorTests()
        {
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
        }

        [Fact]
        public void IsValidTV_ShouldReturnTrue()
        {
            // Arange
            var tv = new TV
            {
                ID = 1,
                Name = "LG",
                Description = "OLED TV",
                Size = 55,
                Resolution = "1920x1080",
                Frequency = 100.5f,
                ReleasedYear = 2020,
                Price = 25500,
                AddedAt = DateTime.UtcNow,
                InStock = 2
            };

            string message = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(tv, out message)).Returns(true);
            
            // Act
            var result = _mockTVRequestValidator.Object.IsValid(tv, out message);

            // Assert
            Assert.True(result);
            Assert.Equal(string.Empty, message);
        }

        [Fact]
        public void IsValidName_ShouldReturnFalse()
        {
            // Arrange
            var tv = new TV
            {
                ID = 1,
                Name = null,
                Description = "OLED TV",
                Size = 55,
                Resolution = "1920x1080",
                Frequency = 100.5f,
                ReleasedYear = 2020,
                Price = 25500,
                AddedAt = DateTime.UtcNow,
                InStock = 2
            };

            string message = "Name is required!";

            _mockTVRequestValidator.Setup(v => v.IsValid(tv, out message)).Returns(false);

            // Act
            var result = _mockTVRequestValidator.Object.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(message, errorMessage);
        }

        [Fact]
        public void IsValidSize_ShouldReturnFalse()
        {
            // Arrange
            var tv = new TV
            {
                ID = 1,
                Name = "LG",
                Description = "OLED TV",
                Size = 0,
                Resolution = "1920x1080",
                Frequency = 100.5f,
                ReleasedYear = 2020,
                Price = 25500,
                AddedAt = DateTime.UtcNow,
                InStock = 2
            };

            string message = "Size can not be zero or negative!";

            _mockTVRequestValidator.Setup(v => v.IsValid(tv, out message)).Returns(false);

            // Act
            var result = _mockTVRequestValidator.Object.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(message, errorMessage);
        }

        [Fact]
        public void IsValidResolution_ShouldReturnFalse()
        {
            // Arrange
            var tv = new TV
            {
                ID = 1,
                Name = "LG",
                Description = "OLED TV",
                Size = 50,
                Resolution = null,
                Frequency = 100.5f,
                ReleasedYear = 2020,
                Price = 25500,
                AddedAt = DateTime.UtcNow,
                InStock = 2
            };

            string message = "Resolution is required!";

            _mockTVRequestValidator.Setup(v => v.IsValid(tv, out message)).Returns(false);

            // Act
            var result = _mockTVRequestValidator.Object.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(message, errorMessage);
        }

        [Fact]
        public void IsValidFrequency_ShouldReturnFalse()
        {
            // Arrange
            var tv = new TV
            {
                ID = 1,
                Name = "LG",
                Description = "OLED TV",
                Size = 50,
                Resolution = "1920x1080",
                Frequency = 0,
                ReleasedYear = 2020,
                Price = 25500,
                AddedAt = DateTime.UtcNow,
                InStock = 2
            };

            string message = "Frequency can not be zero or negative!";

            _mockTVRequestValidator.Setup(v => v.IsValid(tv, out message)).Returns(false);

            // Act
            var result = _mockTVRequestValidator.Object.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(message, errorMessage);
        }

        [Fact]
        public void IsValidReleasedYear_ShouldReturnFalse()
        {
            // Arrange
            var tv = new TV
            {
                ID = 1,
                Name = "LG",
                Description = "OLED TV",
                Size = 55,
                Resolution = "1920x1080",
                Frequency = 100.5f,
                ReleasedYear = 1899,
                Price = 25500,
                AddedAt = DateTime.UtcNow,
                InStock = 2
            };

            string message = "ReleasedYear must be a correct!";

            _mockTVRequestValidator.Setup(v => v.IsValid(tv, out message)).Returns(false);

            // Act
            var result = _mockTVRequestValidator.Object.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(message, errorMessage);
        }

        [Fact]
        public void IsValidPrice_ShouldReturnFalse()
        {
            // Arrange
            var tv = new TV
            {
                ID = 1,
                Name = "LG",
                Description = "OLED TV",
                Size = 55,
                Resolution = "1920x1080",
                Frequency = 100.5f,
                ReleasedYear = 2020,
                Price = -13223,
                AddedAt = DateTime.UtcNow,
                InStock = 2
            };

            string message = "Price can not be negative!";

            _mockTVRequestValidator.Setup(v => v.IsValid(tv, out message)).Returns(false);

            // Act
            var result = _mockTVRequestValidator.Object.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(message, errorMessage);
        }

        [Fact]
        public void IsValidInStock_ShouldReturnFalse()
        {
            // Arrange
            var tv = new TV
            {
                ID = 1,
                Name = "LG",
                Description = "OLED TV",
                Size = 55,
                Resolution = "1920x1080",
                Frequency = 100.5f,
                ReleasedYear = 2020,
                Price = 25500,
                AddedAt = DateTime.UtcNow,
                InStock = -2
            };

            string message = "InStock can not be negative!";

            _mockTVRequestValidator.Setup(v => v.IsValid(tv, out message)).Returns(false);

            // Act
            var result = _mockTVRequestValidator.Object.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(message, errorMessage);
        }
    }
}