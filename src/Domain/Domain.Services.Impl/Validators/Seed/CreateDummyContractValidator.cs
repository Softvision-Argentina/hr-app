// <copyright file="CreateDummyContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Seed
{
    using Domain.Services.Contracts.Seed;
    using FluentValidation;

    public class CreateDummyContractValidator : AbstractValidator<CreateDummyContract>
    {
        public CreateDummyContractValidator()
        {
            this.RuleFor(_ => _.TestValue).NotEmpty();
        }
    }
}
