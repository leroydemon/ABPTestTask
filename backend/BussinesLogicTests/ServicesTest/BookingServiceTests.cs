using AutoMapper;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Services;
using DbLevel.Interfaces;
using Domain.Entities;
using FakeItEasy;
using FluentAssertions;
using Moq;
using Xunit;

namespace BussinesLogicTests.ServicesTest
{
    public class BookingServiceTests
    {
        private readonly Mock<IRepository<Booking>> _mockBookingRepository;
        private readonly Mock<IRepository<Hall>> _mockHallRepository;
        private readonly IMapper _mapper;
        private readonly BookingService _bookingService;

        public BookingServiceTests()
        {
            _mockBookingRepository = new Mock<IRepository<Booking>>();
            _mockHallRepository = new Mock<IRepository<Hall>>();
            _mapper = A.Fake<IMapper>();

            _bookingService = new BookingService(_mockBookingRepository.Object, _mapper, _mockHallRepository.Object);
        }

        [Fact]
        public async Task CalculatePrice_ShouldThrowArgumentNullException_WhenEntityDtoIsNull()
        {
            // Act
            Func<Task> act = async () => await _bookingService.CalculatePrice(null);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'entityDto')");
        }

        [Fact]
        public async Task CalculatePrice_ShouldThrowInvalidOperationException_WhenHallNotFound()
        {
            // Arrange
            var bookingDto = new BookingDto { HallId = Guid.NewGuid(), StartDateTime = DateTime.Now, EndDateTime = DateTime.Now.AddHours(2) };

            _mockHallRepository.Setup(repo => repo.GetByIdAsync(bookingDto.HallId)).ReturnsAsync((Hall)null);

            // Act
            Func<Task> act = async () => await _bookingService.CalculatePrice(bookingDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Hall not found.");
        }

        [Fact]
        public async Task CalculatePrice_ShouldReturnCorrectPrice_BasedOnTimeOfDay()
        {
            // Arrange
            var hall = new Hall { Price = 100 };
            var bookingDto = new BookingDto
            {
                HallId = Guid.NewGuid(),
                StartDateTime = DateTime.Today.AddHours(18),
                EndDateTime = DateTime.Today.AddHours(20)
            };

            _mockHallRepository.Setup(repo => repo.GetByIdAsync(bookingDto.HallId)).ReturnsAsync(hall);

            // Act
            var result = await _bookingService.CalculatePrice(bookingDto);

            // Assert
            result.Should().Be(160); // 100 * 0.8 * 2 hours
        }

        [Fact]
        public async Task CalculatePrice_ShouldIncludeServicePrices()
        {
            // Arrange
            var hall = new Hall { Price = 100 };
            var bookingDto = new BookingDto
            {
                HallId = Guid.NewGuid(),
                StartDateTime = DateTime.Today.AddHours(10),
                EndDateTime = DateTime.Today.AddHours(12),
                Services = new List<EquipmentDto>
                {
                     new EquipmentDto { Price = 50 },
                     new EquipmentDto { Price = 75 }
                 }
            };

            _mockHallRepository.Setup(repo => repo.GetByIdAsync(bookingDto.HallId)).ReturnsAsync(hall);

            // Act
            var result = await _bookingService.CalculatePrice(bookingDto);

            // Assert
            result.Should().Be(325); // 100 * 2 hours + 50 + 75
        }

        [Fact]
        public async Task BookingHall_ShouldThrowArgumentNullException_WhenEntityDtoIsNull()
        {
            // Act
            Func<Task> act = async () => await _bookingService.BookingHall(null);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'entityDto')");
        }

        [Fact]
        public async Task BookingHall_ShouldCallAddAsync_OnBookingRepository()
        {
            // Arrange
            var bookingDto = new BookingDto
            {
                HallId = Guid.NewGuid(),
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddHours(2),
            };

            var booking = new Booking();

            var hall = new Hall
            {
                Id = bookingDto.HallId,
                Price = 200 // Base price per hour
            };

            A.CallTo(() => _mapper.Map<Booking>(bookingDto)).Returns(booking);
            _mockHallRepository.Setup(repo => repo.GetByIdAsync(bookingDto.HallId)).ReturnsAsync(hall);

            // Act
            await _bookingService.BookingHall(bookingDto);

            // Assert
            _mockBookingRepository.Verify(repo => repo.AddAsync(It.IsAny<Booking>()),Moq.Times.Once());
        }

        [Fact]
        public async Task BookingHall_ShouldReturnCalculatedPrice()
        {
            // Arrange
            var bookingDto = new BookingDto
            {
                HallId = Guid.NewGuid(),
                StartDateTime = DateTime.Now.AddHours(17), // Starting at 17:00 to apply discounts/surcharges
                EndDateTime = DateTime.Now.AddHours(19) // Ending at 19:00, which gives 2 hours
            };

            // Mocking hall repository to return a hall with a price of 100
            _mockHallRepository.Setup(repo => repo.GetByIdAsync(bookingDto.HallId)).ReturnsAsync(new Hall { Price = 100 });

            // Act
            var totalPrice = await _bookingService.BookingHall(bookingDto);

            // Assert
            totalPrice.Should().BeApproximately(2300, 0.01m); // 100 * 2 hours + 20% markup for evening hours
        }
    }
}
