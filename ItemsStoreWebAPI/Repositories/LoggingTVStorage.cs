using ItemsStoreWebAPI.Models;


namespace ItemsStoreWebAPI.Repositories
{
     public class LoggingTVStorage : ITVStorage
     {
         private readonly ITVStorage _innerTVCollection;
         private readonly ILogger<LoggingTVStorage> _logger;
     
         public LoggingTVStorage(ITVStorage tvCollection, ILogger<LoggingTVStorage> logger)
         {
             _innerTVCollection = tvCollection;
             _logger = logger;
         }
     
         public TV AddTV(TV tv)
         {
             var addedTV = _innerTVCollection.AddTV(tv);
             _logger.LogInformation($"TV added: {tv}");
             return addedTV;
         }
     
         public TV? GetTVById(int id)
         {
             var tv = _innerTVCollection.GetTVById(id);
             
             if (tv != null)
                 _logger.LogInformation($"TV with ID: {id} found. {tv}");
             else
                 _logger.LogError($"TV with ID: {id} not found");
             
             return tv;
         }
     
         public IEnumerable<TV> GetAllTVs()
         {
             var tvs = _innerTVCollection.GetAllTVs();
             _logger.LogInformation($"Retrieving all TVs, count: {tvs.Count()}");
             return tvs;
         }
         
         public TV? UpdateTV(int id, TV updatedTV)
         {
             _logger.LogInformation($"Updating TV with ID: {id}");
             
             var tv = _innerTVCollection.UpdateTV(id, updatedTV);
             
             if (tv != null)
                 _logger.LogInformation($"TV with ID: {id} successfully updated. {tv}");
             else
                 _logger.LogError($"Attempt to update TV with ID: {id} failed. NOT FOUND");
             
             return tv;
         }
     
         public void DeleteTV(int id)
         {
             var tv = _innerTVCollection.GetTVById(id);
     
             if (tv != null)
             {
                 _innerTVCollection.DeleteTV(id);
                 _logger.LogInformation($"TV with ID: {id} deleted");
             }
             else
                 _logger.LogError($"TV with ID: {id} not found");
         }
     }   
}