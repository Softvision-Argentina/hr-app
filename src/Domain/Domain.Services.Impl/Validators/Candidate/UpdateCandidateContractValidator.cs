// <copyright file="UpdateCandidateContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Candidate
{
    using Domain.Services.Contracts.Candidate;
    using FluentValidation;

    public class UpdateCandidateContractValidator : AbstractValidator<UpdateCandidateContract>
    {
        public UpdateCandidateContractValidator()
        {
            this.RuleFor(_ => _.Name).NotEmpty()
                .MaximumLength(ValidationConstants.MAXINPUT);

            this.RuleFor(_ => _.LastName).NotEmpty()
                .MaximumLength(ValidationConstants.MAXINPUT);

            // RuleFor(_ => _.User).NotEmpty();
            this.RuleFor(_ => _.Community).NotEmpty();

            // RuleFor(_ => _.Profile).NotEmpty();

            this.RuleFor(_ => _.Cv)
                .MaximumLength(100);

            this.RuleFor(_ => _.EmailAddress)
                .MaximumLength(ValidationConstants.MAXINPUTEMAIL);

            this.RuleFor(_ => _.EnglishLevel).IsInEnum();

            this.RuleFor(_ => _.KnownFrom)
                .MaximumLength(ValidationConstants.MAXINPUT);

            this.RuleFor(_ => _.LinkedInProfile)
                .MaximumLength(ValidationConstants.MAXINPUT);

            this.RuleFor(_ => _.PhoneNumber)
                .MaximumLength(ValidationConstants.MAXPHONENUMBER);

            this.RuleFor(_ => _.ReferredBy)
                .MaximumLength(ValidationConstants.MAXINPUT);

            this.RuleFor(_ => _.Status).IsInEnum();

            this.RuleFor(_ => _.DNI).LessThan(ValidationConstants.MAXDNI);
        }
    }
}
