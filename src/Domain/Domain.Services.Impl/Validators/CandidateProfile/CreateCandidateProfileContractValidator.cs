// <copyright file="CreateCandidateProfileContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.CandidateProfile
{
    using Domain.Services.Contracts.CandidateProfile;
    using FluentValidation;

    public class CreateCandidateProfileContractValidator : AbstractValidator<CreateCandidateProfileContract>
    {
        public CreateCandidateProfileContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETCREATE, () =>
            {
                this.RuleFor(_ => _.Name).NotEmpty();
                this.RuleFor(_ => _.Description).NotEmpty();
            });
        }
    }
}
