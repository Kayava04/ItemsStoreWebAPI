using ItemsStoreWebAPI.Controllers;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;
using ItemsStoreWebAPITests.Factories;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVStorageControllerTests
    {
        private readonly Mock<ITVService> _mockTVService;
        private readonly Mock<ITVRequestValidator> _mockTVRequestValidator;
        private readonly TVStorageController _tvStorageController;
        private readonly ITVFactory _tvFactory;
        private readonly TV _defaultTV;

        public TVStorageControllerTests()
        {
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _tvStorageController = new TVStorageController(_mockTVService.Object, _mockTVRequestValidator.Object);
            _tvFactory = new TVFactory();
            _defaultTV = _tvFactory.CreateDefaultTV();
        }

        [Fact]
        public void AddInvalidTV_ShouldReturnBadRequest()
        {
            // Arrange
            var invalidTV = new TV();
            string errorMessage = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(invalidTV, out errorMessage)).Returns(false);

            // Act
            var result = _tvStorageController.AddTV(invalidTV);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public void AddTV_ShouldReturnCreated()
        {
            // Arrange
            string errorMessage = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(_defaultTV, out errorMessage)).Returns(true);

            // Act
            var result = _tvStorageController.AddTV(_defaultTV);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(_defaultTV, createdResult.Value);
        }

        [Fact]
        public void GetTVById_ShouldReturnOk()
        {
            // Arrange
            _mockTVService.Setup(service => service.GetTVById(1)).Returns(_defaultTV);

            // Act
            var result = _tvStorageController.GetTVById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(_defaultTV, okResult.Value);
        }

        [Fact]
        public void GetAllTVs_ShouldReturnOk()
        {
            // Arrange
            var tvs = new List<TV>
            {
                _defaultTV,
                _tvFactory.CreateTV("Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5)
            };

            _mockTVService.Setup(service => service.GetAllTVs()).Returns(tvs);

            // Act
            var result = _tvStorageController.GetAllTVs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tvs, okResult.Value);
        }

        [Fact]
        public void UpdateInvalidTV_ShouldReturnBadRequest()
        {
            // Arrange
            var invalidTV = new TV();
            string errorMessage = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(invalidTV, out errorMessage)).Returns(false);

            // Act
            var result = _tvStorageController.UpdateTV(1, invalidTV);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public void UpdateTV_ShouldReturnOk()
        {
            // Arrange
            var updatedTV = _tvFactory.CreateTV("LG", "Bravia", 65, "7680x4320", 120, 2019, 32000, 2);
            string errorMessage = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(updatedTV, out errorMessage)).Returns(true);
            _mockTVService.Setup(service => service.UpdateTV(1, updatedTV)).Returns(updatedTV);

            // Act
            var result = _tvStorageController.UpdateTV(1, updatedTV);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedTV, okResult.Value);
        }

        [Fact]
        public void DeleteTV_ShouldReturnNoContent()
        {
            // Arrange
            _mockTVService.Setup(service => service.DeleteTV(1));

            // Act
            var result = _tvStorageController.DeleteTV(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}