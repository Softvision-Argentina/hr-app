using Domain.Services.Contracts.Stage;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Stages.OfferStage
{
    public class UpdateOfferStageContractValidator : AbstractValidator<UpdateOfferStageContract>
    {
        public UpdateOfferStageContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_UPDATE, () =>
            {
                RuleFor(_ => _.Bonus).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.Feedback).MaximumLength(5000);
                RuleFor(_ => _.Notes).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAX_TEXTAREA);

                RuleFor(_ => _.HealthInsurance).IsInEnum();
                RuleFor(_ => _.Status).IsInEnum();
                RuleFor(_ => _.Seniority).IsInEnum();
            });
        }
    }
}
