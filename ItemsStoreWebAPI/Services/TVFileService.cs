using FileToolKit.IO.File.Factories;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Services
{
    public class TVFileService : IFileService<TV>
    {
        private readonly ITVService _tvService;
        private readonly IFileToolFactory<TV> _fileService;
        private readonly ILogger<TVFileService> _logger;

        public TVFileService(ITVService tvService, IFileToolFactory<TV> fileService, ILogger<TVFileService> logger)
        {
            _tvService = tvService;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<IEnumerable<TV>> ImportFromFileAsync(IFormFile file)
        {
            if (file.Length == 0)
                _logger.LogError("File is empty");
            
            var extension = Path.GetExtension(file.FileName);
            var tool = _fileService.GetTool(extension);
            
            await using var stream = file.OpenReadStream();
            var importedData = await tool.ImportAsync(stream);
            
            foreach (var tv in importedData)
                _tvService.AddTV(tv);
            
            _logger.LogInformation($"Imported {importedData.Count()} TVs from file: {file.FileName}");
            return importedData;
        }
        
        public async Task<(byte[] data, string contentType, string downloadFileName)> ExportToFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                _logger.LogError("Missing or empty FileName");

            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension))
                _logger.LogError("File extension not found in FileName");
            
            var tool = _fileService.GetTool(extension);

            var data = await _tvService.GetTVs();
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