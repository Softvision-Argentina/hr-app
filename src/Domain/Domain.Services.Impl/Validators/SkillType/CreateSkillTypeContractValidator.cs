// <copyright file="CreateSkillTypeContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.SkillType
{
    using Domain.Services.Contracts.SkillType;
    using FluentValidation;

    public class CreateSkillTypeContractValidator : AbstractValidator<CreateSkillTypeContract>
    {
        public CreateSkillTypeContractValidator()
        {
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
