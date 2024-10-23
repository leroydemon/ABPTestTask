using ABPTestTask.Common.Equipment;
using ABPTestTask.Common.Filters;
using Domain.Enum;
using Domain.Filters;
using Domain.SortableFields;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public class EquipmentSpecification : SpecificationBase<EquipmentEntity>
    {
        public EquipmentSpecification(IEquipmentFilter filter)
        {
            // Initialize filter to a new instance if null
            filter ??= new EquipmentFilter();

            // Apply filtering based on the Name if provided
            if (!string.IsNullOrEmpty(filter.Name))
            {
                ApplyFilter(e => e.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)); // Case insensitive search
            }

            // Apply filtering based on the Price if provided
            if (filter.Price.HasValue)
            {
                ApplyFilter(e => e.Price == filter.Price.Value);
            }

            // Apply sorting and pagination
            ApplySorting(filter.OrderBy, filter.Ascending);
            if (filter.Take > 0) // Ensure Take is a positive number before applying paging
            {
                ApplyPaging(filter.Skip, filter.Take);
            }
        }

        private void ApplySorting(EquipmentSortableFields sortBy, OrderByDirection ascending)
        {
            Expression<Func<EquipmentEntity, object>> orderByExpression = sortBy switch
            {
                EquipmentSortableFields.Name => e => e.Name,
                EquipmentSortableFields.Price => e => e.Price,
                _ => e => e.Id
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
