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
    public class TVController(
        ITVService tvService,
        ITVRequestValidator tvRequestValidator,
        IFileService<TV> tvFileService,
        IDbTransactionsService<TV> tvDbTransactionsService,
        IMapper mapper,
        ILogger<TVController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddTV([FromBody] RequestTvDto newTV)
        {
            if (!tvRequestValidator.IsValid(newTV, out var errorMessage))
            {
                logger.LogWarning($"Received invalid TV data: {errorMessage}");
                return BadRequest(errorMessage);
            }
            
            var tv = mapper.Map<TV>(newTV);
            
            var createdTv = await tvService.AddTV(tv);
            var responseTv = mapper.Map<ResponseTvDto>(createdTv);
            
            logger.LogInformation($"TV with ID: {responseTv.ID}, added successfully");
            return StatusCode(201, responseTv);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTVById(int id)
        {
            var tv = await tvService.GetTVById(id);

            if (tv == null)
            {
                logger.LogWarning($"TV with ID: {id} not found");
                return NotFound();
            }
            
            var responseTv = mapper.Map<ResponseTvDto>(tv);
            
            logger.LogInformation($"Received TV with ID: {id}");
            return Ok(responseTv);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetTVs([FromQuery] TvFilterDto? filter = null)
        {
            var tvs = await tvService.GetTVs(filter);
            var responseTvs = tvs.Select(tv => mapper.Map<ResponseTvDto>(tv));
            
            logger.LogInformation($"Received all TVs. Total count: {responseTvs.Count()}");
            return Ok(responseTvs);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTV([FromBody] RequestTvDto updatedTV)
        {
            if (!tvRequestValidator.IsValid(updatedTV, out var errorMessage))
            {
                logger.LogWarning($"Received invalid update TV data: {errorMessage}");
                return BadRequest(errorMessage);
            }

            var tv = mapper.Map<TV>(updatedTV);
            var updated = await tvService.UpdateTV(tv.ID, tv);
            
            if (updated == null)
            {
                logger.LogWarning($"Attempted to update non-existent TV with ID: {tv.ID}");
                return NotFound();
            }

            var responseTv = mapper.Map<ResponseTvDto>(updated);
            
            logger.LogInformation($"TV with ID: {responseTv.ID}, updated successfully");
            return Ok(responseTv);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTV(int id)
        {
            await tvService.DeleteTV(id);
            
            logger.LogInformation($"TV with ID: {id}, deleted successfully");
            return NoContent();
        }

        [HttpPost("import-file")]
        public async Task<IActionResult> ImportDataFromFile(IFormFile file)
        {
            var data = await tvFileService.ImportFromFileAsync(file);
            
            logger.LogInformation($"TV data imported successfully from {file.FileName.ToUpper()} file");
            return Ok(data);
        }

        [HttpPost("export-file")]
        public async Task<IActionResult> ExportDataToFile([FromQuery] string fileName)
        {
            var (data, contentType, downloadName) = await tvFileService.ExportToFileAsync(fileName);
            
            logger.LogInformation($"TV data exported successfully to {fileName.ToUpper()} file");
            return File(data, contentType, downloadName);
        }
        
        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultiple([FromBody] IEnumerable<TV> tvs)
        {
            var addedTVs = await tvDbTransactionsService.AddMultipleAsync(tvs);
            
            logger.LogInformation("Multiple TVs added successfully");
            return Ok(addedTVs);
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] IEnumerable<TV> tvs)
        {
            var updatedTVs = await tvDbTransactionsService.UpdateMultipleAsync(tvs);
            
            logger.LogInformation("Multiple TVs updated successfully");
            return Ok(updatedTVs);
        }

        [HttpDelete("delete-multiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody] IEnumerable<TV> tvs)
        {
            await tvDbTransactionsService.DeleteMultipleAsync(tvs);
            
            logger.LogInformation("Multiple TVs deleted successfully");
            return NoContent();
        }
    }
}