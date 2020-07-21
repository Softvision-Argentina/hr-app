

using Domain.Services.Contracts.OpenPositions;
using FluentValidation;

namespace Domain.Services.Impl.Validators.OpenPosition
{
    public class UpdateOpenPositionContractValidator : AbstractValidator<UpdateOpenPositionContract>
    {
        public UpdateOpenPositionContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_UPDATE, () =>
            {
                RuleFor(_ => _.Title)
                    .NotEmpty()
                    .MaximumLength(50);

                RuleFor(_ => _.Community)
                   .NotEmpty();

                RuleFor(_ => _.Studio)
                .MaximumLength(40)
                .NotEmpty();

                RuleFor(_ => _.Seniority)
                .NotEmpty();
            });
        }
    }
}
