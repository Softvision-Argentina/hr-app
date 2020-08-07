// <copyright file="CreateCandidateContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Candidate
{
    using Domain.Services.Contracts.Candidate;
    using FluentValidation;

    public class CreateCandidateContractValidator : AbstractValidator<CreateCandidateContract>
    {
        public CreateCandidateContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETCREATE, () =>
            {
                this.RuleFor(_ => _.Name)
                    .NotEmpty()
                    .MaximumLength(ValidationConstants.MAXINPUT);

                this.RuleFor(_ => _.LastName)
                    .NotEmpty()
                    .MaximumLength(ValidationConstants.MAXINPUT);

                this.RuleFor(_ => _.DNI).LessThan(ValidationConstants.MAXDNI);

                this.RuleFor(_ => _.EmailAddress)
                    .MaximumLength(ValidationConstants.MAXINPUTEMAIL)
                    .NotNull()
                        .When(_ => string.IsNullOrEmpty(_.PhoneNumber) || _.PhoneNumber.Length < 6);

                this.RuleFor(_ => _.PhoneNumber)
                    .MaximumLength(ValidationConstants.MAXPHONENUMBER)
                    .NotNull()
                        .When(_ => string.IsNullOrEmpty(_.EmailAddress));

                this.RuleFor(_ => _.EnglishLevel).IsInEnum();
                this.RuleFor(_ => _.Status).IsInEnum();

                this.RuleFor(_ => _.KnownFrom).MaximumLength(ValidationConstants.MAXINPUT);
                this.RuleFor(_ => _.LinkedInProfile).MaximumLength(ValidationConstants.MAXINPUT);
                this.RuleFor(_ => _.ReferredBy).MaximumLength(ValidationConstants.MAXINPUT);
                this.RuleFor(_ => _.Community).NotEmpty();
            });
        }
    }
}
