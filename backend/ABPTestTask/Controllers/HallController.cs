using ABPTestTask.BBL.Requests;
using ABPTestTask.Common.Hall;
using AutoMapper;
using BussinesLogic.EntitiesDto;
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
        private readonly ILogger<HallController> _logger;
        private readonly IMapper _mapper;

        public HallController(IHallService hallService, ILogger<HallController> logger, IMapper mapper)
        {
            _hallService = hallService;
            _logger = logger;
            _mapper = mapper;
        }

        // Search for halls based on provided filter criteria
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<HallDto>>> SearchAsync([FromQuery] HallFilter filter)
        {
            var entities = await _hallService.SearchAsync(filter);

            return Ok(_mapper.Map<HallDto>(entities));
        }

        // Remove a hall by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            try
            {
                await _hallService.RemoveAsync(id);
                return Ok(new { Message = "Conference hall deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message }); // Return 404 if the hall was not found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the conference hall.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request." });
            }
        }

        // Get a hall by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var entity = await _hallService.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound(new { Message = "Hall not found." }); // Return 404 if the hall was not found
            }

            return Ok(_mapper.Map<Hall>(entity));
        }

        // Update hall information
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] HallDto entityDto)
        {
            var entity = _mapper.Map<Hall>(entityDto);

            await _hallService.UpdateAsync(entity);
            return Ok(new { Message = "Hall updated successfully." });
        }

        // Add a new hall
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] HallDto entityDto)
        {
            var entity = _mapper.Map<Hall>(entityDto);

            var entityAdded = await _hallService.AddAsync(entity);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = entity.Id }, _mapper.Map<Hall>(entityAdded)); // Return 201 Created with the resource's location
        }

        // Search for available halls based on specified criteria
        [HttpGet("list")]
        public async Task<IActionResult> SearchAvailableHallsAsync([FromQuery] IHallAvailabilityRequest request)
        {
            var entities = await _hallService.SearchAvailableHallsAsync(request);
            return Ok(entities);
        }
    }
}
