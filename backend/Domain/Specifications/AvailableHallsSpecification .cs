using Domain.Entities;

namespace Domain.Specifications
{
    public class AvailableHallsSpecification : SpecificationBase<Hall>
    {
        public AvailableHallsSpecification(HallAvailabilityRequest request)
        {
            ApplyFilter(h => h.Capacity >= request.Capacity);

            ApplyFilter(h => !h.Bookings.Any(b =>
                (b.StartDateTime < request.EndDateTime && b.EndDateTime > request.StartDateTime) 
            ));
        }
    }
}
