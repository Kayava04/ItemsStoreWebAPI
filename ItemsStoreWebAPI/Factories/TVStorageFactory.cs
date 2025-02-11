using ItemsStoreWebAPI.Repositories;


namespace ItemsStoreWebAPI.Factories
{
    public class TVStorageFactory : ITVStorageFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public TVStorageFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public ITVStorage CreateStorage(string type)
        {
            return type switch
            {
                "ListStorage" => _serviceProvider.GetRequiredService<TVListStorage>(),
                "DictionaryStorage" => _serviceProvider.GetRequiredService<TVDictionaryStorage>(),
                _ => throw new ArgumentException($"Invalid storage type: {type}!")
            };
        }
    }
}