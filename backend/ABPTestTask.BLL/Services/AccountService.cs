using ABPTestTask.Common.Interfaces;
using ABPTestTask.Common.User;
using ABPTestTask.Results;
using Authorization.Interfaces;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BussinesLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountService> _logger;
        private readonly ITokenGeneratorService _tokenGenerator;
        private readonly IRepository<UserEntity> _userRepo;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountService> logger,
            ITokenGeneratorService tokenGenerator,
            IRepository<UserEntity> userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
            _userRepo = userRepository;
        }

        // Register a new user
        public async Task<ResultBase> Register(Register request)
        {
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            // Use UserManager to handle password hashing and user creation
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User registered successfully.");
                return new ResultBase
                {
                    Success = true,
                    Message = "User registered successfully."
                };
            }

            // If registration fails, gather all error descriptions
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("User registration failed: {Errors}", errors);

            return new ResultBase
            {
                Success = false,
                Message = $"User registration failed: {errors}"
            };
        }

        // Log in an existing user
        public async Task<string> Login(Login input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Login input cannot be null.");
            }

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                throw new Exception("Invalid login attempt."); // Consider using a more specific exception type
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, input.Password, false, false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid login attempt."); // Consider using a more specific exception type
            }

            // Generate JWT token for the authenticated user
            var token = _tokenGenerator.GenerateJwtToken(user);
            return token;
        }

        // Log out the user
        public async Task<bool> LogOut(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("Logout failed: Invalid user ID.");
                return false; // Early return if user ID is invalid
            }

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Logout failed: User not found.");
                return false;
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out successfully.");

            return true;
        }
    }
}
