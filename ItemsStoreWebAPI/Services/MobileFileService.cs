using FileToolKit.IO.File.Factories;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Services
{
    public class MobileFileService(
        IServiceBase<Mobile> mobileService,
        IFileToolFactory<Mobile> fileService,
        ILogger<MobileFileService> logger) : IFileService<Mobile>
    {
        public async Task<IEnumerable<Mobile>> ImportFromFileAsync(IFormFile file)
        {
            if (file.Length == 0)
                logger.LogError("File is empty");
            
            var extension = Path.GetExtension(file.FileName);
            var tool = fileService.GetTool(extension);
            
            await using var stream = file.OpenReadStream();
            var importedData = await tool.ImportAsync(stream);
            
            foreach (var mobile in importedData)
                mobileService.AddAsync(mobile);
            
            logger.LogInformation($"Imported {importedData.Count()} Mobiles from file: {file.FileName}");
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

            var data = await mobileService.GetAllAsync();
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