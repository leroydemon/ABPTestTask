using Domain.Filters;
using Domain.SortableFields;

namespace ABPTestTask.Common.Filters
{
    public interface IHallFilter : IFilterBase<HallSortableFields>
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Сapacity { get; set; }
    }
}
