using Domain.Filters;

public class ReportRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BookingFilter? BookingFilter { get; set; }
}
