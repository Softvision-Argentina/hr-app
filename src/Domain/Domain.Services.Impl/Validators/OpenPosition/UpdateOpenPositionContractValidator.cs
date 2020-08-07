// <copyright file="UpdateOpenPositionContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.OpenPosition
{
    using Domain.Services.Contracts.OpenPositions;
    using FluentValidation;

    public class UpdateOpenPositionContractValidator : AbstractValidator<UpdateOpenPositionContract>
    {
        public UpdateOpenPositionContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETUPDATE, () =>
            {
                this.RuleFor(_ => _.Title)
                    .NotEmpty()
                    .MaximumLength(50);

                this.RuleFor(_ => _.Community)
                   .NotEmpty();

                this.RuleFor(_ => _.Studio)
                .MaximumLength(40)
                .NotEmpty();

                this.RuleFor(_ => _.Seniority)
                .NotEmpty();
            });
        }
    }
}
