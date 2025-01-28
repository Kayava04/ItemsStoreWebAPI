using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPITests.Factories;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVStorageTests
    {
        private TVStorage _tvStorage;
        private TV _defaultTV;

        [Fact]
        public void AddTV_ShouldAddTVWithUniqueID()
        {
            // Arrange
            _tvStorage = new TVStorage();
            _defaultTV = TVFactory.CreateDefaultTV();

            // Act
            var result = _tvStorage.AddTV(_defaultTV);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_defaultTV.ID, result.ID);
            Assert.Equal(_defaultTV.Name, result.Name);
        }

        [Fact]
        public void GetTVById_ShouldReturnCorrectTV()
        {
            // Arrange
            _tvStorage = new TVStorage();
            _defaultTV = TVFactory.CreateDefaultTV();

            _tvStorage.AddTV(_defaultTV);

            // Act
            var result = _tvStorage.GetTVById(_defaultTV.ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_defaultTV.ID, result.ID);
            Assert.Equal(_defaultTV.Name, result.Name);
        }

        [Fact]
        public void GetAllTVs_ShouldReturnAllAddedTVs()
        {
            // Arrange
            _tvStorage = new TVStorage();
            _defaultTV = TVFactory.CreateDefaultTV();
            var newTV = TVFactory.CreateTV(2, "Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5);

            _tvStorage.AddTV(_defaultTV);
            _tvStorage.AddTV(newTV);

            // Act
            var result = _tvStorage.GetAllTVs();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void UpdateTV_ShouldModifyExistingTV()
        {
            // Arrange
            _tvStorage = new TVStorage();
            _defaultTV = TVFactory.CreateDefaultTV();
            var updatedTV = TVFactory.CreateTV(1, "Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5);

            _tvStorage.AddTV(_defaultTV);

            // Act
            var result = _tvStorage.UpdateTV(_defaultTV.ID, updatedTV);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_defaultTV.ID, result.ID);
            Assert.Equal("Samsung", result.Name);
            Assert.Equal(50, result.Size);
            Assert.Equal(45000, result.Price);
        }

        [Fact]
        public void DeleteTV_ShouldRemoveTVFromStorage()
        {
            // Arrange
            _tvStorage = new TVStorage();
            _defaultTV = TVFactory.CreateDefaultTV();

            // Act
            _tvStorage.DeleteTV(_defaultTV.ID);
            var result = _tvStorage.GetTVById(_defaultTV.ID);

            // Assert
            Assert.Null(result);
        }
    }
}