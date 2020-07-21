using Domain.Services.Contracts.HireProjection;
using FluentValidation;

namespace Domain.Services.Impl.Validators.HireProjection
{
    public class CreateHireProjectionContractValidator : AbstractValidator<CreateHireProjectionContract>
    {
        public CreateHireProjectionContractValidator()
        {
            RuleFor(_ => _.Month).NotEmpty();
            RuleFor(_ => _.Year).NotEmpty();
            RuleFor(_ => _.Value).NotEmpty();
        }
    }
}
