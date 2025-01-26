using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPITests.Factories;
using Moq;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVStorageTests
    {
        private Mock<ITVStorage> _mockTVStorage;
        private ITVService _tvService;
        private TVFactory _tvFactory;
        private TV _defaultTV;

        public TVStorageTests()
        {
            _tvFactory = new TVFactory();
        }

        [Fact]
        public void AddTV_ShouldAddTVToStorage()
        {
            // Arrange
            _mockTVStorage = new Mock<ITVStorage>();
            _tvService = new TVService(_mockTVStorage.Object);
            _defaultTV = _tvFactory.CreateDefaultTV();

            _mockTVStorage.Setup(storage => storage.AddTV(It.IsAny<TV>())).Returns(_defaultTV);

            // Act
            var result = _tvService.AddTV(_defaultTV);

            // Assert
            _mockTVStorage.Verify(storage => storage.AddTV(_defaultTV), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(_defaultTV.ID, result.ID);
        }

        [Fact]
        public void GetTVById_ShouldReturnCorrectTV()
        {
            // Arrange
            _mockTVStorage = new Mock<ITVStorage>();
            _tvService = new TVService(_mockTVStorage.Object);
            _defaultTV = _tvFactory.CreateDefaultTV();

            _mockTVStorage.Setup(storage => storage.GetTVById(_defaultTV.ID)).Returns(_defaultTV);

            // Act
            var result = _tvService.GetTVById(_defaultTV.ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_defaultTV.ID, result.ID);
            Assert.Equal(_defaultTV.Name, result.Name);
        }

        [Fact]
        public void GetAllTVs_ShoudReturnListOfTVs()
        {
            // Arrange
            _mockTVStorage = new Mock<ITVStorage>();
            _tvService = new TVService(_mockTVStorage.Object);
            _defaultTV = _tvFactory.CreateDefaultTV();
            
            var tvs = new List<TV>
            {
                _defaultTV,
                _tvFactory.CreateTV(2, "Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5)
            };

            _mockTVStorage.Setup(storage => storage.GetAllTVs()).Returns(tvs);

            // Act
            var result = _tvService.GetAllTVs();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, tv => tv.Name == "LG");
            Assert.Contains(result, tv => tv.Name == "Samsung");
        }

        [Fact]
        public void UpdateTV_ShouldUpdateTVById()
        {
            // Arrange
            _mockTVStorage = new Mock<ITVStorage>();
            _tvService = new TVService(_mockTVStorage.Object);
            _defaultTV = _tvFactory.CreateDefaultTV();

            var updatedTV = _tvFactory.CreateTV(1, "LG", "Bravia", 65, "7680x4320", 120, 2019, 32000, 2);

            _mockTVStorage.Setup(storage => storage.UpdateTV(_defaultTV.ID, updatedTV)).Returns(updatedTV);

            // Act
            var result = _tvService.UpdateTV(_defaultTV.ID, updatedTV);

            // Assert
            _mockTVStorage.Verify(storage => storage.UpdateTV(_defaultTV.ID, updatedTV), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(updatedTV.Name, result.Name);
            Assert.Equal(updatedTV.Size, result.Size);
            Assert.Equal(updatedTV.Resolution, result.Resolution);
            Assert.Equal(updatedTV.Price, result.Price);
        }

        [Fact]
        public void UpdateNonExistentTVById_ShouldReturnNull()
        {
            // Arrange
            _mockTVStorage = new Mock<ITVStorage>();
            _tvService = new TVService(_mockTVStorage.Object);

            int nonExistentID = int.MaxValue;
            var updatedTV = _tvFactory.CreateTV(nonExistentID, "LG", "Bravia", 65, "7680x4320", 120, 2019, 32000, 2);

            _mockTVStorage.Setup(storage => storage.UpdateTV(nonExistentID, updatedTV)).Returns((TV?)null);

            // Act
            var result = _tvService.UpdateTV(nonExistentID, updatedTV);

            // Assert
            _mockTVStorage.Verify(storage => storage.UpdateTV(nonExistentID, updatedTV), Times.Once);
            Assert.Null(result);
        }

        [Fact]
        public void DeleteTV_ShouldRemoveTVFromStorage()
        {
            // Arrange
            _mockTVStorage = new Mock<ITVStorage>();
            _tvService = new TVService(_mockTVStorage.Object);
            _defaultTV = _tvFactory.CreateDefaultTV();

            // Act
            _tvService.DeleteTV(_defaultTV.ID);

            // Assert
            _mockTVStorage.Verify(storage => storage.DeleteTV(_defaultTV.ID), Times.Once);
        }

        //TODO: Fix this method for non-existent ID
        [Fact]
        public void DeleteNonExistentTV_ShouldNotCallDeleteInTVStorage()
        {
            // Arrange
            _mockTVStorage = new Mock<ITVStorage>();
            _tvService = new TVService(_mockTVStorage.Object);

            int nonExistentID = int.MaxValue;

            // Act
            _tvService.DeleteTV(nonExistentID);

            // Assert
            _mockTVStorage.Verify(storage => storage.DeleteTV(nonExistentID), Times.Never);
        }
    }
}