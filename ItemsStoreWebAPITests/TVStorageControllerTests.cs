using ItemsStoreWebAPI.Controllers;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace ItemsStoreWebAPITests
{
    public class TVStorageControllerTests
    {
        private readonly Mock<ITVService> _mockTVService;
        private readonly Mock<ITVRequestValidator> _mockTVRequestValidator;
        private readonly TVStorageController _tvStorageController;

        public TVStorageControllerTests()
        {
            _mockTVService = new Mock<ITVService>();
            _mockTVRequestValidator = new Mock<ITVRequestValidator>();
            _tvStorageController = new TVStorageController(_mockTVService.Object, _mockTVRequestValidator.Object);
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
            var validTV = new TV
            {
                Name = "LG",
                Size = 55,
                Resolution = "3840x2160",
                Frequency = 60,
                ReleasedYear = 2021,
                Price = 1200,
                InStock = 5
            };

            string errorMessage = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(validTV, out errorMessage)).Returns(true);

            // Act
            var result = _tvStorageController.AddTV(validTV);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(validTV, createdResult.Value);
        }

        [Fact]
        public void GetTVById_ShouldReturnOk()
        {
            // Arrange
            var tv = new TV
            {
                ID = 1,
                Name = "Samsung",
                Size = 50
            };

            _mockTVService.Setup(s => s.GetTVById(1)).Returns(tv);

            // Act
            var result = _tvStorageController.GetTVById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tv, okResult.Value);
        }

        [Fact]
        public void GetAllTVs_ShouldReturnOk()
        {
            // Arrange
            var tvList = new List<TV>
            {
                new TV
                {
                    ID = 1,
                    Name = "Samsung",
                    Size = 50
                },
                new TV
                {
                    ID = 2,
                    Name = "LG",
                    Size = 75
                }
            };

            _mockTVService.Setup(s => s.GetAllTVs()).Returns(tvList);

            // Act
            var result = _tvStorageController.GetAllTVs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tvList, okResult.Value);
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
            var updatedTV = new TV
            {
                ID = 1,
                Name = "Sony",
                Size = 65
            };

            string errorMessage = string.Empty;

            _mockTVRequestValidator.Setup(v => v.IsValid(updatedTV, out errorMessage)).Returns(true);
            _mockTVService.Setup(s => s.UpdateTV(1, updatedTV)).Returns(updatedTV);

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