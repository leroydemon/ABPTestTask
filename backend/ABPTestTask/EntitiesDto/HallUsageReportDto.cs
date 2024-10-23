public class HallUsageReportDto
{
    public string? HallName { get; set; }
    public double OccupancyRate { get; set; }
    public double AverageBookingDuration { get; set; }
    public List<int> PopularHours { get; set; } = new List<int>();
}
