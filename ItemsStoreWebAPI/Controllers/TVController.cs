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
        private readonly ICsvService<TV> _csvService;
        private readonly ITVRequestValidator _tvRequestValidator;
        private readonly ILogger<TVController> _logger;

        public TVController(ITVService tvService, ICsvService<TV> csvService, ITVRequestValidator tvRequestValidator, ILogger<TVController> logger)
        {
            _tvService = tvService;
            _csvService = csvService;
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

        [HttpPost("import-csv")]
        public async Task<IActionResult> ImportFromCsv(IFormFile file)
        {
            if (file.Length == 0)
            {
                _logger.LogError("File is empty");
                return BadRequest();
            }
            
            await using var stream = file.OpenReadStream();
            var importedData = await _csvService.ImportFromCsvAsync(stream);

            foreach (var tv in importedData)
            {
                if (_tvRequestValidator.IsValid(tv, out _))
                    _tvService.AddTV(tv);
            }
            
            _logger.LogInformation("TV data imported successfully from CSV file");
            return Ok(importedData);
        }

        [HttpPost("export-csv")]
        public async Task<IActionResult> ExportToCsv()
        {
            var allTVs = _tvService.GetAllTVs();
            var csvData = await _csvService.ExportToCsvAsync(allTVs);
            
            _logger.LogInformation("TV data exported successfully to CSV file");
            return File(csvData, "text/csv", $"TVs-{DateTime.UtcNow:yyyyMMdd}.csv");
        }
    }
}