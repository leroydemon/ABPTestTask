using ABPTestTask.Common.Bookings;
using ABPTestTask.Common.Filters;
using Domain.Filters;
using Domain.SortableFields;

namespace ABPTestTask.Common.Bookings
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> SearchAsync(IBookingFilter filter);
        Task RemoveAsync(Guid Id);
        Task<Booking> GetByIdAsync(Guid Id);
        Task<Booking> UpdateAsync(Booking entityDto);
        Task<decimal> CalculatePrice(Booking entityDto);
        Task<decimal> BookingHall(Booking entityDto);
    }
}
