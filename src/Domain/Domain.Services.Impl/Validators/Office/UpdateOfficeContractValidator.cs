// <copyright file="UpdateOfficeContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Office
{
    using Domain.Services.Contracts.Office;
    using FluentValidation;

    public class UpdateOfficeContractValidator : AbstractValidator<UpdateOfficeContract>
    {
        public UpdateOfficeContractValidator()
        {
            this.RuleFor(_ => _.Id).NotEmpty();
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
