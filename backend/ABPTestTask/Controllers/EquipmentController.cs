using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using Domain.Entities;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Equipment>>> SearchAsync([FromQuery] EquipmentFilter filter)
        {
            var entities = await _equipmentService.SearchAsync(filter);

            return Ok(entities);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync(Guid Id)
        {
            await _equipmentService.RemoveAsync(Id);

            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var entity = await _equipmentService.GetByIdAsync(id);

            return Ok(entity);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(EquipmentDto entityDto)
        {
            await _equipmentService.UpdateAsync(entityDto);

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] EquipmentDto entityDto)
        {
            var entity = await _equipmentService.AddAsync(entityDto);

            return Ok(entity);
        }
    }
}
