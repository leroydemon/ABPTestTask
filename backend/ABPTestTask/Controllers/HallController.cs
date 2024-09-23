using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using Domain.Entities;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallController : ControllerBase
    {
        private readonly IHallService _hallService;

        public HallController(IHallService hallService)
        {
            _hallService = hallService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Hall>>> SearchAsync([FromQuery] HallFilter filter)
        {
            var entities = await _hallService.SearchAsync(filter);

            return Ok(entities);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync(Guid Id)
        {
            await _hallService.RemoveAsync(Id);

            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var entity = await _hallService.GetByIdAsync(id);

            return Ok(entity);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(HallDto entityDto)
        {
            await _hallService.UpdateAsync(entityDto);

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] HallDto entityDto)
        {
            var entity = await _hallService.AddAsync(entityDto);

            return Ok(entity);
        }
        [HttpGet("list")]
        public async Task<IActionResult> SearchAvailableHallsAsync([FromQuery] HallAvailabilityRequest request)
        {
            var entities = await _hallService.SearchAvailableHallsAsync(request);

            return Ok(entities);
        }
    }
}
