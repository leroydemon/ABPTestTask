using Domain.Enum;

namespace Domain.Filters
{
    public interface IFilterBase<TOrderBy>
    {
        public TOrderBy OrderBy { get; set; }
        public OrderByDirection Ascending { get; set; }
        public int Skip { get; set; } 
        public int Take { get; set; } 
    }
}
