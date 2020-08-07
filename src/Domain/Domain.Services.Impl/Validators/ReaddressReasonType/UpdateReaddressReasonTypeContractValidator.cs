// <copyright file="UpdateReaddressReasonTypeContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators
{
    using Domain.Services.Contracts.ReaddressReason;
    using FluentValidation;

    public class UpdateReaddressReasonTypeContractValidator : AbstractValidator<UpdateReaddressReasonType>
    {
        public UpdateReaddressReasonTypeContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETUPDATE, () =>
            {
                this.RuleFor(_ => _.Name).MaximumLength(50).NotEmpty();
                this.RuleFor(_ => _.Description).MaximumLength(200);
            });
        }
    }
}
