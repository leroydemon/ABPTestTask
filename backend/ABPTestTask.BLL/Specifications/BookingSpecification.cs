using ABPTestTask.Common.Booking;
using ABPTestTask.DAL.Entities;
using Domain.Enum;
using Domain.Filters;
using Domain.SortableFields;
using System.Linq.Expressions;

namespace Domain.Specifications
{
    public class BookingSpecification : SpecificationBase<BookingEntity>
    {
        public BookingSpecification(BookingFilter filter, bool ablePaging = true)
        {
            // Initialize filter to a new instance if null
            filter ??= new BookingFilter();

            // Apply filters based on the provided filter properties
            if (filter.HallId.HasValue)
            {
                ApplyFilter(b => b.HallId == filter.HallId.Value);
            }

            if (filter.UserId.HasValue)
            {
                ApplyFilter(b => b.UserId == filter.UserId.Value);
            }

            // Ensure StartDateTime and EndDateTime are applied only if both are specified
            if (filter.StartDateTime.HasValue && filter.EndDateTime.HasValue)
            {
                ApplyFilter(b => b.StartDateTime >= filter.StartDateTime.Value && b.EndDateTime <= filter.EndDateTime.Value);
            }
            else if (filter.StartDateTime.HasValue) // Only StartDateTime is specified
            {
                ApplyFilter(b => b.StartDateTime >= filter.StartDateTime.Value);
            }
            else if (filter.EndDateTime.HasValue) // Only EndDateTime is specified
            {
                ApplyFilter(b => b.EndDateTime <= filter.EndDateTime.Value);
            }

            if (filter.IsConfirmed.HasValue)
            {
                ApplyFilter(b => b.IsConfirmed == filter.IsConfirmed.Value);
            }

            // Apply sorting and paging if specified
            ApplySorting(filter.OrderBy, filter.Ascending);
            if (ablePaging)
            {
                ApplyPaging(filter.Skip, filter.Take);
            }
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

