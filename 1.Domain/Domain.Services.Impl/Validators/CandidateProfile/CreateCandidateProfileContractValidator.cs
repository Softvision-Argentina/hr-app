using Domain.Services.Contracts.CandidateProfile;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Impl.Validators.CandidateProfile
{
    public class CreateCandidateProfileContractValidator : AbstractValidator<CreateCandidateProfileContract>
    {
        public CreateCandidateProfileContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.Name).NotEmpty();
                RuleFor(_ => _.Description).NotEmpty();
            });
        }
    }
}
