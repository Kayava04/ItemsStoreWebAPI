using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Validators;
using ItemsStoreWebAPITests.Factories;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVValidatorTests
    {
        private TVRequestValidator _tvRequestValidator;
        private TVFactory _tvFactory;
        private TV _defaultTV;

        public TVValidatorTests()
        {
            _tvFactory = new TVFactory();
        }

        [Fact]
        public void IsValidTV_ShouldReturnTrue()
        {
            // Arrange
            _tvRequestValidator = new TVRequestValidator();
            _defaultTV = _tvFactory.CreateDefaultTV();

            // Act
            var result = _tvRequestValidator.IsValid(_defaultTV, out string errorMessage);

            // Assert
            Assert.True(result);
            Assert.Equal(string.Empty, errorMessage);
        }

        [Fact]
        public void IsValidName_ShouldReturnFalse()
        {
            // Arrange
            _tvRequestValidator = new TVRequestValidator();
            _defaultTV = _tvFactory.CreateDefaultTV();

            _defaultTV.Name = string.Empty;

            // Act
            var result = _tvRequestValidator.IsValid(_defaultTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.NameIsRequired, errorMessage);
        }

        [Fact]
        public void IsValidSize_ShouldReturnFalse()
        {
            // Arrange
            _tvRequestValidator = new TVRequestValidator();
            _defaultTV = _tvFactory.CreateDefaultTV();

            _defaultTV.Size = 0;

            // Act
            var result = _tvRequestValidator.IsValid(_defaultTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.SizeCannotBeZeroOrNegative, errorMessage);
        }

        [Fact]
        public void IsValidResolution_ShouldReturnFalse()
        {
            // Arrange
            _tvRequestValidator = new TVRequestValidator();
            _defaultTV = _tvFactory.CreateDefaultTV();

            _defaultTV.Resolution = string.Empty;

            // Act
            var result = _tvRequestValidator.IsValid(_defaultTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.ResolutionIsRequired, errorMessage);
        }

        [Fact]
        public void IsValidFrequency_ShouldReturnFalse()
        {
            // Arrange
            _tvRequestValidator = new TVRequestValidator();
            _defaultTV = _tvFactory.CreateDefaultTV();

            _defaultTV.Frequency = 0;

            // Act
            var result = _tvRequestValidator.IsValid(_defaultTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.FrequencyCannotBeZeroOrNegative, errorMessage);
        }

        [Fact]
        public void IsValidReleasedYear_ShouldReturnFalse()
        {
            // Arrange
            _tvRequestValidator = new TVRequestValidator();
            _defaultTV = _tvFactory.CreateDefaultTV();

            _defaultTV.ReleasedYear = 1899;

            // Act
            var result = _tvRequestValidator.IsValid(_defaultTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.ReleasedYearMustBeCorrect, errorMessage);
        }

        [Fact]
        public void IsValidPrice_ShouldReturnFalse()
        {
            // Arrange
            _tvRequestValidator = new TVRequestValidator();
            _defaultTV = _tvFactory.CreateDefaultTV();

            _defaultTV.Price = -19299;

            // Act
            var result = _tvRequestValidator.IsValid(_defaultTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.PriceCannotBeNegative, errorMessage);
        }

        [Fact]
        public void IsValidInStock_ShouldReturnFalse()
        {
            // Arrange
            _tvRequestValidator = new TVRequestValidator();
            _defaultTV = _tvFactory.CreateDefaultTV();

            _defaultTV.InStock = -5;

            // Act
            var result = _tvRequestValidator.IsValid(_defaultTV, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.InStockCannotBeNegative, errorMessage);
        }
    }
}