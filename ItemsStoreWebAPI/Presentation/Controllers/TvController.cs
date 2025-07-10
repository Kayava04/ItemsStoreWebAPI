using AutoMapper;
using ItemsStoreWebAPI.Application.DTOs.TV;
using ItemsStoreWebAPI.Application.Models;
using ItemsStoreWebAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItemsStoreWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("v1/stock/electronic/tv")]
    public class TvController(
        IService<TV> tvService,
        IFileService<TV> tvFileService,
        IMapper mapper,
        ILogger<TvController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddTv([FromBody] RequestTvDto requestTv)
        {
            var tv = mapper.Map<TV>(requestTv);
            
            var createdTv = await tvService.AddAsync(tv);
            var responseTv = mapper.Map<ResponseTvDto>(createdTv);
            
            logger.LogInformation($"TV with ID: {responseTv.Id}, added successfully");
            return StatusCode(201, responseTv);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTvById(int id)
        {
            var tv = await tvService.GetByIdAsync(id);

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
            var tvs = await tvService.GetAllAsync(filter);
            var responseTVs = tvs.Select(tv => mapper.Map<ResponseTvDto>(tv));
            
            logger.LogInformation($"Received all TVs. Total count: {responseTVs.Count()}");
            return Ok(responseTVs);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTv([FromBody] RequestTvDto updatedTv)
        {
            var tv = mapper.Map<TV>(updatedTv);
            var updated = await tvService.UpdateAsync(tv.Id, tv);
            
            if (updated == null)
            {
                logger.LogWarning($"Attempted to update non-existent TV with ID: {tv.Id}");
                return NotFound();
            }

            var responseTv = mapper.Map<ResponseTvDto>(updated);
            
            logger.LogInformation($"TV with ID: {responseTv.Id}, updated successfully");
            return Ok(responseTv);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTv(int id)
        {
            await tvService.DeleteAsync(id);
            
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
        public async Task<IActionResult> AddMultiple([FromBody] IEnumerable<RequestTvDto> requestTVs)
        {
            var tvs = requestTVs.Select(mapper.Map<TV>);
            var addedTVs = await tvService.AddMultipleAsync(tvs);
            
            var responseTVs = addedTVs.Select(mapper.Map<RequestTvDto>);
            
            logger.LogInformation("Multiple TVs added successfully");
            return Ok(responseTVs);
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> UpdateMultiple([FromBody] IEnumerable<RequestTvDto> requestTVs)
        {
            var tvs = requestTVs.Select(mapper.Map<TV>);
            var updatedTVs = await tvService.UpdateMultipleAsync(tvs);
            
            var responseTVs = updatedTVs.Select(mapper.Map<RequestTvDto>);
            
            logger.LogInformation("Multiple TVs updated successfully");
            return Ok(responseTVs);
        }

        [HttpDelete("delete-multiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody] IEnumerable<RequestTvDto> requestTVs)
        {
            var tvs = requestTVs.Select(mapper.Map<TV>);
            await tvService.DeleteMultipleAsync(tvs);
            
            logger.LogInformation("Multiple TVs deleted successfully");
            return NoContent();
        }
    }
}