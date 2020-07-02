using Domain.Services.Contracts;
using Domain.Services.Contracts.ReaddressReason;
using FluentValidation;

namespace Domain.Services.Impl.Validators
{
    public class CreateReaddressReasonContractValidator : AbstractValidator<CreateReaddressReason>
    {
        public CreateReaddressReasonContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.Description)
                    .MaximumLength(200)
                    .NotEmpty()
                        .WithMessage("Reason needs a description defined");
                
                RuleFor(_ => _.TypeId)
                    .NotNull()
                    .WithMessage("Reason needs a type");
            });
        }
    }
}
