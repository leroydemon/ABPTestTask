using ABPTestTask.Common.Filters;
using ABPTestTask.Common.User;
using Domain.Enum;
using Domain.Filters;
using Domain.SortableFields;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public class UserSpecification : SpecificationBase<UserEntity>
    {
        public UserSpecification(IUserFilter filter)
        {
            // Initialize filter to a new instance if null
            filter ??= new UserFilter();

            // Apply filters based on the UserName field
            if (!string.IsNullOrEmpty(filter.UserName))
            {
                ApplyFilter(u => u.UserName.Contains(filter.UserName));
            }

            // Apply filters based on the Email field
            if (!string.IsNullOrEmpty(filter.Email))
            {
                ApplyFilter(u => u.Email.Contains(filter.Email));
            }

            // Apply sorting and pagination
            ApplySorting(filter.OrderBy, filter.Ascending);
            ApplyPaging(filter.Skip, filter.Take);
        }

        // Applies sorting based on the specified criteria
        private void ApplySorting(UserSortableFields sortBy, OrderByDirection ascending)
        {
            // Determine the sorting expression based on the specified field
            Expression<Func<UserEntity, object>> orderByExpression = sortBy switch
            {
                UserSortableFields.UserName => u => u.UserName,
                UserSortableFields.Email => u => u.Email,
                _ => u => u.Id // Default to sorting by Id
            };

            // Apply the appropriate sorting order
            if (ascending == OrderByDirection.Ascending)
            {
                ApplyOrderBy(orderByExpression);
            }
            else
            {
                ApplyOrderByDescending(orderByExpression);
            }
        }
    }
}
