﻿using ABPTestTask.Common.Filters;
using Domain.SortableFields;

namespace Domain.Filters
{
    public class HallFilter : FilterBase<HallSortableFields>, IHallFilter
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Сapacity { get; set; }
    }
}
