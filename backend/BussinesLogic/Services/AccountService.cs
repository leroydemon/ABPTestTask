using Authorization.Interfaces;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using BussinesLogic.Results;
using DbLevel.Interfaces;
using Domain.Entities;
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
        private readonly IRepository<User> _userRepo;
        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountService> logger,
            ITokenGeneratorService tokenGenerator,
            IRepository<User> userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
            _userRepo = userRepository;
        }
        public async Task<ResultBase> Register(RegisterDto request)
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
        public async Task<string> Login(LoginDto input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                throw new Exception("Invalid login attempt.");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, input.Password, false, false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid login attempt.");
            }

            var token = _tokenGenerator.GenerateJwtToken(user);

            return token;
        }
        public async Task<bool> LogOut(Guid userId)
        {
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
