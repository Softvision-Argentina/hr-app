// <copyright file="UpdateEmployeeCasualtyContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.EmployeeCasualty
{
    using Domain.Services.Contracts.EmployeeCasualty;
    using FluentValidation;

    public class UpdateEmployeeCasualtyContractValidator : AbstractValidator<UpdateEmployeeCasualtyContract>
    {
        public UpdateEmployeeCasualtyContractValidator()
        {
            this.RuleFor(_ => _.Month).NotEmpty();
            this.RuleFor(_ => _.Year).NotEmpty();
            this.RuleFor(_ => _.Value).NotEmpty();
        }
    }
}
