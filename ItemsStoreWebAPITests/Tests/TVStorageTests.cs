using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPI.Services;
using Moq;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVStorageTests
    {
        private readonly Mock<ITVStorage> _mockTVStorage;
        private readonly ITVService _tvService;

        public TVStorageTests()
        {
            _mockTVStorage = new Mock<ITVStorage>();

            //TODO: Read when this way realization better to use
            //_tvService = new TVService(_mockTVStorage.Object);
        }

        //TODO: Write a new realization of this incorrect test
        [Fact]
        public void AddTV_ShouldAddTVToStorage()
        {
            // Arrange
            var tv = new TV
            {
                ID = 1,
                Name = "LG",
                Description = "OLED TV",
                Size = 55,
                Resolution = "3840x2160",
                Frequency = 120,
                ReleasedYear = 2022,
                Price = 50000,
                InStock = 5
            };


            //TODO: replace mock on service
            //_mockTVStorage.Setup(storage => storage.AddTV(It.IsAny<TV>())).Returns(tv);
            var instance = new TVService(_mockTVStorage.Object);
            instance.AddTV(tv);

            // Act
            //_mockTVStorage.Object.AddTV(tv);


            // Assert
            _mockTVStorage.Verify(storage => storage.AddTV(tv), Times.Once);
        }

        [Fact]
        public void GetTVById_ShouldReturnCorrectTV()
        {
            // Arange
            var tv = new TV
            {
                ID = 1,
                Name = "Sony",
                Description = "Bravia",
                Size = 65,
                Resolution = "7680x4320",
                Frequency = 120,
                ReleasedYear = 2023,
                Price = 120000,
                InStock = 3
            };

            _mockTVStorage.Setup(storage => storage.GetTVById(tv.ID)).Returns(tv);

            // Act
            var result = _mockTVStorage.Object.GetTVById(tv.ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tv.ID, result.ID);
            Assert.Equal(tv.Name, result.Name);
        }

        [Fact]
        public void GetAllTVs_ShoudReturnListOfTVs()
        {
            // Arange
            var tvs = new List<TV>
            {
                new TV
                {
                    ID = 1,
                    Name = "Samsung",
                    Description = "QLED TV",
                    Size = 55,
                    Resolution = "3840x2160",
                    Frequency = 60,
                    ReleasedYear = 2021,
                    Price = 20000,
                    InStock = 10
                },

                new TV
                {
                    ID = 2,
                    Name = "LG",
                    Description = "OLED TV",
                    Size = 50,
                    Resolution = "1920x1080",
                    Frequency = 120,
                    ReleasedYear = 2022,
                    Price = 45000,
                    InStock = 5
                }
            };

            _mockTVStorage.Setup(storage => storage.GetAllTVs()).Returns(tvs);

            // Act
            var result = _mockTVStorage.Object.GetAllTVs();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, tv => tv.Name == "Samsung");
            Assert.Contains(result, tv => tv.Name == "LG");
        }

        [Fact]
        public void UpdateTV_ShouldUpdateTVById()
        {
            // Arange
            var tv = new TV
            {
                ID = 1,
                Name = "Sony",
                Description = "Bravia",
                Size = 65,
                Resolution = "7680x4320",
                Frequency = 120,
                ReleasedYear = 2023,
                Price = 300000,
                InStock = 3
            };

            var updatedTV = new TV
            {
                ID = 1,
                Name = "Sony",
                Description = "Bravia",
                Size = 65,
                Resolution = "7680x4320",
                Frequency = 120,
                ReleasedYear = 2019,
                Price = 320000,
                InStock = 2
            };

            _mockTVStorage.Setup(storage => storage.UpdateTV(tv.ID, updatedTV)).Returns(updatedTV);

            // Act
            var result = _mockTVStorage.Object.UpdateTV(tv.ID, updatedTV);

            // Assert
            _mockTVStorage.Verify(storage => storage.UpdateTV(tv.ID, updatedTV), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Sony", result.Name);
            Assert.Equal("Bravia", result.Description);
        }

        [Fact]
        public void DeleteTV_ShouldRemoveTVFromStorage()
        {
            // Arange
            var tv = new TV
            {
                ID = 1,
                Name = "Panasonic",
                Description = "Plasma TV",
                Size = 50,
                Resolution = "1920x1080",
                Frequency = 60,
                ReleasedYear = 2019,
                Price = 8000,
                InStock = 7
            };

            var tvs = new List<TV> { tv };

            _mockTVStorage.Setup(storage => storage.GetAllTVs()).Returns(tvs);
            _mockTVStorage.Setup(storage => storage.DeleteTV(tv.ID)).Callback(() => tvs.Remove(tv));

            // Act
            _mockTVStorage.Object.DeleteTV(tv.ID);

            // Assert
            Assert.Empty(tvs);
        }
    }
}