using Domain.Services.Contracts.Employee;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Impl.Validators.Employee
{
    public class CreateEmployeeContractValidator : AbstractValidator<CreateEmployeeContract>
    {
        public CreateEmployeeContractValidator()
        {
            RuleFor(employee => employee.Name).NotEmpty();
            RuleFor(employee => employee.LastName).NotEmpty();
            RuleFor(employee => employee.EmailAddress).NotEmpty();
            RuleFor(employee => employee.DNI).NotEmpty();
            RuleFor(employee => employee.DNI).GreaterThan(0);
            RuleFor(employee => employee.PhoneNumber).NotEmpty();
            RuleFor(employee => employee.User).NotNull();
            RuleFor(employee => employee.isReviewer).NotNull();
            RuleFor(employee => employee.Role).NotNull();
        }
    }
}
