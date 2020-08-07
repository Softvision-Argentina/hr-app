// <copyright file="CreateEmployeeContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Employee
{
    using Domain.Services.Contracts.Employee;
    using FluentValidation;

    public class CreateEmployeeContractValidator : AbstractValidator<CreateEmployeeContract>
    {
        public CreateEmployeeContractValidator()
        {
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.LastName).NotEmpty();
            this.RuleFor(_ => _.EmailAddress).NotEmpty();
            this.RuleFor(_ => _.DNI).NotEmpty();
            this.RuleFor(_ => _.DNI).GreaterThan(0);
            this.RuleFor(_ => _.PhoneNumber).NotEmpty();
            this.RuleFor(_ => _.User).NotNull();
            this.RuleFor(_ => _.IsReviewer).NotNull();
            this.RuleFor(_ => _.Role).NotNull();
        }
    }
}
