// <copyright file="UpdateDeclineReasonContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators
{
    using Domain.Services.Contracts;
    using FluentValidation;

    public class UpdateDeclineReasonContractValidator : AbstractValidator<UpdateDeclineReasonContract>
    {
        public UpdateDeclineReasonContractValidator()
        {
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
