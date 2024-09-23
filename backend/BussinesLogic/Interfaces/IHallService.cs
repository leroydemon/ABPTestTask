using BussinesLogic.EntitiesDto;
using Domain.Entities;
using Domain.Filters;

namespace BussinesLogic.Interfaces
{
    public interface IHallService
    {
        Task<IEnumerable<HallDto>> SearchAsync(HallFilter filter);
        Task RemoveAsync(Guid Id);
        Task<HallDto> GetByIdAsync(Guid Id);
        Task<HallDto> UpdateAsync(HallDto hallDto);
        Task<HallDto> AddAsync(HallDto hallDto);
        Task<IEnumerable<HallDto>> SearchAvailableHallsAsync(HallAvailabilityRequest request);
    }
}
