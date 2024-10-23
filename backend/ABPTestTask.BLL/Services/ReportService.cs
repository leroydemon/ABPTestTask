using ABPTestTask.Common.Enum;
using ABPTestTask.Common.ExporterInterfaces;
using ABPTestTask.Common.Hall;
using ABPTestTask.Common.HallUsageReports;
using ABPTestTask.Common.Interfaces;
using ABPTestTask.DAL.Entities;
using Domain.Specifications;


namespace BussinesLogic.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<BookingEntity> _bookingRepository;
        private readonly IRepository<HallEntity> _hallRepository;
        private readonly IReportExporterFactory _exporterFactory;

        public ReportService(IRepository<BookingEntity> bookingRepository, IRepository<HallEntity> hallRepository, IReportExporterFactory exporterFactory)
        {
            _bookingRepository = bookingRepository;
            _hallRepository = hallRepository;
            _exporterFactory = exporterFactory;
        }

        public async Task<List<HallUsageReport>> GetHallUsageReport(Report reportRequest)
        {
            // Create specification for bookings based on the provided filter
            var bookingSpec = new BookingSpecification(reportRequest.BookingFilter, false);

            // Retrieve all bookings applying the specification
            var bookings = await _bookingRepository.ListAsync(bookingSpec);
            if (bookings == null || !bookings.Any())
            {
                return new List<HallUsageReport>(); // Return an empty report if no bookings are found
            }

            // Retrieve all halls
            var halls = await _hallRepository.GetAllAsync();
            if (halls == null || !halls.Any())
            {
                return new List<HallUsageReport>(); // Return an empty report if no halls are found
            }

            var report = new List<HallUsageReport>();

            // Generate report for halls based on filtered bookings
            foreach (var hall in halls)
            {
                // Get bookings for the current hall
                var hallBookings = bookings.Where(b => b.HallId == hall.Id).ToList();

                // Calculate total booking hours for the hall
                var totalBookingHours = hallBookings.Sum(b => (b.EndDateTime - b.StartDateTime).TotalHours);
                // Calculate total available hours based on the report request
                var totalAvailableHours = (reportRequest.EndDate - reportRequest.StartDate).TotalHours;

                // Calculate average booking duration for the hall
                var averageDuration = hallBookings.Any()
                    ? hallBookings.Average(b => (b.EndDateTime - b.StartDateTime).TotalHours)
                    : 0;

                // Get the top 3 popular hours based on bookings
                var popularHours = hallBookings
                    .Select(b => b.StartDateTime.Hour) // Take the hour from StartDateTime
                    .GroupBy(hour => hour)
                    .OrderByDescending(g => g.Count())
                    .Take(3) // Top 3 popular hours
                    .Select(g => g.Key)
                    .ToList();

                // Add hall usage report for the current hall
                report.Add(new HallUsageReport
                {
                    HallName = hall.Name,
                    OccupancyRate = totalAvailableHours > 0 ? totalBookingHours / totalAvailableHours * 100 : 0,
                    AverageBookingDuration = averageDuration,
                    PopularHours = popularHours
                });
            }

            return report; 
        }

        public async Task<string> ExportHallUsageReportToFile(Report reportRequest, ReportFormat format = ReportFormat.Csv)
        {
            var report = await GetHallUsageReport(reportRequest);
            string fileName = Path.Combine(Path.GetTempPath(), $"HallUsageReport_{reportRequest.StartDate:yyyyMMdd}_{reportRequest.EndDate:yyyyMMdd}.{format.ToString().ToLower()}");


            var exporter = _exporterFactory.CreateExporter(format);

            try
            {
                return await exporter.ExportAsync(report, fileName);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                throw new InvalidOperationException("Failed to export the hall usage report.", ex);
            }
        }
    }
}
