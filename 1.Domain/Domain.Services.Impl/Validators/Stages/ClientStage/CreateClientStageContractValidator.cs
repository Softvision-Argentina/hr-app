using Domain.Services.Contracts.Stage;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Stages.ClientStage
{
    public class CreateClientStageContractValidator : AbstractValidator<CreateClientStageContract>
    {
        public CreateClientStageContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.DelegateName).MaximumLength(ValidationConstants.MAX_INPUT);
                RuleFor(_ => _.Feedback).MaximumLength(5000);
                RuleFor(_ => _.Interviewer).MaximumLength(ValidationConstants.MAX_INPUT);
                RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAX_TEXTAREA);

                RuleFor(_ => _.Status).IsInEnum();
            });
        }
    }
}
