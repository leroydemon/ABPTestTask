using ABPTestTask.Common.Filters;

public class Report
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IBookingFilter? BookingFilter { get; set; }
}
