using Domain.Filters;
using Domain.SortableFields;

namespace ABPTestTask.Common.Filters
{
    public interface IUserFilter : IFilterBase<UserSortableFields>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
