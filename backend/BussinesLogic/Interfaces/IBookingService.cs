using BussinesLogic.EntitiesDto;
using Domain.Filters;

namespace BussinesLogic.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> SearchAsync(BookingFilter filter);
        Task RemoveAsync(Guid Id);
        Task<BookingDto> GetByIdAsync(Guid Id);
        Task<BookingDto> UpdateAsync(BookingDto entityDto);
        Task<decimal> CalculatePrice(BookingDto entityDto);
        Task<decimal> BookingHall(BookingDto entityDto);
    }
}
