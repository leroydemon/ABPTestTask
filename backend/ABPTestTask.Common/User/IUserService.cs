using Domain.Filters;
using Domain.SortableFields;

namespace ABPTestTask.Common.User
{
    public interface IUserService
    {
        Task<IEnumerable<User>> SearchAsync(IFilterBase<UserSortableFields> filter);
        Task RemoveAsync(Guid userId);
        Task<User> GetByIdAsync(Guid userId);
        Task<User> UpdateAsync(User user);
    }
}
