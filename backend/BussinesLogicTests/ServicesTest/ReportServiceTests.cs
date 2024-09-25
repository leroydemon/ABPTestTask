using BussinesLogic.Enums;
using BussinesLogic.Interfaces;
using BussinesLogic.Services;
using DbLevel.Interfaces;
using Domain.Entities;
using Domain.Specifications;
using FluentAssertions;
using Moq;
using Xunit;

namespace BussinesLogicTests.ServicesTest
{
    public class ReportServiceTests
    {
        private readonly Mock<IRepository<Booking>> _bookingRepositoryMock;
        private readonly Mock<IRepository<Hall>> _hallRepositoryMock;
        private readonly Mock<IReportExporterFactory> _exporterFactoryMock;
        private readonly ReportService _reportService;

        public ReportServiceTests()
        {
            _bookingRepositoryMock = new Mock<IRepository<Booking>>();
            _hallRepositoryMock = new Mock<IRepository<Hall>>();
            _exporterFactoryMock = new Mock<IReportExporterFactory>();
            _reportService = new ReportService(_bookingRepositoryMock.Object, _hallRepositoryMock.Object, _exporterFactoryMock.Object);
        }

        [Fact]
        public async Task GetHallUsageReport_NoBookings_ReturnsEmptyReport()
        {
            // Arrange
            var reportRequest = new ReportRequest { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            _bookingRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<BookingSpecification>())).ReturnsAsync(new List<Booking>());
            _hallRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Hall> { new Hall { Id = Guid.NewGuid(), Name = "Hall 1" } });

            // Act
            var result = await _reportService.GetHallUsageReport(reportRequest);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetHallUsageReport_NoHalls_ReturnsEmptyReport()
        {
            // Arrange
            var reportRequest = new ReportRequest { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            _bookingRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<BookingSpecification>())).ReturnsAsync(new List<Booking> { new Booking { HallId = Guid.NewGuid(), StartDateTime = DateTime.Now, EndDateTime = DateTime.Now.AddHours(1) } });
            _hallRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Hall>());

            // Act
            var result = await _reportService.GetHallUsageReport(reportRequest);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetHallUsageReport_WithBookings_ReturnsCorrectReport()
        {
            // Arrange
            var hallId = Guid.NewGuid(); 
            var reportRequest = new ReportRequest { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };

            var bookings = new List<Booking>
            {
                new Booking { HallId = hallId, StartDateTime = DateTime.Now.AddHours(1), EndDateTime = DateTime.Now.AddHours(2) }, 
                new Booking { HallId = hallId, StartDateTime = DateTime.Now.AddHours(3), EndDateTime = DateTime.Now.AddHours(5) }  
            };

                var halls = new List<Hall>
                {
                    new Hall { Id = hallId, Name = "Hall 1" } 
                };

            _bookingRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<BookingSpecification>())).ReturnsAsync(bookings);
            _hallRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(halls);

            // Act
            var result = await _reportService.GetHallUsageReport(reportRequest);

            // Assert
            result.Should().HaveCount(1); 
            var hallReport = result.First();
            hallReport.HallName.Should().Be("Hall 1");

            hallReport.AverageBookingDuration.Should().BeApproximately(1.5, 0.1);
        }

        [Fact]
        public async Task ExportHallUsageReportToFile_ExportFails_ThrowsInvalidOperationException()
        {
            // Arrange
            var reportRequest = new ReportRequest { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            var report = new List<HallUsageReport>();

            _bookingRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<BookingSpecification>())).ReturnsAsync(new List<Booking>());
            _hallRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Hall> { new Hall { Id = Guid.NewGuid(), Name = "Hall 1" } });

            var exporterMock = new Mock<IReportExporter>();
            exporterMock.Setup(exp => exp.ExportAsync(report, It.IsAny<string>())).ThrowsAsync(new Exception("Export failed"));
            _exporterFactoryMock.Setup(factory => factory.CreateExporter(It.IsAny<ReportFormat>())).Returns(exporterMock.Object);

            // Act
            Func<Task> act = async () => await _reportService.ExportHallUsageReportToFile(reportRequest);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Failed to export the hall usage report.*")
                .Where(ex => ex.InnerException.Message == "Export failed");
        }
    }
}
