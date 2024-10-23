using ABPTestTask.BBL.Requests;
using ABPTestTask.Common.Hall;
using Domain.Filters;
using Domain.Specifications;

namespace ABPTestTask.BBL.Specifications
{
    public class AvailableHallsSpecification : SpecificationBase<HallEntity>
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
