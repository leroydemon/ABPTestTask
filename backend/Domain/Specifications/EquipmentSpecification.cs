using Domain.Entities;
using Domain.Enum;
using Domain.Filters;
using Domain.SortableFields;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public class EquipmentSpecification : SpecificationBase<Equipment>
    {
        public EquipmentSpecification(EquipmentFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                ApplyFilter(u => u.Name.Contains(filter.Name));
            }

            if (filter.Price.HasValue)
            {
                ApplyFilter(p => p.Price == filter.Price.Value);
            }

            ApplySorting(filter.OrderBy, filter.Ascending);
            ApplyPaging(filter.Skip, filter.Take);
        }

        private void ApplySorting(EquipmentSortableFields sortBy, OrderByDirection ascending)
        {
            Expression<Func<Equipment, object>> orderByExpression = sortBy switch
            {
                EquipmentSortableFields.Name => u => u.Name,
                EquipmentSortableFields.Price => u => u.Price,
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
