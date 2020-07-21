using Domain.Services.Contracts.CandidateProfile;
using FluentValidation;

namespace Domain.Services.Impl.Validators.CandidateProfile
{
    public class UpdateCandidateProfileContractValidator : AbstractValidator<UpdateCandidateProfileContract>
    {
        public UpdateCandidateProfileContractValidator()
        {
            RuleFor(_ => _.Id).NotEmpty();
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
