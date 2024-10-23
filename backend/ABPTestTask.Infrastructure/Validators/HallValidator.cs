using FluentValidation;
using ABPTestTask.Common.Hall;

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
