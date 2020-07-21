using Domain.Services.Contracts.SkillType;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface ISkillTypeService
    {
        CreatedSkillTypeContract Create(CreateSkillTypeContract contract);
        ReadedSkillTypeContract Read(int id);
        void Update(UpdateSkillTypeContract contract);
        void Delete(int id);
        IEnumerable<ReadedSkillTypeContract> List();
    }
}
