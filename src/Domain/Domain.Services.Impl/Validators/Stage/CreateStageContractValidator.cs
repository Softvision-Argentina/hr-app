// <copyright file="CreateStageContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Stage
{
    using Domain.Services.Contracts.Stage;
    using FluentValidation;

    public class CreateStageContractValidator : AbstractValidator<CreateStageContract>
    {
        public CreateStageContractValidator()
        {
            this.RuleFor(_ => _.Status).NotEmpty();
            this.RuleFor(_ => _.Feedback).NotEmpty();
        }
    }
}
