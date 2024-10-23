using ABPTestTask.Common.User;
using AutoMapper;
using BussinesLogic.EntitiesDto;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ABPTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, ILogger<UserController> logger, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        // Search for users based on filter criteria
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchAsync([FromQuery] UserFilter filter)
        {
            try
            {
                var users = await _userService.SearchAsync(filter);
                return Ok(_mapper.Map<IEnumerable<User>>(users));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for users.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // Remove a user by their ID
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveAsync(Guid userId)
        {
            try
            {
                await _userService.RemoveAsync(userId);
                return Ok(new { Message = "User deleted successfully." });
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Attempted to delete a user that was not found: {UserId}", userId);
                return NotFound(new { Message = "User not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // Get user details by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);

                return Ok(_mapper.Map<UserDto>(user));
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("User not found for ID: {Id}", id);
                return NotFound(new { Message = "User not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // Update user details
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { Message = "User data is required." });
            }

            try
            {
                await _userService.UpdateAsync(_mapper.Map<User>(userDto));
                return Ok(new { Message = "User updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Attempted to update a user that was not found");
                return NotFound(new { Message = "User not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
