using Domain.Services.Contracts;
using Domain.Services.Contracts.ReaddressReason;
using FluentValidation;

namespace Domain.Services.Impl.Validators
{
    public class UpdateReaddressReasonContractValidator : AbstractValidator<UpdateReaddressReason>
    {
        public UpdateReaddressReasonContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_UPDATE, () =>
            {
                RuleFor(_ => _.Description).MaximumLength(200).NotEmpty();
            });
        }
    }
}
