using Domain.Services.Contracts.Stage;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Stages.HrStage
{
    public class UpdateHrStageContractValidator : AbstractValidator<UpdateHrStageContract>
    {
        public UpdateHrStageContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_UPDATE, () =>
            {
                RuleFor(_ => _.ActualSalary).LessThan(ValidationConstants.MAX_MONTHLY_INCOME);
                RuleFor(_ => _.WantedSalary).LessThan(ValidationConstants.MAX_MONTHLY_INCOME);

                RuleFor(_ => _.AdditionalInformation).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.Feedback).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAX_TEXTAREA);

                RuleFor(_ => _.EnglishLevel).IsInEnum();
                RuleFor(_ => _.RejectionReasonsHr).IsInEnum();
                RuleFor(_ => _.Status).IsInEnum();
            });
        }
    }
}
