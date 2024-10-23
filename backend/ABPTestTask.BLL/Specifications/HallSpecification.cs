using ABPTestTask.Common.Hall;
using Domain.Enum;
using Domain.Filters;
using Domain.SortableFields;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public class HallSpecification : SpecificationBase<HallEntity>
    {
        public HallSpecification(HallFilter filter)
        {
            // Initialize filter to a new instance if null
            filter ??= new HallFilter();

            // Apply filtering based on the Name if provided
            if (!string.IsNullOrEmpty(filter.Name))
            {
                ApplyFilter(h => h.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)); // Case-insensitive search
            }

            // Apply filtering based on the Capacity if provided
            if (filter.Сapacity.HasValue)
            {
                ApplyFilter(h => h.Capacity == filter.Сapacity.Value);
            }

            // Apply filtering based on the Price if provided
            if (filter.Price.HasValue)
            {
                ApplyFilter(h => h.Price == filter.Price.Value);
            }

            // Apply sorting and pagination
            ApplySorting(filter.OrderBy, filter.Ascending);
            if (filter.Take > 0) // Ensure Take is a positive number
            {
                ApplyPaging(filter.Skip, filter.Take);
            }
        }

        private void ApplySorting(HallSortableFields sortBy, OrderByDirection ascending)
        {
            Expression<Func<HallEntity, object>> orderByExpression = sortBy switch
            {
                HallSortableFields.Name => h => h.Name,
                HallSortableFields.Capacity => h => h.Capacity,
                HallSortableFields.Price => h => h.Price,
                _ => h => h.Id
            };

            // Apply sorting based on the direction
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
