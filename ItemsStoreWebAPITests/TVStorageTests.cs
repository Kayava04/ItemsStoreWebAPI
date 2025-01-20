using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using Moq;


namespace ItemsStoreWebAPI
{
    public class TVStorageTests
    {
        private readonly Mock<ITVStorage> _mockTVStorage;

        public TVStorageTests()
        {
            _mockTVStorage = new Mock<ITVStorage>();
        }

        [Fact]
        public void AddTV_ShouldAddTVToStorage()
        {
            // Arange
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
                AddedAt = DateTime.UtcNow,
                InStock = 5
            };

            _mockTVStorage.Setup(storage => storage.AddTV(tv));

            var allTVs = new List<TV> { tv };
            _mockTVStorage.Setup(storage => storage.GetAllTVs()).Returns(allTVs);

            // Act
            var result = _mockTVStorage.Object.GetAllTVs();

            // Assert
            Assert.Single(result);
            Assert.NotEqual(0, result.First().ID);
            Assert.Equal(tv.Name, result.First().Name);
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
                AddedAt = DateTime.UtcNow,
                InStock = 3
            };

            _mockTVStorage.Setup(storage => storage.GetTVById(tv.ID)).Returns(tv);

            // Act
            var fetchedTV = _mockTVStorage.Object.GetTVById(tv.ID);

            // Assert
            Assert.NotNull(fetchedTV);
            Assert.Equal(tv.ID, fetchedTV.ID);
            Assert.Equal(tv.Name, fetchedTV.Name);
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
                    AddedAt = DateTime.UtcNow,
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
                    AddedAt = DateTime.UtcNow,
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
                AddedAt = DateTime.UtcNow,
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
                ModifiedAt = DateTime.UtcNow,
                InStock = 2
            };

            _mockTVStorage.Setup(storage => storage.GetTVById(tv.ID)).Returns(tv);
            _mockTVStorage.Setup(storage => storage.UpdateTV(tv.ID, updatedTV)).Verifiable();

            // Act
            _mockTVStorage.Object.UpdateTV(tv.ID, updatedTV);

            // Assert
            _mockTVStorage.Verify(storage => storage.UpdateTV(tv.ID, updatedTV), Times.Once);
            Assert.True(updatedTV.ModifiedAt > default(DateTime));
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
                AddedAt = DateTime.UtcNow,
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