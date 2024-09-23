using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using Domain.Entities;
using Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchAsync([FromQuery] UserFilter filter)
        {
            var users = await _userService.SearchAsync(filter);

            return Ok(users);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync(Guid userId)
        {
            await _userService.RemoveAsync(userId);

            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);

            return Ok(user);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UserDto userDto)
        {
            await _userService.UpdateAsync(userDto);

            return Ok();
        }
    }
}
