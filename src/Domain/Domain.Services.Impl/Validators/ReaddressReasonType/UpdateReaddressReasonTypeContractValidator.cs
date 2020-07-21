using Domain.Services.Contracts;
using Domain.Services.Contracts.ReaddressReason;
using FluentValidation;

namespace Domain.Services.Impl.Validators
{
    public class UpdateReaddressReasonTypeContractValidator : AbstractValidator<UpdateReaddressReasonType>
    {
        public UpdateReaddressReasonTypeContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_UPDATE, () =>
            {
                RuleFor(_ => _.Name).MaximumLength(50).NotEmpty();
                RuleFor(_ => _.Description).MaximumLength(200);
            });
        }
    }
}
