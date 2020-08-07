// <copyright file="CreateOfficeContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Office
{
    using Domain.Services.Contracts.Office;
    using FluentValidation;

    public class CreateOfficeContractValidator : AbstractValidator<CreateOfficeContract>
    {
        public CreateOfficeContractValidator()
        {
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
