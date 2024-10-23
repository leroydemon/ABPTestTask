using ABPTestTask.Common.Filters;
using Domain.SortableFields;

namespace Domain.Filters
{
    public class UserFilter : FilterBase<UserSortableFields>, IUserFilter
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
