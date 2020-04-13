using Domain.Services.Contracts.Candidate;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Impl.Validators.Candidate
{
    public class CreateCandidateContractValidator : AbstractValidator<CreateCandidateContract>
    {
        public CreateCandidateContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.DNI).GreaterThanOrEqualTo(0);
                RuleFor(_ => _.Name).NotEmpty();
                RuleFor(_ => _.LastName).NotEmpty();
                RuleFor(_ => _.User).NotEmpty();
                RuleFor(_ => _.Community).NotEmpty();
                RuleFor(_ => _.Profile).NotEmpty();
            });
        }
    }
}
