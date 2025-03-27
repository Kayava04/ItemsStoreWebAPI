namespace ItemsStoreWebAPI.Services
{
    public interface ICsvService<T> where T : class
    {
        Task<IEnumerable<T>> ImportFromCsvAsync(Stream csvStream);
        Task<byte[]> ExportToCsvAsync(IEnumerable<T> items);
    }
}