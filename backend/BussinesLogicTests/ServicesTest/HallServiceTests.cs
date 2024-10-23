using ABPTestTask.Common.Hall;
using ABPTestTask.Common.Interfaces;
using AutoMapper;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BussinesLogicTests.ServicesTest
{
    public class HallServiceTests
    {
        private readonly HallService _service;
        private readonly Mock<IRepository<Hall>> _hallRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<HallService>> _loggerMock;

        public HallServiceTests()
        {
            _hallRepositoryMock = new Mock<IRepository<Hall>>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<HallService>>();
            _service = new HallService(_hallRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Hall_And_Return_Dto()
        {
            // Arrange
            var hallDto = new HallDto { Id = Guid.NewGuid(), Name = "Main Hall", Capacity = 100 };
            var hall = new Hall { Id = hallDto.Id, Name = hallDto.Name, Capacity = hallDto.Capacity };

            _mapperMock.Setup(m => m.Map<Hall>(hallDto)).Returns(hall);
            _hallRepositoryMock.Setup(r => r.AddAsync(hall)).ReturnsAsync(hall);
            _mapperMock.Setup(m => m.Map<HallDto>(hall)).Returns(hallDto);

            // Act
            var result = await _service.AddAsync(hallDto);

            // Assert
            result.Should().BeEquivalentTo(hallDto);
            _hallRepositoryMock.Verify(r => r.AddAsync(hall), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_HallDto_When_Hall_Exists()
        {
            // Arrange
            var hallId = Guid.NewGuid();
            var hall = new Hall { Id = hallId, Name = "Conference Room", Capacity = 50 };
            var hallDto = new HallDto { Id = hallId, Name = "Conference Room", Capacity = 50 };

            _hallRepositoryMock.Setup(r => r.GetByIdAsync(hallId)).ReturnsAsync(hall);
            _mapperMock.Setup(m => m.Map<HallDto>(hall)).Returns(hallDto);

            // Act
            var result = await _service.GetByIdAsync(hallId);

            // Assert
            result.Should().BeEquivalentTo(hallDto);
            _hallRepositoryMock.Verify(r => r.GetByIdAsync(hallId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Hall_And_Return_UpdatedDto()
        {
            // Arrange
            var hallDto = new HallDto { Id = Guid.NewGuid(), Name = "Updated Hall", Capacity = 120 };
            var existingHall = new Hall { Id = hallDto.Id, Name = "Old Hall", Capacity = 100 };

            _hallRepositoryMock.Setup(r => r.GetByIdAsync(hallDto.Id)).ReturnsAsync(existingHall);
            _hallRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Hall>())).ReturnsAsync(existingHall);
            _mapperMock.Setup(m => m.Map<Hall>(hallDto)).Returns(existingHall);
            _mapperMock.Setup(m => m.Map<HallDto>(existingHall)).Returns(hallDto);

            // Act
            var result = await _service.UpdateAsync(hallDto);

            // Assert
            result.Should().BeEquivalentTo(hallDto);
            _hallRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Hall>()), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_Should_Delete_Hall_When_Found()
        {
            // Arrange
            var hallId = Guid.NewGuid();
            var hall = new Hall { Id = hallId, Name = "Banquet Hall", Capacity = 150 };

            _hallRepositoryMock.Setup(r => r.GetByIdAsync(hallId)).ReturnsAsync(hall);
            _hallRepositoryMock.Setup(r => r.DeleteAsync(hall)).Returns(Task.CompletedTask);

            // Act
            await _service.RemoveAsync(hallId);

            // Assert
            _hallRepositoryMock.Verify(r => r.GetByIdAsync(hallId), Times.Once);
            _hallRepositoryMock.Verify(r => r.DeleteAsync(hall), Times.Once);
        }        
    }
}
