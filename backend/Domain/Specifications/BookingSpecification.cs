using Domain.Enum;
using Domain.Filters;
using Domain.SortableFields;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public class BookingSpecification : SpecificationBase<Booking>
    {
        public BookingSpecification(BookingFilter filter)
        {
            if (filter.HallId.HasValue)
            {
                ApplyFilter(b => b.HallId == filter.HallId.Value);
            }

            if (filter.UserId.HasValue)
            {
                ApplyFilter(b => b.UserId == filter.UserId.Value);
            }

            if (filter.StartDateTime.HasValue && filter.EndDateTime.HasValue)
            {
                ApplyFilter(b => b.StartDateTime >= filter.StartDateTime.Value && b.EndDateTime <= filter.EndDateTime.Value);
            }

            if (filter.IsConfirmed.HasValue)
            {
                ApplyFilter(b => b.IsConfirmed == filter.IsConfirmed.Value);
            }

            ApplySorting(filter.OrderBy, filter.Ascending);
            ApplyPaging(filter.Skip, filter.Take);
        }

        private void ApplySorting(BookingSortableFields sortBy, OrderByDirection ascending)
        {
            Expression<Func<Booking, object>> orderByExpression = sortBy switch
            {
                BookingSortableFields.UserId => b => b.UserId,
                BookingSortableFields.HallId => b => b.HallId,
                BookingSortableFields.StartDateTime => b => b.StartDateTime,
                BookingSortableFields.EndDateTime => b => b.EndDateTime,
                BookingSortableFields.IsConfirmed => b => b.IsConfirmed,
                _ => b => b.Id
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

