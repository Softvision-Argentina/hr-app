// <copyright file="UpdateRoleContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Role
{
    using Domain.Services.Contracts.Role;
    using FluentValidation;

    public class UpdateRoleContractValidator : AbstractValidator<UpdateRoleContract>
    {
        public UpdateRoleContractValidator()
        {
            this.RuleFor(_ => _.Id).NotEmpty();
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.IsActive).NotNull();
        }
    }
}
