using Domain.Entities;
using Domain.Filters;

namespace Domain.Specifications
{
    public class AvailableHallsSpecification : SpecificationBase<Hall>
    {
        public AvailableHallsSpecification(HallAvailabilityRequest request)
        {
            // Filter halls based on the requested capacity
            ApplyFilter(h => h.Capacity >= request.Capacity);

            // Filter out halls that have bookings overlapping with the requested time
            ApplyFilter(h => !h.Bookings.Any(b =>
                b.StartDateTime < request.EndDateTime && b.EndDateTime > request.StartDateTime
                ));
        }
    }
}
