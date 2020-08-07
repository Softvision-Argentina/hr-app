// <copyright file="CreateRoleContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Role
{
    using Domain.Services.Contracts.Role;
    using FluentValidation;

    public class CreateRoleContractValidator : AbstractValidator<CreateRoleContract>
    {
        public CreateRoleContractValidator()
        {
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.IsActive).NotEmpty();
        }
    }
}
