using Domain.Services.Contracts.Candidate;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Candidate
{
    public class UpdateCandidateContractValidator : AbstractValidator<UpdateCandidateContract>
    {
        public UpdateCandidateContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.LastName).NotEmpty();
            RuleFor(_ => _.User).NotEmpty();
            RuleFor(_ => _.Community).NotEmpty();
            RuleFor(_ => _.Profile).NotEmpty();
        }
    }
}
