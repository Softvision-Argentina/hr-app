using Domain.Services.Contracts.Stage;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Stages.TechnicalStage
{
    public class CreateTechnicalStageContractValidator : AbstractValidator<CreateTechnicalStageContract>
    {
        public CreateTechnicalStageContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.AlternativeSeniority).IsInEnum();
                RuleFor(_ => _.EnglishLevel).IsInEnum();
                RuleFor(_ => _.Seniority).IsInEnum();
                RuleFor(_ => _.Status).IsInEnum();

                RuleFor(_ => _.Client).MaximumLength(ValidationConstants.MAX_INPUT);
                RuleFor(_ => _.Feedback).MaximumLength(5000);
                RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAX_TEXTAREA);
            });
        }
    }
}
