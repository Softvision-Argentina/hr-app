// <copyright file="CreateReaddressReasonContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators
{
    using Domain.Services.Contracts.ReaddressReason;
    using FluentValidation;

    public class CreateReaddressReasonContractValidator : AbstractValidator<CreateReaddressReason>
    {
        public CreateReaddressReasonContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETCREATE, () =>
            {
                this.RuleFor(_ => _.Description)
                    .MaximumLength(200)
                    .NotEmpty()
                        .WithMessage("Reason needs a description defined");

                this.RuleFor(_ => _.TypeId)
                    .NotNull()
                    .WithMessage("Reason needs a type");
            });
        }
    }
}
