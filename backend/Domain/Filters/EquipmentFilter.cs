using Domain.SortableFields;

namespace Domain.Filters
{
    public class EquipmentFilter : FilterBase<EquipmentSortableFields>
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
