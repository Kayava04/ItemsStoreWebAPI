using FileToolKit.Factories;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Services;
using ItemsStoreWebAPI.Validators;
using Microsoft.AspNetCore.Mvc;


namespace ItemsStoreWebAPI.Controllers
{
    [ApiController]
    [Route("v1/stock/electronic/tv")]
    public class TVController : ControllerBase
    {
        private readonly ITVService _tvService;
        // private readonly ICsvService<TV> _csvService;
        private readonly ITVRequestValidator _tvRequestValidator;
        private readonly ILogger<TVController> _logger;
        
        private readonly IFileServiceFactory<TV> _fileServiceFactory;

        public TVController(ITVService tvService, IFileServiceFactory<TV> fileServiceFactory, ITVRequestValidator tvRequestValidator, ILogger<TVController> logger)
        {
            _tvService = tvService;
            // _csvService = csvService;
            _fileServiceFactory = fileServiceFactory;
            _tvRequestValidator = tvRequestValidator;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddTV([FromBody] TV newTV)
        {
            if (!_tvRequestValidator.IsValid(newTV, out var errorMessage))
            {
                _logger.LogWarning($"Received invalid TV data: {errorMessage}");
                return BadRequest(errorMessage);
            }

            _tvService.AddTV(newTV);
            
            _logger.LogInformation($"TV with ID: {newTV.ID}, added successfully");
            return StatusCode(201, newTV);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetTVById(int id)
        {
            var tv = _tvService.GetTVById(id);

            if (!_tvRequestValidator.IsValid(tv, out var errorMessage))
            {
                _logger.LogWarning($"Received invalid TV data: {errorMessage}");
                return BadRequest(errorMessage);
            }
            
            _logger.LogInformation($"Received TV with ID: {id}");
            return Ok(tv);
        }

        [HttpGet]
        public IActionResult GetAllTVs()
        {
            var tvs = _tvService.GetAllTVs();
            
            _logger.LogInformation($"Received all TVs. Total count: {tvs.Count()}");
            return Ok(tvs);
        }

        [HttpGet("filter")]
        public IActionResult GetFilteredTVs()
        {
            var tvs = _tvService.GetFilteredTVs();
            
            _logger.LogInformation($"Filtered TVs");
            return Ok(tvs);
        }

        [HttpPut]
        public IActionResult UpdateTV([FromBody] TV updatedTV)
        {
            if (!_tvRequestValidator.IsValid(updatedTV, out var errorMessage))
            {
                _logger.LogWarning($"Received invalid update TV data: {errorMessage}");
                return BadRequest(errorMessage);
            }

            var tv = _tvService.UpdateTV(updatedTV.ID, updatedTV);
            
            _logger.LogInformation($"TV with ID: {tv.ID}, updated successfully");
            return Ok(tv);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTV(int id)
        {
            _tvService.DeleteTV(id);
            
            _logger.LogInformation($"TV with ID: {id}, deleted successfully");
            return NoContent();
        }

        [HttpPost("import-file")]
        public async Task<IActionResult> ImportDataFromFile(IFormFile file, [FromQuery] string fileType)
        {
            if (file.Length == 0)
            {
                _logger.LogError("File is empty");
                return BadRequest();
            }
            
            var handler = _fileServiceFactory.GetService(fileType);
            await using var stream = file.OpenReadStream();
            var importedData = await handler.ReadDataAsync(stream);

            foreach (var tv in importedData)
            {
                if (_tvRequestValidator.IsValid(tv, out _))
                    _tvService.AddTV(tv);
            }
            
            _logger.LogInformation($"TV data imported successfully from {fileType.ToUpper()} file");
            return Ok(importedData);
        }

        [HttpPost("export-file")]
        public async Task<IActionResult> ExportDataToFile([FromQuery] string fileType)
        {
            var handler = _fileServiceFactory.GetService(fileType);
            var allTVs = _tvService.GetAllTVs();
            var fileData = await handler.WriteDataAsync(allTVs);

            var (contentType, fileExtension) = fileType.ToLower() switch
            {
                "csv" => ("text/csv", "csv"),
                "excel" => ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx"),
                "json" => ("application/json", "json"),
                _ => ("application/octet-stream", "bin")
            };
        
            _logger.LogInformation($"TV data exported successfully to {fileType.ToUpper()} file");

            return File(fileData, contentType, $"TVs-{DateTime.UtcNow:yyyyMMdd}.{fileExtension}");
        }
    }
}