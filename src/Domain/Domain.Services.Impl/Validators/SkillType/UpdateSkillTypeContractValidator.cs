using Domain.Services.Contracts.SkillType;
using FluentValidation;

namespace Domain.Services.Impl.Validators.SkillType
{
    public class UpdateSkillTypeContractValidator : AbstractValidator<UpdateSkillTypeContract>
    {
        public UpdateSkillTypeContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
