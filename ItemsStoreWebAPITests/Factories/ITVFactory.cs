using ItemsStoreWebAPI.Models;


namespace ItemsStoreWebAPITests.Factories
{
    public interface ITVFactory
    {
        TV CreateTV(string name, string description, float size, string resolution, float frequency, int releasedYear, decimal price, int inStock);
        TV CreateDefaultTV();
    }
}