using Domain.Filters;
using Domain.SortableFields;

namespace ABPTestTask.Common.Filters
{
    public interface IEquipmentFilter : IFilterBase<EquipmentSortableFields>
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
