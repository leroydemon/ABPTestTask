using ABPTestTask.BBL.Requests;
using Domain.Filters;

namespace ABPTestTask.Common.Hall
{
    public interface IHallService
    {
        Task<IEnumerable<Hall>> SearchAsync(IFilterBase<Hall> filter);
        Task RemoveAsync(Guid Id);
        Task<Hall> GetByIdAsync(Guid Id);
        Task<Hall> UpdateAsync(Hall hallDto);
        Task<Hall> AddAsync(Hall hallDto);
        Task<IEnumerable<Hall>> SearchAvailableHallsAsync(HallAvailabilityRequest request);
    }
}
