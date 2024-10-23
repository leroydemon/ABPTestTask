using ABPTestTask.Common.Interfaces;
using ABPTestTask.Common.User;
using AutoMapper;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Services;
using Domain.Filters;
using Domain.Specifications;
using FluentAssertions;
using Moq;
using Xunit;

namespace BussinesLogicTests.ServicesTest
{
    public class UserServiceTests
    {
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task SearchAsync_ValidFilter_ReturnsMappedUserDtos()
        {
            // Arrange
            var filter = new UserFilter();
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), UserName = "user1", Email = "user1@example.com" },
                new User { Id = Guid.NewGuid(), UserName = "user2", Email = "user2@example.com" }
            };
            var userDtos = new List<UserDto>
            {
                new UserDto { Id = users[0].Id, UserName = users[0].UserName, Email = users[0].Email },
                new UserDto { Id = users[1].Id, UserName = users[1].UserName, Email = users[1].Email }
            };

            _userRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<UserSpecification>())).ReturnsAsync(users);
            _mapperMock.Setup(mapper => mapper.Map<List<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = await _userService.SearchAsync(filter);

            // Assert
            result.Should().BeEquivalentTo(userDtos);
        }

        [Fact]
        public async Task RemoveAsync_ValidUserId_DeletesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            await _userService.RemoveAsync(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_InvalidUserId_ThrowsArgumentException()
        {
            // Arrange
            Guid userId = Guid.Empty;

            // Act
            Func<Task> act = async () => await _userService.RemoveAsync(userId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("User ID must be provided.*");
        }

        [Fact]
        public async Task GetByIdAsync_ValidUserId_ReturnsMappedUserDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, UserName = "user1", Email = "user1@example.com" };
            var userDto = new UserDto { Id = userId, UserName = user.UserName, Email = user.Email };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            result.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidUserId_ThrowsArgumentException()
        {
            // Arrange
            Guid userId = Guid.Empty;

            // Act
            Func<Task> act = async () => await _userService.GetByIdAsync(userId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("User ID must be provided.*");
        }

        [Fact]
        public async Task UpdateAsync_ValidUserDto_ReturnsUpdatedUserDto()
        {
            // Arrange
            var userDto = new UserDto { Id = Guid.NewGuid(), UserName = "updatedUser", Email = "updated@example.com" };
            var user = new User { Id = userDto.Id, UserName = userDto.UserName, Email = userDto.Email };
            var updatedUser = new User { Id = userDto.Id, UserName = "newUserName", Email = userDto.Email };
            var updatedUserDto = new UserDto { Id = updatedUser.Id, UserName = updatedUser.UserName, Email = updatedUser.Email };

            _mapperMock.Setup(mapper => mapper.Map<User>(userDto)).Returns(user);
            _userRepositoryMock.Setup(repo => repo.UpdateAsync(user)).ReturnsAsync(updatedUser);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(updatedUser)).Returns(updatedUserDto);

            // Act
            var result = await _userService.UpdateAsync(userDto);

            // Assert
            result.Should().BeEquivalentTo(updatedUserDto);
        }

        [Fact]
        public async Task AddAsync_ValidUserDto_ReturnsAddedUserDto()
        {
            // Arrange
            var userDto = new UserDto { Id = Guid.NewGuid(), UserName = "newUser", Email = "new@example.com" };
            var user = new User { Id = userDto.Id, UserName = userDto.UserName, Email = userDto.Email };
            var addedUser = new User { Id = userDto.Id, UserName = user.UserName, Email = user.Email };
            var addedUserDto = new UserDto { Id = addedUser.Id, UserName = addedUser.UserName, Email = addedUser.Email };

            _mapperMock.Setup(mapper => mapper.Map<User>(userDto)).Returns(user);
            _userRepositoryMock.Setup(repo => repo.AddAsync(user)).ReturnsAsync(addedUser);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(addedUser)).Returns(addedUserDto);

            // Act
            var result = await _userService.AddAsync(userDto);

            // Assert
            result.Should().BeEquivalentTo(addedUserDto);
        }

        [Fact]
        public async Task UpdateAsync_NullUserDto_ThrowsArgumentNullException()
        {
            // Arrange
            UserDto userDto = null;

            // Act
            Func<Task> act = async () => await _userService.UpdateAsync(userDto);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName("userDto");
        }

        [Fact]
        public async Task AddAsync_NullUserDto_ThrowsArgumentNullException()
        {
            // Arrange
            UserDto userDto = null;

            // Act
            Func<Task> act = async () => await _userService.AddAsync(userDto);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName("userDto");
        }
    }
}
