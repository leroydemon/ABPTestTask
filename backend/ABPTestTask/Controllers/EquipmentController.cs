
using ABPTestTask.Common.Equipments;
using AutoMapper;
using BussinesLogic.EntitiesDto;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;
        private readonly ILogger<EquipmentController> _logger;
        private readonly IMapper _mapper;

        public EquipmentController(IEquipmentService equipmentService, ILogger<EquipmentController> logger, IMapper mapper)
        {
            _equipmentService = equipmentService;
            _logger = logger;
            _mapper = mapper;
        }

        // Search for equipment based on filter criteria
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> SearchAsync([FromQuery] EquipmentFilter filter)
        {
            var entities = await _equipmentService.SearchAsync(filter);

            if (entities == null || !entities.Any())
            {
                return NotFound(new { Message = "No equipment found matching the specified criteria." });
            }

            return Ok(_mapper.Map<EquipmentDto>(entities));
        }

        // Remove equipment by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            try
            {
                // Call the service to remove the equipment
                await _equipmentService.RemoveAsync(id);

                // Return a confirmation of deletion
                return Ok(new { Message = "Equipment deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific errors related to deletion
                return NotFound(new { Message = ex.Message }); // Return 404 if the equipment was not found
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                _logger.LogError(ex, "An error occurred while deleting the equipment.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }

        // Get equipment details by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var entity = await _equipmentService.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound(new { Message = $"Equipment with ID {id} not found." });
            }

            return Ok(_mapper.Map<EquipmentDto>(entity));
        }

        // Update equipment details
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] EquipmentDto entityDto)
        {
            if (entityDto == null)
            {
                return BadRequest(new { Message = "Equipment data must be provided." });
            }

            var updatedEntity = await _equipmentService.UpdateAsync(_mapper.Map<Equipment>(entityDto));

            return Ok(_mapper.Map<EquipmentDto>(updatedEntity)); // Return updated equipment
        }

        // Add new equipment
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] EquipmentDto entityDto)
        {
            if (entityDto == null)
            {
                return BadRequest(new { Message = "Equipment data must be provided." });
            }

            var entity = await _equipmentService.AddAsync(_mapper.Map<Equipment>(entityDto));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = entity.Id }, _mapper.Map<EquipmentDto>(entity)); // 201 Created
        }
    }

}
