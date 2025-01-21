using ItemsStoreWebAPI.Validators;
using ItemsStoreWebAPITests.Common;
using ItemsStoreWebAPITests.Factories;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVValidatorTests
    {
        private readonly ITVRequestValidator _tvRequestValidator;
        private readonly ITVFactory _tvFactory;

        public TVValidatorTests()
        {
            _tvRequestValidator = new TVRequestValidator();
            _tvFactory = new TVFactory();
        }

        [Fact]
        public void IsValidTV_ShouldReturnTrue()
        {
            // Arrange
            var tv = _tvFactory.CreateTV();

            string errorMessage = string.Empty;

            // Act
            var result = _tvRequestValidator.IsValid(tv, out errorMessage);

            // Assert
            Assert.True(result);
            Assert.Equal(string.Empty, errorMessage);
        }

        [Fact]
        public void IsValidName_ShouldReturnFalse()
        {
            // Arrange
            var tv = _tvFactory.CreateTV();
            tv.Name = string.Empty;

            // Act
            var result = _tvRequestValidator.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.NameIsRequired, errorMessage);
        }

        [Fact]
        public void IsValidSize_ShouldReturnFalse()
        {
            // Arrange
            var tv = _tvFactory.CreateTV();
            tv.Size = 0;

            // Act
            var result = _tvRequestValidator.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.SizeCannotBeZeroOrNegative, errorMessage);
        }

        [Fact]
        public void IsValidResolution_ShouldReturnFalse()
        {
            // Arrange
            var tv = _tvFactory.CreateTV();
            tv.Resolution = string.Empty;

            // Act
            var result = _tvRequestValidator.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.ResolutionIsRequired, errorMessage);
        }

        [Fact]
        public void IsValidFrequency_ShouldReturnFalse()
        {
            // Arrange
            var tv = _tvFactory.CreateTV();
            tv.Frequency = 0;

            // Act
            var result = _tvRequestValidator.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.FrequencyCannotBeZeroOrNegative, errorMessage);
        }

        [Fact]
        public void IsValidReleasedYear_ShouldReturnFalse()
        {
            // Arrange
            var tv = _tvFactory.CreateTV();
            tv.ReleasedYear = 1899;

            // Act
            var result = _tvRequestValidator.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.ReleasedYearMustBeCorrect, errorMessage);
        }

        [Fact]
        public void IsValidPrice_ShouldReturnFalse()
        {
            // Arrange
            var tv = _tvFactory.CreateTV();
            tv.Price = -19500;

            // Act
            var result = _tvRequestValidator.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.PriceCannotBeNegative, errorMessage);
        }

        [Fact]
        public void IsValidInStock_ShouldReturnFalse()
        {
            // Arrange
            var tv = _tvFactory.CreateTV();
            tv.InStock = -2;

            // Act
            var result = _tvRequestValidator.IsValid(tv, out string errorMessage);

            // Assert
            Assert.False(result);
            Assert.Equal(ValidationMessages.InStockCannotBeNegative, errorMessage);
        }
    }
}