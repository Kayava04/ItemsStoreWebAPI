using ItemsStoreWebAPI.Factories;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPITests.Factories;
using Moq;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVServiceTests
    {
        [Fact]
        public void AddTV_ShouldAddTVToStorage()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            var mockTVStorage = new Mock<ITVStorage>();
            var mockTVStorageFactory = new Mock<ITVStorageFactory>();
            
            mockTVStorageFactory.Setup(factory => factory.CreateStorage(It.IsAny<string?>())).Returns(mockTVStorage.Object);
            
            var tvService = new TVService(mockTVStorageFactory.Object);

            mockTVStorage.Setup(storage => storage.AddTV(It.IsAny<TV>())).Returns(expectedTV);

            // Act
            var result = tvService.AddTV(expectedTV);

            // Assert
            mockTVStorage.Verify(storage => storage.AddTV(expectedTV), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(expectedTV.ID, result.ID);
            Assert.Equal(expectedTV.Name, result.Name);
            Assert.Equal(expectedTV.Description, result.Description);
            Assert.Equal(expectedTV.Size, result.Size);
            Assert.Equal(expectedTV.Resolution, result.Resolution);
            Assert.Equal(expectedTV.Frequency, result.Frequency);
            Assert.Equal(expectedTV.ReleasedYear, result.ReleasedYear);
            Assert.Equal(expectedTV.Price, result.Price);
            Assert.Equal(expectedTV.InStock, result.InStock);
        }

        [Fact]
        public void GetTVById_ShouldReturnCorrectTV()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            
            var mockTVStorage = new Mock<ITVStorage>();
            var mockTVStorageFactory = new Mock<ITVStorageFactory>();
            
            mockTVStorageFactory.Setup(factory => factory.CreateStorage(It.IsAny<string?>())).Returns(mockTVStorage.Object);
            
            var tvService = new TVService(mockTVStorageFactory.Object);

            mockTVStorage.Setup(storage => storage.GetTVById(expectedTV.ID)).Returns(expectedTV);

            // Act
            var result = tvService.GetTVById(expectedTV.ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTV.ID, result.ID);
            Assert.Equal(expectedTV.Name, result.Name);
            Assert.Equal(expectedTV.Description, result.Description);
            Assert.Equal(expectedTV.Size, result.Size);
            Assert.Equal(expectedTV.Resolution, result.Resolution);
            Assert.Equal(expectedTV.Frequency, result.Frequency);
            Assert.Equal(expectedTV.ReleasedYear, result.ReleasedYear);
            Assert.Equal(expectedTV.Price, result.Price);
            Assert.Equal(expectedTV.InStock, result.InStock);
        }

        [Fact]
        public void GetAllTVs_ShoudReturnListOfTVs()
        {
            // Arrange
            var defaultTV = TVFactory.CreateDefaultTV();
            var tvs = new List<TV>
            {
                defaultTV,
                TVFactory.CreateTV(2, "Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5)
            };
            
            var mockTVStorage = new Mock<ITVStorage>();
            var mockTVStorageFactory = new Mock<ITVStorageFactory>();
            
            mockTVStorageFactory.Setup(factory => factory.CreateStorage(It.IsAny<string?>())).Returns(mockTVStorage.Object);
            
            var tvService = new TVService(mockTVStorageFactory.Object);

            mockTVStorage.Setup(storage => storage.GetAllTVs()).Returns(tvs);

            // Act
            var result = tvService.GetAllTVs();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, tv => tv.ID == defaultTV.ID);
            Assert.Contains(result, tv => tv.ID == tvs[1].ID);
        }

        [Fact]
        public void GetFilteredTVs_ShouldReturnCorrectTVs()
        {
        }

        [Fact]
        public void UpdateTV_ShouldUpdateTVById()
        {
            // Arrange
            var existingTV = TVFactory.CreateDefaultTV();
            var updatedTV = TVFactory.CreateTV(1, "LG", "Bravia", 65, "7680x4320", 120, 2019, 32000, 2);
            
            var mockTVStorage = new Mock<ITVStorage>();
            var mockTVStorageFactory = new Mock<ITVStorageFactory>();

            mockTVStorageFactory.Setup(factory => factory.CreateStorage(It.IsAny<string?>())).Returns(mockTVStorage.Object);
            
            var tvService = new TVService(mockTVStorageFactory.Object);

            mockTVStorage.Setup(storage => storage.UpdateTV(existingTV.ID, updatedTV)).Returns(updatedTV);

            // Act
            var result = tvService.UpdateTV(updatedTV.ID, updatedTV);

            // Assert
            mockTVStorage.Verify(storage => storage.UpdateTV(existingTV.ID, updatedTV), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(existingTV.ID, result.ID);
            Assert.Equal(updatedTV.Name, result.Name);
            Assert.Equal(updatedTV.Description, result.Description);
            Assert.Equal(updatedTV.Size, result.Size);
            Assert.Equal(updatedTV.Resolution, result.Resolution);
            Assert.Equal(updatedTV.Frequency, result.Frequency);
            Assert.Equal(updatedTV.ReleasedYear, result.ReleasedYear);
            Assert.Equal(updatedTV.Price, result.Price);
            Assert.Equal(updatedTV.InStock, result.InStock);
        }
        
        [Fact]
        public void DeleteTV_ShouldRemoveTVFromStorage()
        {
            // Arrange
            var existingTV = TVFactory.CreateDefaultTV();
            
            var mockTVStorage = new Mock<ITVStorage>();
            var mockTVStorageFactory = new Mock<ITVStorageFactory>();
            
            mockTVStorageFactory.Setup(factory => factory.CreateStorage(It.IsAny<string?>())).Returns(mockTVStorage.Object);
            
            var tvService = new TVService(mockTVStorageFactory.Object);

            // Act
            tvService.DeleteTV(existingTV.ID);

            // Assert
            mockTVStorage.Verify(storage => storage.DeleteTV(existingTV.ID), Times.Once);
        }
    }
}