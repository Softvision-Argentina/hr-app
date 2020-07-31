using Domain.Services.Contracts.Candidate;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Candidate
{
    public class CreateCandidateContractValidator : AbstractValidator<CreateCandidateContract>
    {
        public CreateCandidateContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.Name)
                    .NotEmpty()
                    .MaximumLength(ValidationConstants.MAX_INPUT);
                
                RuleFor(_ => _.LastName)
                    .NotEmpty()
                    .MaximumLength(ValidationConstants.MAX_INPUT);

                RuleFor(_ => _.DNI).LessThan(ValidationConstants.MAX_DNI);

                RuleFor(_ => _.EmailAddress)
                    .MaximumLength(ValidationConstants.MAX_INPUT_EMAIL)
                    .NotNull()
                        .When(_ => string.IsNullOrEmpty(_.PhoneNumber) || _.PhoneNumber.Length < 6);

                RuleFor(_ => _.PhoneNumber)
                    .MaximumLength(ValidationConstants.MAX_PHONE_NUMBER)
                    .NotNull()
                        .When(_ => string.IsNullOrEmpty(_.EmailAddress));

                RuleFor(_ => _.EnglishLevel).IsInEnum();
                RuleFor(_ => _.Status).IsInEnum();

                RuleFor(_ => _.KnownFrom).MaximumLength(ValidationConstants.MAX_INPUT);
                RuleFor(_ => _.LinkedInProfile).MaximumLength(ValidationConstants.MAX_INPUT);
                RuleFor(_ => _.ReferredBy).MaximumLength(ValidationConstants.MAX_INPUT);

                RuleFor(_ => _.Community).NotEmpty();
            });
        }
    }
}
