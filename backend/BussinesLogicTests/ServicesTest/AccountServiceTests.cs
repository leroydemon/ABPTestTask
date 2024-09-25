using Authorization.Interfaces;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Services;
using DbLevel.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BussinesLogicTests.ServicesTest
{
    public class AccountServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<ILogger<AccountService>> _loggerMock;
        private readonly Mock<ITokenGeneratorService> _tokenGeneratorMock;
        private readonly Mock<IRepository<User>> _userRepoMock;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
            _loggerMock = new Mock<ILogger<AccountService>>();
            _tokenGeneratorMock = new Mock<ITokenGeneratorService>();
            _userRepoMock = new Mock<IRepository<User>>();

            _accountService = new AccountService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _loggerMock.Object,
                _tokenGeneratorMock.Object,
                _userRepoMock.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnSuccess_WhenUserIsCreated()
        {
            // Arrange
            var registerDto = new RegisterDto { UserName = "testuser", Email = "test@test.com", Password = "Test@123" };
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountService.Register(registerDto);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("User registered successfully.");
        }

        [Fact]
        public async Task Register_ShouldReturnFailure_WhenUserCreationFails()
        {
            // Arrange
            var registerDto = new RegisterDto { UserName = "testuser", Email = "test@test.com", Password = "Test@123" };
            var identityErrors = new IdentityError[] { new IdentityError { Description = "Password too weak" } };
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            // Act
            var result = await _accountService.Register(registerDto);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("User registration failed: Password too weak");
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@test.com", Password = "Test@123" };
            var user = new User { UserName = "testuser", Email = "test@test.com" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(SignInResult.Success);
            _tokenGeneratorMock.Setup(tg => tg.GenerateJwtToken(It.IsAny<User>())).Returns("token");

            // Act
            var result = await _accountService.Login(loginDto);

            // Assert
            result.Should().Be("token");
            _userManagerMock.Verify(um => um.FindByEmailAsync(loginDto.Email), Times.Once);
            _signInManagerMock.Verify(sm => sm.PasswordSignInAsync(user.UserName, loginDto.Password, false, false), Times.Once);
        }

        [Fact]
        public async Task LogOut_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            _userRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);

            // Act
            var result = await _accountService.LogOut(userId);

            // Assert
            result.Should().BeTrue();
            _signInManagerMock.Verify(sm => sm.SignOutAsync(), Times.Once);
        }

        [Fact]
        public async Task LogOut_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);

            // Act
            var result = await _accountService.LogOut(userId);

            // Assert
            result.Should().BeFalse();
        }
    }
}
