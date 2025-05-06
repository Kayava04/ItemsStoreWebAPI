using AutoMapper;
using ItemsStoreWebAPI.DTOs;
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
        private readonly ITVRequestValidator _tvRequestValidator;
        private readonly IFileService<TV> _tvFileService;
        private readonly IMapper _mapper;
        private readonly ILogger<TVController> _logger;

        public TVController(ITVService tvService, ITVRequestValidator tvRequestValidator, IFileService<TV> tvFileService, IMapper mapper, ILogger<TVController> logger)
        {
            _tvService = tvService;
            _tvRequestValidator = tvRequestValidator;
            _tvFileService = tvFileService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddTV([FromBody] RequestTvDto newTV)
        {
            if (!_tvRequestValidator.IsValid(newTV, out var errorMessage))
            {
                _logger.LogWarning($"Received invalid TV data: {errorMessage}");
                return BadRequest(errorMessage);
            }
            
            var tv = _mapper.Map<TV>(newTV);
            
            var createdTv = await _tvService.AddTV(tv);
            var responseTv = _mapper.Map<ResponseTvDto>(createdTv);
            
            _logger.LogInformation($"TV with ID: {responseTv.ID}, added successfully");
            return StatusCode(201, responseTv);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTVById(int id)
        {
            var tv = await _tvService.GetTVById(id);

            if (tv == null)
            {
                _logger.LogWarning($"TV with ID: {id} not found");
                return NotFound();
            }
            
            var responseTv = _mapper.Map<ResponseTvDto>(tv);
            
            _logger.LogInformation($"Received TV with ID: {id}");
            return Ok(responseTv);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetTVs([FromQuery] TvFilterDto? filter = null)
        {
            var tvs = await _tvService.GetTVs(filter);
            var responseTvs = tvs.Select(tv => _mapper.Map<ResponseTvDto>(tv));
            
            _logger.LogInformation($"Received all TVs. Total count: {responseTvs.Count()}");
            return Ok(responseTvs);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTV([FromBody] RequestTvDto updatedTV)
        {
            if (!_tvRequestValidator.IsValid(updatedTV, out var errorMessage))
            {
                _logger.LogWarning($"Received invalid update TV data: {errorMessage}");
                return BadRequest(errorMessage);
            }

            var tv = _mapper.Map<TV>(updatedTV);
            var updated = await _tvService.UpdateTV(tv.ID, tv);
            
            if (updated == null)
            {
                _logger.LogWarning($"Attempted to update non-existent TV with ID: {tv.ID}");
                return NotFound();
            }

            var responseTv = _mapper.Map<ResponseTvDto>(updated);
            
            _logger.LogInformation($"TV with ID: {responseTv.ID}, updated successfully");
            return Ok(responseTv);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTV(int id)
        {
            await _tvService.DeleteTV(id);
            
            _logger.LogInformation($"TV with ID: {id}, deleted successfully");
            return NoContent();
        }

        [HttpPost("import-file")]
        public async Task<IActionResult> ImportDataFromFile(IFormFile file)
        {
            var data = await _tvFileService.ImportFromFileAsync(file);
            
            _logger.LogInformation($"TV data imported successfully from {file.FileName.ToUpper()} file");
            return Ok(data);
        }

        [HttpPost("export-file")]
        public async Task<IActionResult> ExportDataToFile([FromQuery] string fileName)
        {
            var (data, contentType, downloadName) = await _tvFileService.ExportToFileAsync(fileName);
            
            _logger.LogInformation($"TV data exported successfully to {fileName.ToUpper()} file");
            return File(data, contentType, downloadName);
        }
    }
}