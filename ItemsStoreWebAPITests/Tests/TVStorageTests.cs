using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPITests.Factories;
using Moq;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVStorageTests
    {
        private readonly Mock<ITVStorage> _mockTVStorage;
        private readonly ITVService _tvService;
        private readonly ITVFactory _tvFactory;
        private readonly TV _defaultTV;

        public TVStorageTests()
        {
            _mockTVStorage = new Mock<ITVStorage>();
            _tvFactory = new TVFactory();

            //TODO: Read when this way realization better to use
            //var instance = new TVService(_mockTVStorage.Object);
            _tvService = new TVService(_mockTVStorage.Object);
            _defaultTV = _tvFactory.CreateDefaultTV();
        }

        [Fact]
        public void AddTV_ShouldAddTVToStorage()
        {
            // Arrange
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
            var tvs = new List<TV>
            {
                _defaultTV,
                _tvFactory.CreateTV("Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5)
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
            var updatedTV = _tvFactory.CreateTV("LG", "Bravia", 65, "7680x4320", 120, 2019, 32000, 2);

            _mockTVStorage.Setup(storage => storage.UpdateTV(_defaultTV.ID, updatedTV)).Returns(updatedTV);

            // Act
            var result = _tvService.UpdateTV(_defaultTV.ID, updatedTV);

            // Assert
            _mockTVStorage.Verify(storage => storage.UpdateTV(_defaultTV.ID, updatedTV), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(updatedTV.Name, result.Name);
            Assert.Equal(updatedTV.Price, result.Price);
        }

        [Fact]
        public void DeleteTV_ShouldRemoveTVFromStorage()
        {
            // Arrange
            var tvs = new List<TV> { _defaultTV };

            _mockTVStorage.Setup(storage => storage.GetAllTVs()).Returns(tvs);
            _mockTVStorage.Setup(storage => storage.DeleteTV(_defaultTV.ID)).Callback(() => tvs.Remove(_defaultTV));

            // Act
            _tvService.DeleteTV(_defaultTV.ID);

            // Assert
            _mockTVStorage.Verify(storage => storage.DeleteTV(_defaultTV.ID), Times.Once);
            Assert.Empty(tvs);
        }
    }
}