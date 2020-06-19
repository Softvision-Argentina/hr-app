using Domain.Services.Contracts.Stage;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Impl.Validators.Stages.ClientStage
{
    public class UpdateClientStageContractValidator : AbstractValidator<UpdateClientStageContract>
    {
        public UpdateClientStageContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_UPDATE, () =>
            {

            });
        }
    }
}
