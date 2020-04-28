using Domain.Services.Contracts.Skill;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface ISkillService
    {
        CreatedSkillContract Create(CreateSkillContract contract);
        ReadedSkillContract Read(int id);
        void Update(UpdateSkillContract contract);
        void Delete(int id);
        IEnumerable<ReadedSkillContract> List();
    }
}
