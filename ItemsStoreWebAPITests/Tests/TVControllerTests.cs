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
        [Fact]
        public void AddInvalidTV_ShouldReturnBadRequest()
        {
            // Arrange
            var expectedTV = new TV();
            var errorMessage = string.Empty;
            
            var mockTVService = new Mock<ITVService>();
            var mockTVRequestValidator = new Mock<ITVRequestValidator>();
            var mockTVFileService = new Mock<IFileService<TV>>();
            var mockLogger = new Mock<ILogger<TVController>>();
            var tvController = new TVController(mockTVService.Object, mockTVRequestValidator.Object, mockTVFileService.Object, mockLogger.Object);

            mockTVRequestValidator.Setup(v => v.IsValid(expectedTV, out errorMessage)).Returns(false);

            // Act
            var result = tvController.AddTV(expectedTV);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public void AddTV_ShouldReturnCreated()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            var errorMessage = string.Empty;
            
            var mockTVService = new Mock<ITVService>();
            var mockTVRequestValidator = new Mock<ITVRequestValidator>();
            var mockTVFileService = new Mock<IFileService<TV>>();
            var mockLogger = new Mock<ILogger<TVController>>();
            var tvController = new TVController(mockTVService.Object, mockTVRequestValidator.Object, mockTVFileService.Object, mockLogger.Object);

            mockTVRequestValidator.Setup(v => v.IsValid(expectedTV, out errorMessage)).Returns(true);

            // Act
            var result = tvController.AddTV(expectedTV);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(expectedTV, createdResult.Value);
        }

        [Fact]
        public void GetTVById_ShouldReturnOk()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            
            var mockTVService = new Mock<ITVService>();
            var mockTVRequestValidator = new Mock<ITVRequestValidator>();
            var mockTVFileService = new Mock<IFileService<TV>>();
            var mockLogger = new Mock<ILogger<TVController>>();
            var tvController = new TVController(mockTVService.Object, mockTVRequestValidator.Object, mockTVFileService.Object, mockLogger.Object);

            mockTVService.Setup(service => service.GetTVById(expectedTV.ID)).Returns(expectedTV);
            mockTVRequestValidator.Setup(v => v.IsValid(expectedTV, out It.Ref<string>.IsAny)).Returns(true);

            // Act
            var result = tvController.GetTVById(expectedTV.ID);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedTV, okResult.Value);
        }

        [Fact]
        public void GetAllTVs_ShouldReturnOk()
        {
            // Arrange
            var defaultTV = TVFactory.CreateDefaultTV();
            var tvs = new List<TV>
            {
                defaultTV,
                TVFactory.CreateTV(2, "Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5)
            };
            
            var mockTVService = new Mock<ITVService>();
            var mockTVRequestValidator = new Mock<ITVRequestValidator>();
            var mockTVFileService = new Mock<IFileService<TV>>();
            var mockLogger = new Mock<ILogger<TVController>>();
            var tvController = new TVController(mockTVService.Object, mockTVRequestValidator.Object, mockTVFileService.Object, mockLogger.Object);

            mockTVService.Setup(service => service.GetAllTVs(null)).Returns(tvs);

            // Act
            var result = tvController.GetAllTVs();

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
            
            var mockTVService = new Mock<ITVService>();
            var mockTVRequestValidator = new Mock<ITVRequestValidator>();
            var mockTVFileService = new Mock<IFileService<TV>>();
            var mockLogger = new Mock<ILogger<TVController>>();
            var tvController = new TVController(mockTVService.Object, mockTVRequestValidator.Object, mockTVFileService.Object, mockLogger.Object);

            mockTVRequestValidator.Setup(v => v.IsValid(expectedTV, out errorMessage)).Returns(false);

            // Act
            var result = tvController.UpdateTV(expectedTV);

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
            
            var mockTVService = new Mock<ITVService>();
            var mockTVRequestValidator = new Mock<ITVRequestValidator>();
            var mockTVFileService = new Mock<IFileService<TV>>();
            var mockLogger = new Mock<ILogger<TVController>>();
            var tvController = new TVController(mockTVService.Object, mockTVRequestValidator.Object, mockTVFileService.Object, mockLogger.Object);

            mockTVRequestValidator.Setup(v => v.IsValid(updatedTV, out errorMessage)).Returns(true);
            mockTVService.Setup(service => service.UpdateTV(updatedTV.ID, updatedTV)).Returns(updatedTV);

            // Act
            var result = tvController.UpdateTV(updatedTV);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedTV, okResult.Value);
        }

        [Fact]
        public void DeleteTV_ShouldReturnNoContent()
        {
            // Arrange
            var existingTV = TVFactory.CreateDefaultTV();
            
            var mockTVService = new Mock<ITVService>();
            var mockTVRequestValidator = new Mock<ITVRequestValidator>();
            var mockTVFileService = new Mock<IFileService<TV>>();
            var mockLogger = new Mock<ILogger<TVController>>();
            var tvController = new TVController(mockTVService.Object, mockTVRequestValidator.Object, mockTVFileService.Object, mockLogger.Object);

            mockTVService.Setup(service => service.DeleteTV(existingTV.ID));

            // Act
            var result = tvController.DeleteTV(existingTV.ID);

            // Assert
            mockTVService.Verify(service => service.DeleteTV(1), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
    }
}