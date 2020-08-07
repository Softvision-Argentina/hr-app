// <copyright file="UpdateDummyContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Seed
{
    using Domain.Services.Contracts.Seed;
    using FluentValidation;

    public class UpdateDummyContractValidator : AbstractValidator<UpdateDummyContract>
    {
        public UpdateDummyContractValidator()
        {
            this.RuleFor(_ => _.TestValue).NotEmpty();
        }
    }
}
