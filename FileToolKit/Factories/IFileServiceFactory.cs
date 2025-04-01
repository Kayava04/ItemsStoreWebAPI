using FileToolKit.Interfaces;

namespace FileToolKit.Factories
{
    public interface IFileServiceFactory<T> where T : class
    {
        IFileHandler<T> GetService(string? type = null);
    }
}