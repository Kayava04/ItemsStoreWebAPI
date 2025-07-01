using AutoMapper;
using ItemsStoreWebAPI.DTOs.Mobile;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ItemsStoreWebAPI.Controllers
{
    [ApiController]
    [Route("v1/stock/electronic/mobile")]
    public class MobileController(
        IServiceBase<Mobile> mobileService,
        IFileService<Mobile> mobileFileService,
        IMapper mapper,
        ILogger<MobileController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddMobile([FromBody] RequestMobileDto requestMobile)
        {
            var mobile = mapper.Map<Mobile>(requestMobile);
            
            var createdMobile = await mobileService.AddAsync(mobile);
            var responseMobile = mapper.Map<ResponseMobileDto>(createdMobile);
            
            logger.LogInformation($"Mobile with ID: {responseMobile.Id}, added successfully");
            return StatusCode(201, responseMobile);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMobileById(int id)
        {
            var mobile = await mobileService.GetByIdAsync(id);

            if (mobile == null)
            {
                logger.LogWarning($"Mobile with ID: {id} not found");
                return NotFound();
            }
            
            var responseMobile = mapper.Map<ResponseMobileDto>(mobile);
            
            logger.LogInformation($"Received Mobile with ID: {id}");
            return Ok(responseMobile);
        }
        
        [HttpGet("filter")]
        public async Task<IActionResult> GetMobiles([FromQuery] MobileFilterDto? filter = null)
        {
            var mobiles = await mobileService.GetAllAsync(filter);
            var responseMobiles = mobiles.Select(m => mapper.Map<ResponseMobileDto>(m));
            
            logger.LogInformation($"Received all Mobiles. Total count: {responseMobiles.Count()}");
            return Ok(responseMobiles);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateMobile([FromBody] RequestMobileDto updatedMobile)
        {
            var mobile = mapper.Map<Mobile>(updatedMobile);
            var updated = await mobileService.UpdateAsync(mobile.Id, mobile);
            
            if (updated == null)
            {
                logger.LogWarning($"Attempted to update non-existent Mobile with ID: {mobile.Id}");
                return NotFound();
            }

            var responseMobile = mapper.Map<ResponseMobileDto>(updated);
            
            logger.LogInformation($"Mobile with ID: {responseMobile.Id}, updated successfully");
            return Ok(responseMobile);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTv(int id)
        {
            await mobileService.DeleteAsync(id);
            
            logger.LogInformation($"Mobile with ID: {id}, deleted successfully");
            return NoContent();
        }
        
        [HttpPost("import-file")]
        public async Task<IActionResult> ImportDataFromFile(IFormFile file)
        {
            var data = await mobileFileService.ImportFromFileAsync(file);
            
            logger.LogInformation($"Mobile data imported successfully from {file.FileName.ToUpper()} file");
            return Ok(data);
        }

        [HttpPost("export-file")]
        public async Task<IActionResult> ExportDataToFile([FromQuery] string fileName)
        {
            var (data, contentType, downloadName) = await mobileFileService.ExportToFileAsync(fileName);
            
            logger.LogInformation($"Mobile data exported successfully to {fileName.ToUpper()} file");
            return File(data, contentType, downloadName);
        }
    }
}