﻿// <copyright file="CreateClientStageContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Interview
{
    using Domain.Services.Contracts.Interview;
    using FluentValidation;

    public class UpdateInterviewContractValidator : AbstractValidator<UpdateInterviewContract>
    {
        public UpdateInterviewContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETCREATE, () =>
            {
                this.RuleFor(_ => _.Feedback).MaximumLength(ValidationConstants.MAXTEXTINTERVIEWFEEDBACK);
            });
        }
    }
}
