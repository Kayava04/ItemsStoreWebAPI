using ItemsStoreWebAPI.Repositories;
using Microsoft.Extensions.Options;

namespace ItemsStoreWebAPI.Factories
{
    public class TVStorageFactory : ITVStorageFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _storageSettings;

        public TVStorageFactory(IServiceScopeFactory scopeFactory, IOptions<StorageSettings> options)
        {
            _scopeFactory = scopeFactory;
            _storageSettings = options.Value.DefaultStorageType;
        }
        
        public ITVStorage CreateStorage(string? type = null)
        {
            var storageType = type ?? _storageSettings;
            
            var scope = _scopeFactory.CreateScope();
            var provider = scope.ServiceProvider;
            
            return storageType switch
            {
                "ListStorage" => provider.GetRequiredService<TVListStorage>(),
                "DictionaryStorage" => provider.GetRequiredService<TVDictionaryStorage>(),
                "DbStorage" => provider.GetRequiredService<TvDbStorage>(),
                _ => throw new ArgumentException($"Invalid storage type: {type}!")
            };
        }
    }
}