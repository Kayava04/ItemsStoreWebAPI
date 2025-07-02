using FileToolKit.IO.File.Factories;

namespace ItemsStoreWebAPI.Services
{
    public class FileService<T>(
        IService<T> service,
        IFileToolFactory<T> fileService,
        ILogger<FileService<T>> logger): IFileService<T> where T : class
    {
        public async Task<IEnumerable<T>> ImportFromFileAsync(IFormFile file)
        {
            if (file.Length == 0)
                logger.LogError("File is empty");
            
            var extension = Path.GetExtension(file.FileName);
            var tool = fileService.GetTool(extension);
            
            await using var stream = file.OpenReadStream();
            var importedData = await tool.ImportAsync(stream);
            
            foreach (var tv in importedData)
                service.AddAsync(tv);
            
            logger.LogInformation($"Imported {importedData.Count()} TVs from file: {file.FileName}");
            return importedData;
        }

        public async Task<(byte[] data, string contentType, string downloadFileName)> ExportToFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                logger.LogError("Missing or empty FileName");

            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension))
                logger.LogError("File extension not found in FileName");
            
            var tool = fileService.GetTool(extension);

            var data = await service.GetAllAsync();
            var fileData = await tool.ExportAsync(data);

            var contentType = extension.ToLower() switch
            {
                ".csv" => "text/csv",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };

            var finalName = $"{DateTime.UtcNow:yyyyMMdd}-TVs-{fileName}";
            return (fileData, contentType, finalName);
        }
    }
}