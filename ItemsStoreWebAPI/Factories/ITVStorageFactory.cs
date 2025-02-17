using ItemsStoreWebAPI.Repositories;


namespace ItemsStoreWebAPI.Factories
{
    public interface ITVStorageFactory
    {
        ITVStorage CreateStorage(string? type = null);
    }
}