using ABPTestTask.Common.Filters;
using Domain.SortableFields;

namespace Domain.Filters
{
    public class EquipmentFilter : FilterBase<EquipmentSortableFields>, IEquipmentFilter
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
