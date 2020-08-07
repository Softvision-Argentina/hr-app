// <copyright file="CreateReaddressReasonTypeContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators
{
    using Domain.Services.Contracts.ReaddressReason;
    using FluentValidation;

    public class CreateReaddressReasonTypeContractValidator : AbstractValidator<CreateReaddressReasonType>
    {
        public CreateReaddressReasonTypeContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETCREATE, () =>
            {
                this.RuleFor(_ => _.Name).MaximumLength(50).NotEmpty();
                this.RuleFor(_ => _.Description).MaximumLength(200);
            });
        }
    }
}
