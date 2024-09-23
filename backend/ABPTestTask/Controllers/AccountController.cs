using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABPTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var result = await _accountService.Register(request);

            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto input)
        {
            _logger.LogInformation("Attempting to log in.");

            var token = await _accountService.Login(input);
            if (token != null)
            {
                _logger.LogInformation("User logged in successfully.");
                return Ok(new { Token = token });
            }

            _logger.LogWarning("Login failed. Invalid credentials.");
            return Unauthorized(new { Message = "Invalid credentials" });
        }

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LogOut([FromQuery] Guid userId)
        {
            _logger.LogInformation("Attempting to log out user with ID {UserId}.", userId);

            var result = await _accountService.LogOut(userId);
            if (result)
            {
                _logger.LogInformation("User logged out successfully.");
                return NoContent();
            }

            _logger.LogWarning("Logout failed for user with ID {UserId}.", userId);
            return BadRequest(new { Message = "Logout failed" });
        }
    }
}
