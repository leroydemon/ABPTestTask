using Domain.Enum;

namespace Domain.Filters
{
    public class FilterBase<TOrderBy>
    {
        public TOrderBy OrderBy { get; set; }
        public OrderByDirection Ascending { get; set; } = OrderByDirection.Ascending;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}
