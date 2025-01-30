using ItemsStoreWebAPI.Controllers;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;
using ItemsStoreWebAPITests.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVControllerTests
    {
        private Mock<ITVService> _mockTVService;
        private Mock<ITVRequestValidator> _mockTVRequestValidator;
        private Mock<ILogger<TVController>> _mockLogger;
        private TVController _tvController;
        private TV _defaultTV;

        [Fact]
        public void AddInvalidTV_ShouldReturnBadRequest()
        {
            // Arrange
            var expectedTV = new TV();
            var errorMessage = string.Empty;
            
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _mockLogger = new Mock<ILogger<TVController>>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object, _mockLogger.Object);

            _mockTVRequestValidator.Setup(v => v.IsValid(expectedTV, out errorMessage)).Returns(false);

            // Act
            var result = _tvController.AddTV(expectedTV);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public void AddTV_ShouldReturnCreated()
        {
            // Arrange
            _defaultTV = TVFactory.CreateDefaultTV();
            var errorMessage = string.Empty;
            
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _mockLogger = new Mock<ILogger<TVController>>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object, _mockLogger.Object);

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
            _defaultTV = TVFactory.CreateDefaultTV();
            
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _mockLogger = new Mock<ILogger<TVController>>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object, _mockLogger.Object);

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
            _defaultTV = TVFactory.CreateDefaultTV();
            var tvs = new List<TV>
            {
                _defaultTV,
                TVFactory.CreateTV(2, "Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5)
            };
            
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _mockLogger = new Mock<ILogger<TVController>>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object, _mockLogger.Object);

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
            var expectedTV = new TV();
            expectedTV.ID = 1;
            var errorMessage = string.Empty;
            
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _mockLogger = new Mock<ILogger<TVController>>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object, _mockLogger.Object);

            _mockTVRequestValidator.Setup(v => v.IsValid(expectedTV, out errorMessage)).Returns(false);

            // Act
            var result = _tvController.UpdateTV(expectedTV);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void UpdateTV_ShouldReturnOk()
        {
            // Arrange
            var updatedTV = TVFactory.CreateTV(1, "LG", "Bravia", 65, "7680x4320", 120, 2019, 32000, 2);
            var errorMessage = string.Empty;
            
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _mockLogger = new Mock<ILogger<TVController>>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object, _mockLogger.Object);

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
            _defaultTV = TVFactory.CreateDefaultTV();
            
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _mockLogger = new Mock<ILogger<TVController>>();
            _tvController = new TVController(_mockTVService.Object, _mockTVRequestValidator.Object, _mockLogger.Object);

            _mockTVService.Setup(service => service.DeleteTV(_defaultTV.ID));

            // Act
            var result = _tvController.DeleteTV(_defaultTV.ID);

            // Assert
            _mockTVService.Verify(service => service.DeleteTV(1), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
    }
}