// <copyright file="CreateHireProjectionContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.HireProjection
{
    using Domain.Services.Contracts.HireProjection;
    using FluentValidation;

    public class CreateHireProjectionContractValidator : AbstractValidator<CreateHireProjectionContract>
    {
        public CreateHireProjectionContractValidator()
        {
            this.RuleFor(_ => _.Month).NotEmpty();
            this.RuleFor(_ => _.Year).NotEmpty();
            this.RuleFor(_ => _.Value).NotEmpty();
        }
    }
}
