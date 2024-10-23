using ABPTestTask.Common.Equipment;
using ABPTestTask.Common.Interfaces;
using AutoMapper;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BussinesLogicTests.ServicesTest
{
    public class EquipmentServiceTests
    {
        private readonly EquipmentService _service;
        private readonly Mock<IRepository<Equipment>> _equipmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public EquipmentServiceTests()
        {
            _equipmentRepositoryMock = new Mock<IRepository<Equipment>>();
            _mapperMock = new Mock<IMapper>();
            _service = new EquipmentService(_equipmentRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Equipment_And_Return_Dto()
        {
            // Arrange
            var equipmentDto = new EquipmentDto { Id = Guid.NewGuid(), Name = "Projector", Price = 100m };
            var equipment = new Equipment { Id = equipmentDto.Id, Name = equipmentDto.Name, Price = equipmentDto.Price };

            _mapperMock.Setup(m => m.Map<Equipment>(equipmentDto)).Returns(equipment);
            _equipmentRepositoryMock.Setup(r => r.AddAsync(equipment)).ReturnsAsync(equipment);
            _mapperMock.Setup(m => m.Map<EquipmentDto>(equipment)).Returns(equipmentDto);

            // Act
            var result = await _service.AddAsync(equipmentDto);

            // Assert
            result.Should().BeEquivalentTo(equipmentDto);
            _equipmentRepositoryMock.Verify(r => r.AddAsync(equipment), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_EquipmentDto_When_Equipment_Exists()
        {
            // Arrange
            var equipmentId = Guid.NewGuid();
            var equipment = new Equipment { Id = equipmentId, Name = "Sound System", Price = 200m };
            var equipmentDto = new EquipmentDto { Id = equipmentId, Name = "Sound System", Price = 200m };

            _equipmentRepositoryMock.Setup(r => r.GetByIdAsync(equipmentId)).ReturnsAsync(equipment);
            _mapperMock.Setup(m => m.Map<EquipmentDto>(equipment)).Returns(equipmentDto);

            // Act
            var result = await _service.GetByIdAsync(equipmentId);

            // Assert
            result.Should().BeEquivalentTo(equipmentDto);
            _equipmentRepositoryMock.Verify(r => r.GetByIdAsync(equipmentId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Throw_Exception_When_Equipment_Not_Found()
        {
            // Arrange
            var equipmentId = Guid.NewGuid();

            _equipmentRepositoryMock.Setup(r => r.GetByIdAsync(equipmentId)).ReturnsAsync((Equipment)null);

            // Act
            Func<Task> act = async () => await _service.GetByIdAsync(equipmentId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Equipment not found.");
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Equipment_And_Return_UpdatedDto()
        {
            // Arrange
            var equipmentDto = new EquipmentDto { Id = Guid.NewGuid(), Name = "Microphone", Price = 50m };
            var equipment = new Equipment { Id = equipmentDto.Id, Name = equipmentDto.Name, Price = equipmentDto.Price };

            _mapperMock.Setup(m => m.Map<Equipment>(equipmentDto)).Returns(equipment);
            _equipmentRepositoryMock.Setup(r => r.UpdateAsync(equipment)).ReturnsAsync(equipment);
            _mapperMock.Setup(m => m.Map<EquipmentDto>(equipment)).Returns(equipmentDto);

            // Act
            var result = await _service.UpdateAsync(equipmentDto);

            // Assert
            result.Should().BeEquivalentTo(equipmentDto);
            _equipmentRepositoryMock.Verify(r => r.UpdateAsync(equipment), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_Should_Delete_Equipment_When_Found()
        {
            // Arrange
            var equipmentId = Guid.NewGuid();
            var equipment = new Equipment { Id = equipmentId, Name = "WiFi", Price = 25m };

            _equipmentRepositoryMock.Setup(r => r.GetByIdAsync(equipmentId)).ReturnsAsync(equipment);
            _equipmentRepositoryMock.Setup(r => r.DeleteAsync(equipment)).Returns(Task.CompletedTask);

            // Act
            await _service.RemoveAsync(equipmentId);

            // Assert
            _equipmentRepositoryMock.Verify(r => r.GetByIdAsync(equipmentId), Times.Once);
            _equipmentRepositoryMock.Verify(r => r.DeleteAsync(equipment), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_Should_Throw_Exception_When_Equipment_Not_Found()
        {
            // Arrange
            var equipmentId = Guid.NewGuid();

            _equipmentRepositoryMock.Setup(r => r.GetByIdAsync(equipmentId)).ReturnsAsync((Equipment)null);

            // Act
            Func<Task> act = async () => await _service.RemoveAsync(equipmentId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Equipment not found.");
        }
    }
}
