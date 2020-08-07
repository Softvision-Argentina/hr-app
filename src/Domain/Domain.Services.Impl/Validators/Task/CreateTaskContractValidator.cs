// <copyright file="CreateTaskContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Task
{
    using Domain.Services.Contracts.Task;
    using FluentValidation;

    public class CreateTaskContractValidator : AbstractValidator<CreateTaskContract>
    {
        public CreateTaskContractValidator()
        {
            this.RuleFor(_ => _.Title).NotEmpty();
            this.RuleFor(_ => _.IsApprove).NotNull();
            this.RuleFor(_ => _.UserId).NotEmpty();
        }
    }
}
