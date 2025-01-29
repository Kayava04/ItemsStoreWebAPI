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

        public TVController(ITVService tvService, ITVRequestValidator tvRequestValidator)
        {
            _tvService = tvService;
            _tvRequestValidator = tvRequestValidator;
        }
        
        //TODO: Add logs on level's controller & storage
        //      Create another TV storage for Dictionary

        [HttpPost]
        public IActionResult AddTV([FromBody] TV newTV)
        {
            if (!_tvRequestValidator.IsValid(newTV, out string errorMessage))
                return BadRequest(errorMessage);

            _tvService.AddTV(newTV);
            return StatusCode(201, newTV);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetTVById(int id)
        {
            var tv = _tvService.GetTVById(id);
            return Ok(tv);
        }

        [HttpGet]
        public IActionResult GetAllTVs()
        {
            var tvs = _tvService.GetAllTVs();
            return Ok(tvs);
        }

        [HttpPut]
        public IActionResult UpdateTV([FromBody] TV updatedTV)
        {
            if (!_tvRequestValidator.IsValid(updatedTV, out string errorMessage))
                return BadRequest(errorMessage);

            var tv = _tvService.UpdateTV(updatedTV.ID, updatedTV);
            return Ok(tv);
        }

        [HttpDelete]
        public IActionResult DeleteTV(int id)
        {
            _tvService.DeleteTV(id);
            return NoContent();
        }
    }
}