using ItemsStoreWebAPI.Repositories;
using ItemsStoreWebAPITests.Factories;
using Microsoft.Extensions.Logging;
using Moq;


namespace ItemsStoreWebAPITests.Tests
{
    public class TVStorageTests
    {
        [Fact]
        public void AddTV_ShouldAddTVWithUniqueID()
        {
            // Arrange
            var expectedID = 1;
            var expectedTV = TVFactory.CreateTV(0, "LG", "OLED TV", 55, "1920x1080", 100.5f, 2020, 25500, 2);
            
            var mockLogger = new Mock<ILogger<TVStorage>>();
            var tvStorage = new TVStorage(mockLogger.Object);
            
            // Act
            var result = tvStorage.AddTV(expectedTV);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedID, result.ID);
            Assert.Equal(expectedTV.Name, result.Name);
            Assert.Equal(expectedTV.Description, result.Description);
            Assert.Equal(expectedTV.Size, result.Size);
            Assert.Equal(expectedTV.Resolution, result.Resolution);
            Assert.Equal(expectedTV.Frequency, result.Frequency);
            Assert.Equal(expectedTV.ReleasedYear, result.ReleasedYear);
            Assert.Equal(expectedTV.Price, result.Price);
            Assert.Equal(expectedTV.InStock, result.InStock);
            
            //TODO: Check how to realize mockLogger
            // mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GetTVById_ShouldReturnCorrectTV()
        {
            // Arrange
            var expectedTV = TVFactory.CreateDefaultTV();
            
            var mockLogger = new Mock<ILogger<TVStorage>>();
            var tvStorage = new TVStorage(mockLogger.Object);

            tvStorage.AddTV(expectedTV);

            // Act
            var result = tvStorage.GetTVById(expectedTV.ID);

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
            
            // mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Fact]
        public void GetAllTVs_ShouldReturnAllAddedTVs()
        {
            // Arrange
            var defaultTV = TVFactory.CreateDefaultTV();
            var newTV = TVFactory.CreateTV(2, "Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5);
            
            var mockLogger = new Mock<ILogger<TVStorage>>();
            var tvStorage = new TVStorage(mockLogger.Object);

            tvStorage.AddTV(defaultTV);
            tvStorage.AddTV(newTV);

            // Act
            var result = tvStorage.GetAllTVs();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, tv => tv.ID == defaultTV.ID);
            Assert.Contains(result, tv => tv.ID == newTV.ID);
            
            // mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdateTV_ShouldModifyExistingTV()
        {
            // Arrange
            var existingTV = TVFactory.CreateDefaultTV();
            var updatedTV = TVFactory.CreateTV(1, "Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5);
            
            var mockLogger = new Mock<ILogger<TVStorage>>();
            var tvStorage = new TVStorage(mockLogger.Object);

            tvStorage.AddTV(existingTV);

            // Act
            var result = tvStorage.UpdateTV(updatedTV.ID, updatedTV);

            // Assert
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
            
            // mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdateTV_ShouldReturnNullForNonExistingTV()
        {
            // Arrange
            var nonExistingID = int.MaxValue;
            var updatedTV = TVFactory.CreateTV(nonExistingID, "Samsung", "OLED TV", 50, "1920x1080", 120, 2022, 45000, 5);
            
            var mockLogger = new Mock<ILogger<TVStorage>>();
            var tvStorage = new TVStorage(mockLogger.Object);
            
            // Act
            var result = tvStorage.UpdateTV(updatedTV.ID, updatedTV);
            
            // Assert
            Assert.Null(result);
            
            // mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeastOnce());
        }
        
        [Fact]
        public void DeleteTV_ShouldRemoveTVFromStorage()
        {
            // Arrange
            var existingTV = TVFactory.CreateDefaultTV();
            
            var mockLogger = new Mock<ILogger<TVStorage>>();
            var tvStorage = new TVStorage(mockLogger.Object);

            // Act
            tvStorage.DeleteTV(existingTV.ID);
            var result = tvStorage.GetTVById(existingTV.ID);

            // Assert
            Assert.Null(result);
            
            // mockLogger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [Fact]
        public void DeleteTV_ShouldReturnNullForNonExistingTV()
        {
            // Arrange
            var nonExistingID = int.MaxValue;
            
            var mockLogger = new Mock<ILogger<TVStorage>>();
            var tvStorage = new TVStorage(mockLogger.Object);
            
            // Act
            var exception = Record.Exception(() => tvStorage.DeleteTV(nonExistingID));
            
            // Assert
            Assert.Null(exception);
            
            // mockLogger.Verify(x => x.LogError(It.IsAny<string>()), Times.Once());
        }
    }
}