using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPITests.Factories;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVStorageTests
    {
        private TV _defaultTV;

        [Fact]
        public void AddTV_ShouldAddTVWithUniqueID()
        {
            // Arrange
            int expectedID = 1;
            var _tvStorage = new TVStorage();
            
            //TODO: Write asserts for other parameters
            //      Do the same thing in every method
            
            _defaultTV = TVFactory.CreateTV(0, "LG", "OLED TV", 55, "1920x1080", 100.5f, 2020, 25500, 2);
            
            // Act
            var result = _tvStorage.AddTV(_defaultTV);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedID, result.ID);
            Assert.Equal(_defaultTV.Name, result.Name);
        }

        [Fact]
        public void GetTVById_ShouldReturnCorrectTV()
        {
            // Arrange
            var _tvStorage = new TVStorage();
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
            var _tvStorage = new TVStorage();
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
            var _tvStorage = new TVStorage();
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
            var _tvStorage = new TVStorage();
            _defaultTV = TVFactory.CreateDefaultTV();

            // Act
            _tvStorage.DeleteTV(_defaultTV.ID);
            var result = _tvStorage.GetTVById(_defaultTV.ID);

            // Assert
            Assert.Null(result);
        }
        
        //TODO: Implement methods(update & delete) for non-existent ID
    }
}