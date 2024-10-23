using ABPTestTask.Common.Equipments;
using Domain.Filters;
using Domain.SortableFields;

namespace ABPTestTask.Common.Filters
{
    public interface IBookingFilter : IFilterBase<BookingSortableFields>
    {
        public Guid? HallId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public List<Equipment>? Services { get; set; }
        public bool? IsConfirmed { get; set; }
    }
}
