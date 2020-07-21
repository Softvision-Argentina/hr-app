using Domain.Services.Contracts.HireProjection;
using FluentValidation;

namespace Domain.Services.Impl.Validators.HireProjection
{
    public class UpdateHireProjectionContractValidator : AbstractValidator<UpdateHireProjectionContract>
    {
        public UpdateHireProjectionContractValidator()
        {
            RuleFor(_ => _.Month).NotEmpty();
            RuleFor(_ => _.Year).NotEmpty();
            RuleFor(_ => _.Value).NotEmpty();
        }
    }
}
