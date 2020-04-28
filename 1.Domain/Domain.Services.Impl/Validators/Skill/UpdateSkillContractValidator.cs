using Domain.Services.Contracts.Skill;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Skill
{
    public class UpdateSkillContractValidator : AbstractValidator<UpdateSkillContract>
    {
        public UpdateSkillContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.Type).NotEmpty();
        }

    }
}
