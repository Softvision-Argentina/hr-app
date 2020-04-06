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
            RuleSet("Create", () =>
            {
                RuleFor(_ => _.DNI).NotEmpty();
                RuleFor(_ => _.DNI).GreaterThan(0);
                RuleFor(_ => _.Name).NotEmpty();
                RuleFor(_ => _.LastName).NotEmpty();
                RuleFor(_ => _.Recruiter).NotEmpty();
                RuleFor(_ => _.Community).NotEmpty();
                RuleFor(_ => _.Profile).NotEmpty();
                RuleFor(_ => _.LinkedInProfile).NotEmpty();
            });
        }
    }
}
