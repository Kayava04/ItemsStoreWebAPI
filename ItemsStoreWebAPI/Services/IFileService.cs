namespace ItemsStoreWebAPI.Services
{
    public interface IFileService<T> where T : class
    {
        Task<IEnumerable<T>> ImportFromFileAsync(IFormFile file);
        Task<(byte[] data, string contentType, string downloadFileName)> ExportToFileAsync(string fileName);
    }
}