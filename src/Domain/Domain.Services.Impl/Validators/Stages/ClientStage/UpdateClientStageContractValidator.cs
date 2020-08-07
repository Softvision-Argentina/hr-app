// <copyright file="UpdateClientStageContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Stages.ClientStage
{
    using Domain.Services.Contracts.Stage;
    using FluentValidation;

    public class UpdateClientStageContractValidator : AbstractValidator<UpdateClientStageContract>
    {
        public UpdateClientStageContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETUPDATE, () =>
            {
            });
        }
    }
}
