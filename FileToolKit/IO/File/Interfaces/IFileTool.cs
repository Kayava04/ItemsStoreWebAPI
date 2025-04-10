namespace FileToolKit.IO.File.Interfaces
{
    public interface IFileTool<T> where T : class
    {
        Task<IEnumerable<T>> ImportAsync(Stream stream);
        Task<byte[]> ExportAsync(IEnumerable<T> items);
    }
}