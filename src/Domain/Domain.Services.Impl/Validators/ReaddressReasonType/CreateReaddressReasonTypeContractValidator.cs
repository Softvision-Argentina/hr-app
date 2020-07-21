using Domain.Services.Contracts;
using Domain.Services.Contracts.ReaddressReason;
using FluentValidation;

namespace Domain.Services.Impl.Validators
{
    public class CreateReaddressReasonTypeContractValidator : AbstractValidator<CreateReaddressReasonType>
    {
        public CreateReaddressReasonTypeContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.Name).MaximumLength(50).NotEmpty();
                RuleFor(_ => _.Description).MaximumLength(200);
            });
        }
    }
}
