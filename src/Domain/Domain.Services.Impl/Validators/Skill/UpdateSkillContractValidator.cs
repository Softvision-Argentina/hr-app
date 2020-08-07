// <copyright file="UpdateSkillContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Skill
{
    using Domain.Services.Contracts.Skill;
    using FluentValidation;

    public class UpdateSkillContractValidator : AbstractValidator<UpdateSkillContract>
    {
        public UpdateSkillContractValidator()
        {
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.Type).NotEmpty();
        }
    }
}
