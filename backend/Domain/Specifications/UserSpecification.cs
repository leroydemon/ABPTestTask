using Domain.Entities;
using Domain.Enum;
using Domain.Filters;
using Domain.SortableFields;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public class UserSpecification : SpecificationBase<User>
    {
        public UserSpecification(UserFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.UserName))
            {
                ApplyFilter(u => u.UserName.Contains(filter.UserName));
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                ApplyFilter(u => u.Email.Contains(filter.Email));
            }

            ApplySorting(filter.OrderBy, filter.Ascending);
            ApplyPaging(filter.Skip, filter.Take);
        }

        private void ApplySorting(UserSortableFields sortBy, OrderByDirection ascending)
        {
            Expression<Func<User, object>> orderByExpression = sortBy switch
            {
                UserSortableFields.UserName => u => u.UserName,
                UserSortableFields.Email => u => u.Email,
                UserSortableFields.Created => u => u.Created,
                _ => u => u.Id
            };

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
