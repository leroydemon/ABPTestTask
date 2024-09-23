using Domain.Entities;
using Domain.Enum;
using Domain.Filters;
using Domain.SortableFields;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public class HallSpecification : SpecificationBase<Hall>
    {
        public HallSpecification(HallFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                ApplyFilter(u => u.Name.Contains(filter.Name));
            }

            if (filter.Сapacity.HasValue)
            {
                ApplyFilter(p => p.Capacity == filter.Сapacity.Value);
            }

            if (filter.Price.HasValue)
            {
                ApplyFilter(p => p.Price == filter.Price.Value);
            }

            ApplySorting(filter.OrderBy, filter.Ascending);
            ApplyPaging(filter.Skip, filter.Take);
        }

        private void ApplySorting(HallSortableFields sortBy, OrderByDirection ascending)
        {
            Expression<Func<Hall, object>> orderByExpression = sortBy switch
            {
                HallSortableFields.Name => u => u.Name,
                HallSortableFields.Capacity => u => u.Capacity,
                HallSortableFields.Price => u => u.Price,
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
