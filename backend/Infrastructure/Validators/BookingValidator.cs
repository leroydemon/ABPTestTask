using FluentValidation;
using Domain.Entities;

namespace Infrastructure.Validators
{
    public class BookingValidator : AbstractValidator<Booking>
    {
        public BookingValidator()
        {
        }
    }
}