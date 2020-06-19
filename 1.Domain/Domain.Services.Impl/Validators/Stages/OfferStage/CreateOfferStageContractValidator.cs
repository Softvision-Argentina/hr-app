using Domain.Services.Contracts.Stage;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Stages.OfferStage
{
    public class CreateOfferStageContractValidator : AbstractValidator<CreateOfferStageContract>
    {
        public CreateOfferStageContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.Bonus).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.Feedback).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.Notes).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAX_TEXTAREA);

                RuleFor(_ => _.HealthInsurance).IsInEnum();
                RuleFor(_ => _.Status).IsInEnum();
                RuleFor(_ => _.Seniority).IsInEnum();
            });
        }
    }
}
