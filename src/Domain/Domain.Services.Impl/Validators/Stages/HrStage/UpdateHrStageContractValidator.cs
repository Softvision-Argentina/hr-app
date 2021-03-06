﻿// <copyright file="UpdateHrStageContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Stages.HrStage
{
    using Domain.Services.Contracts.Stage;
    using FluentValidation;

    public class UpdateHrStageContractValidator : AbstractValidator<UpdateHrStageContract>
    {
        public UpdateHrStageContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETUPDATE, () =>
            {
                this.RuleFor(_ => _.ActualSalary).LessThan(ValidationConstants.MAXMONTHLYINCOME);
                this.RuleFor(_ => _.WantedSalary).LessThan(ValidationConstants.MAXMONTHLYINCOME);

                this.RuleFor(_ => _.AdditionalInformation).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.Feedback).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAXTEXTAREA);

                this.RuleFor(_ => _.EnglishLevel).IsInEnum();
                this.RuleFor(_ => _.RejectionReasonsHr).IsInEnum();
                this.RuleFor(_ => _.Status).IsInEnum();
            });
        }
    }
}
