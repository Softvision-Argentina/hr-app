using Domain.Services.Contracts.DaysOff;
using FluentValidation;

namespace Domain.Services.Impl.Validators.DaysOff
{
    public class UpdateDaysOffContractValidator : AbstractValidator<UpdateDaysOffContract>
    {
        public UpdateDaysOffContractValidator()
        {
            RuleFor(_ => _.Type).NotNull().WithMessage("Type must not be empty");
            RuleFor(_ => _.Date).NotEmpty();
            RuleFor(_ => _.EndDate).NotEmpty();
        }
    }
}
