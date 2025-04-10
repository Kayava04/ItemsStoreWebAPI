using ItemsStoreWebAPI.Validators;
using ItemsStoreWebAPITests.Factories;

namespace ItemsStoreWebAPITests.Tests
{
    public class TVValidatorTests
    {
        [Fact]
        public void IsValidTV_ShouldReturnFalse_WhenTVIsNull()
        {
            // Arrange
            var tvRequestValidator = new TVRequestValidator();

            // Act
            var result = tvRequestValidator.IsValid(null, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.TVObjectCannotBeNull, errorMessage);
        }

        [Fact]
        public void IsValidName_ShouldReturnFalse()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            expectedTV.Name = string.Empty;
            
            var tvRequestValidator = new TVRequestValidator();

            // Act
            var result = tvRequestValidator.IsValid(expectedTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.NameIsRequired, errorMessage);
        }

        [Fact]
        public void IsValidSize_ShouldReturnFalse()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            expectedTV.Size = 0;
            
            var tvRequestValidator = new TVRequestValidator();

            // Act
            var result = tvRequestValidator.IsValid(expectedTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.SizeCannotBeZeroOrNegative, errorMessage);
        }

        [Fact]
        public void IsValidResolution_ShouldReturnFalse()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            expectedTV.Resolution = string.Empty;
            
            var tvRequestValidator = new TVRequestValidator();

            // Act
            var result = tvRequestValidator.IsValid(expectedTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.ResolutionIsRequired, errorMessage);
        }

        [Fact]
        public void IsValidFrequency_ShouldReturnFalse()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            expectedTV.Frequency = 0;
            
            var tvRequestValidator = new TVRequestValidator();
            
            // Act
            var result = tvRequestValidator.IsValid(expectedTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.FrequencyCannotBeZeroOrNegative, errorMessage);
        }

        [Fact]
        public void IsValidReleasedYear_ShouldReturnFalse()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            expectedTV.ReleasedYear = 1899;
            
            var tvRequestValidator = new TVRequestValidator();

            // Act
            var result = tvRequestValidator.IsValid(expectedTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.ReleasedYearMustBeCorrect, errorMessage);
        }

        [Fact]
        public void IsValidPrice_ShouldReturnFalse()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            expectedTV.Price = -19299;
            
            var tvRequestValidator = new TVRequestValidator();

            // Act
            var result = tvRequestValidator.IsValid(expectedTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.PriceCannotBeNegative, errorMessage);
        }

        [Fact]
        public void IsValidInStock_ShouldReturnFalse()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            expectedTV.InStock = -5;
            
            var tvRequestValidator = new TVRequestValidator();

            // Act
            var result = tvRequestValidator.IsValid(expectedTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.InStockCannotBeNegative, errorMessage);
        }
    }
}