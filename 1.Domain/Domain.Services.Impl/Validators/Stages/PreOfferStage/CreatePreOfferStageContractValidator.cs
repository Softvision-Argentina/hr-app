using Domain.Services.Contracts.Stage;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Stages.PreOfferStage
{
    public class CreatePreOfferStageContractValidator : AbstractValidator<CreatePreOfferStageContract>
    {
        public CreatePreOfferStageContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.Bonus).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.Notes).MaximumLength(ValidationConstants.MAX_TEXTAREA);

                RuleFor(_ => _.DNI)
                    .GreaterThanOrEqualTo(0)
                    .LessThan(ValidationConstants.MAX_DNI);

                RuleFor(_ => _.HealthInsurance).IsInEnum();
                RuleFor(_ => _.Status).IsInEnum();
            });
        }
    }
}
