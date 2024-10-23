using Domain.Filters;

public class ReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BookingFilter? BookingFilter { get; set; }
}
