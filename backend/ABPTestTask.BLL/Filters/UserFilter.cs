using Domain.SortableFields;

namespace Domain.Filters
{
    public class UserFilter : FilterBase<UserSortableFields>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
