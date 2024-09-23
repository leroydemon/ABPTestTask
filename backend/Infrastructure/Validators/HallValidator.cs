using FluentValidation;
using Domain.Entities;

namespace Infrastructure.Validators
{
    public class HallValidator : AbstractValidator<Hall>
    {
        public HallValidator() 
        {
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);
        }
    }
}
