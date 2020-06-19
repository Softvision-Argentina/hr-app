﻿using Domain.Services.Contracts.Process;
using Domain.Services.Impl.Validators.Candidate;
using Domain.Services.Impl.Validators.Stages.ClientStage;
using Domain.Services.Impl.Validators.Stages.HrStage;
using Domain.Services.Impl.Validators.Stages.OfferStage;
using Domain.Services.Impl.Validators.Stages.PreOfferStage;
using Domain.Services.Impl.Validators.Stages.TechnicalStage;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Reservation
{
    public class CreateProcessContractValidator : AbstractValidator<CreateProcessContract>
    {
        public CreateProcessContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.Candidate).SetValidator(new UpdateCandidateContractValidator(), ValidatorConstants.RULESET_DEFAULT);
               
                RuleFor(_ => _.HrStage).SetValidator(new CreateHrStageContractValidator(), ValidatorConstants.RULESET_CREATE);
                RuleFor(_ => _.TechnicalStage).SetValidator(new CreateTechnicalStageContractValidator(), ValidatorConstants.RULESET_CREATE);
                RuleFor(_ => _.ClientStage).SetValidator(new CreateClientStageContractValidator(), ValidatorConstants.RULESET_CREATE);
                RuleFor(_ => _.PreOfferStage).SetValidator(new CreatePreOfferStageContractValidator(), ValidatorConstants.RULESET_CREATE);
                RuleFor(_ => _.OfferStage).SetValidator(new CreateOfferStageContractValidator(), ValidatorConstants.RULESET_CREATE);

                RuleFor(_ => _.CurrentStage).IsInEnum();
                RuleFor(_ => _.Seniority).IsInEnum();
                RuleFor(_ => _.Status).IsInEnum();

                RuleFor(_ => _.EnglishLevel).MaximumLength(ValidationConstants.MAX_INPUT);
                RuleFor(_ => _.Profile).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAX_TEXTAREA);

                RuleFor(_ => _.WantedSalary).LessThan(ValidationConstants.MAX_MONTHLY_INCOME);
                RuleFor(_ => _.ActualSalary).LessThan(ValidationConstants.MAX_MONTHLY_INCOME);
            });
        }
    }
}
