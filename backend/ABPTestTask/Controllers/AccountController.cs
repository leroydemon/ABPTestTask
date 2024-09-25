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

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (request == null)
            {
                return BadRequest(new { Message = "Invalid registration request." }); // Return bad request if the request is null
            }

            var result = await _accountService.Register(request);

            if (result.Success)
            {
                return Ok(new { Message = result.Message }); // Return success message
            }

            return BadRequest(new { Message = result.Message }); // Return error message
        }

        // Log in an existing user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto input)
        {
            if (input == null)
            {
                return BadRequest(new { Message = "Invalid login request." }); // Return bad request if the input is null
            }

            _logger.LogInformation("Attempting to log in.");

            var token = await _accountService.Login(input);
            if (token != null)
            {
                _logger.LogInformation("User logged in successfully.");
                return Ok(new { Token = token }); // Return JWT token
            }

            _logger.LogWarning("Login failed. Invalid credentials.");
            return Unauthorized(new { Message = "Invalid credentials" }); // Return unauthorized message
        }

        // Log out the user
        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LogOut([FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest(new { Message = "Invalid user ID." }); // Return bad request if the user ID is empty
            }

            _logger.LogInformation("Attempting to log out user with ID {UserId}.", userId);

            var result = await _accountService.LogOut(userId);
            if (result)
            {
                _logger.LogInformation("User logged out successfully.");
                return NoContent(); // Return no content on successful logout
            }

            _logger.LogWarning("Logout failed for user with ID {UserId}.", userId);
            return BadRequest(new { Message = "Logout failed" }); // Return error message
        }
    }
}
