using ItemsStoreWebAPI.Repositories;
using Microsoft.Extensions.Options;

namespace ItemsStoreWebAPI.Factories
{
    public class TVStorageFactory : ITVStorageFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _storageSettings;

        public TVStorageFactory(IServiceProvider serviceProvider, IOptions<StorageSettings> options)
        {
            _serviceProvider = serviceProvider;
            _storageSettings = options.Value.DefaultStorageType;
        }
        
        public ITVStorage CreateStorage(string? type = null)
        {
            var storageType = type ?? _storageSettings;
            return storageType switch
            {
                "ListStorage" => _serviceProvider.GetRequiredService<TVListStorage>(),
                "DictionaryStorage" => _serviceProvider.GetRequiredService<TVDictionaryStorage>(),
                _ => throw new ArgumentException($"Invalid storage type: {type}!")
            };
        }
    }
}