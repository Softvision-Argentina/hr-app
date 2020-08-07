// <copyright file="UpdateReaddressStatusValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.ReaddressStatus
{
    using Domain.Services.Contracts.ReaddressStatus;
    using FluentValidation;

    public class UpdateReaddressStatusValidator : AbstractValidator<UpdateReaddressStatus>
    {
        public UpdateReaddressStatusValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETUPDATE, () =>
            {
                this.RuleFor(_ => _.Feedback).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.FromStatus).IsInEnum();
                this.RuleFor(_ => _.ToStatus).IsInEnum();
            });
        }
    }
}
