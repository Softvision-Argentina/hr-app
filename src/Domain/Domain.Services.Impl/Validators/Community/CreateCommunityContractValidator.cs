// <copyright file="CreateCommunityContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Community
{
    using Domain.Services.Contracts.Community;
    using FluentValidation;

    public class CreateCommunityContractValidator : AbstractValidator<CreateCommunityContract>
    {
        public CreateCommunityContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETCREATE, () =>
            {
                this.RuleFor(_ => _.Name).NotEmpty();
                this.RuleFor(_ => _.Description).NotNull();
            });
        }
    }
}
