using FileToolKit.IO.File.Interfaces;

namespace FileToolKit.IO.File.Factories
{
    public interface IFileToolFactory<T> where T : class
    {
        IFileTool<T> GetTool(string extension);
    }
}