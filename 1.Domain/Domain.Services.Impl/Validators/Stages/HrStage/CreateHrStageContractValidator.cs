using Domain.Services.Contracts.Stage;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Stages.HrStage
{
    public class CreateHrStageContractValidator : AbstractValidator<CreateHrStageContract>
    {
        public CreateHrStageContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.ActualSalary).LessThan(ValidationConstants.MAX_MONTHLY_INCOME);
                RuleFor(_ => _.WantedSalary).LessThan(ValidationConstants.MAX_MONTHLY_INCOME);

                RuleFor(_ => _.AdditionalInformation).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.Feedback).MaximumLength(5000);
                RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAX_TEXTAREA);

                RuleFor(_ => _.EnglishLevel).IsInEnum();
                RuleFor(_ => _.RejectionReasonsHr).IsInEnum();
                RuleFor(_ => _.Status).IsInEnum();
            });
        }
    }
}
