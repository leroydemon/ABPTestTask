using Domain.Filters;
using Domain.SortableFields;

public class Report
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IFilterBase<BookingSortableFields>? BookingFilter { get; set; }
}
