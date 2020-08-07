// <copyright file="UpdateReaddressReasonContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators
{
    using Domain.Services.Contracts.ReaddressReason;
    using FluentValidation;

    public class UpdateReaddressReasonContractValidator : AbstractValidator<UpdateReaddressReason>
    {
        public UpdateReaddressReasonContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETUPDATE, () =>
            {
                this.RuleFor(_ => _.Description).MaximumLength(200).NotEmpty();
            });
        }
    }
}
