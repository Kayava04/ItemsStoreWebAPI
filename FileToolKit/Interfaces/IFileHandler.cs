namespace FileToolKit.Interfaces
{
    public interface IFileHandler<T> where T : class
    {
        Task<IEnumerable<T>> ReadDataAsync(Stream dataStream);
        Task<byte[]> WriteDataAsync(IEnumerable<T> items);
    }
}