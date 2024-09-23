using BussinesLogic.EntitiesDto;
using Domain.Filters;

namespace BussinesLogic.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> SearchAsync(UserFilter filter);
        Task RemoveAsync(Guid userId);
        Task<UserDto> GetByIdAsync(Guid userId);
        Task<UserDto> UpdateAsync(UserDto user);
    }
}
