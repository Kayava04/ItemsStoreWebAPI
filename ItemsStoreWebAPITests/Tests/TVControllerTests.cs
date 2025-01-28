using ItemsStoreWebAPI.Controllers;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;
using ItemsStoreWebAPITests.Factories;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVControllerTests
    {
        private Mock<ITVService> _mockTVService;
        private Mock<ITVRequestValidator> _mockTVRequestValidator;
        private TVController _tvController;
        private TV _defaultTV;

        [Fact]
        public void AddInvalidTV_ShouldReturnBadRequest()
        {
            // Arrange
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object);

            var invalidTV = new TV();
            string errorMessage = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(invalidTV, out errorMessage)).Returns(false);

            // Act
            var result = _tvController.AddTV(invalidTV);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public void AddTV_ShouldReturnCreated()
        {
            // Arrange
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object);
            _defaultTV = TVFactory.CreateDefaultTV();

            string errorMessage = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(_defaultTV, out errorMessage)).Returns(true);

            // Act
            var result = _tvController.AddTV(_defaultTV);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(_defaultTV, createdResult.Value);
        }

        [Fact]
        public void GetTVById_ShouldReturnOk()
        {
            // Arrange
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object);
            _defaultTV = TVFactory.CreateDefaultTV();

            _mockTVService.Setup(service => service.GetTVById(_defaultTV.ID)).Returns(_defaultTV);

            // Act
            var result = _tvController.GetTVById(_defaultTV.ID);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_defaultTV, okResult.Value);
        }

        [Fact]
        public void GetAllTVs_ShouldReturnOk()
        {
            // Arrange
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object);
            _defaultTV = TVFactory.CreateDefaultTV();

            var tvs = new List<TV>
            {
                _defaultTV,
                TVFactory.CreateTV(2, "Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5)
            };

            _mockTVService.Setup(service => service.GetAllTVs()).Returns(tvs);

            // Act
            var result = _tvController.GetAllTVs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tvs, okResult.Value);
        }

        [Fact]
        public void UpdateInvalidTV_ShouldReturnBadRequest()
        {
            // Arrange
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object);

            var invalidTV = new TV();
            invalidTV.ID = 1;
            string errorMessage = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(invalidTV, out errorMessage)).Returns(false);

            // Act
            var result = _tvController.UpdateTV(invalidTV);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void UpdateTV_ShouldReturnOk()
        {
            // Arrange
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object);

            var updatedTV = TVFactory.CreateTV(1, "LG", "Bravia", 65, "7680x4320", 120, 2019, 32000, 2);
            string errorMessage = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(updatedTV, out errorMessage)).Returns(true);
            _mockTVService.Setup(service => service.UpdateTV(updatedTV.ID, updatedTV)).Returns(updatedTV);

            // Act
            var result = _tvController.UpdateTV(updatedTV);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedTV, okResult.Value);
        }

        [Fact]
        public void DeleteTV_ShouldReturnNoContent()
        {
            // Arrange
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object);
            _defaultTV = TVFactory.CreateDefaultTV();

            _mockTVService.Setup(service => service.DeleteTV(_defaultTV.ID));

            // Act
            var result = _tvController.DeleteTV(_defaultTV.ID);

            // Assert
            _mockTVService.Verify(service => service.DeleteTV(1), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
    }
}