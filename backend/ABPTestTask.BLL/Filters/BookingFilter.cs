﻿using ABPTestTask.Common.Equipments;
using ABPTestTask.Common.Filters;
using Domain.SortableFields;

namespace Domain.Filters
{
    public class BookingFilter : FilterBase<BookingSortableFields>, IBookingFilter
    {
        public Guid? HallId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public List<Equipment>? Services { get; set; }
        public bool? IsConfirmed { get; set; }
    }
}
