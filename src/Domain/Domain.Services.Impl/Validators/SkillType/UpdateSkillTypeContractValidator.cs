// <copyright file="UpdateSkillTypeContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.SkillType
{
    using Domain.Services.Contracts.SkillType;
    using FluentValidation;

    public class UpdateSkillTypeContractValidator : AbstractValidator<UpdateSkillTypeContract>
    {
        public UpdateSkillTypeContractValidator()
        {
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
