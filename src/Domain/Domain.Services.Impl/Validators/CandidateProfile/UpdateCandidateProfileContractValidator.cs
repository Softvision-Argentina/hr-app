// <copyright file="UpdateCandidateProfileContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.CandidateProfile
{
    using Domain.Services.Contracts.CandidateProfile;
    using FluentValidation;

    public class UpdateCandidateProfileContractValidator : AbstractValidator<UpdateCandidateProfileContract>
    {
        public UpdateCandidateProfileContractValidator()
        {
            this.RuleFor(_ => _.Id).NotEmpty();
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
