namespace Domain.Services.Interfaces.Services
{
    using Domain.Model;
    using Domain.Services.Contracts.OpenPositions;
    using Domain.Services.Contracts.ProfileByCommunity;
    using Domain.Services.Contracts.SkillProfile;
    using System;
    using System.Collections.Generic;

    public interface ISkillProfileService
    {
        CreatedSkillProfileContract Create(CreateSkillProfileContract skillProfileContract);

        IEnumerable<ReadedSkillProfileContract> GetAll(int id);

        void Update(int profileId, int skillId, UpdateSkillProfileContract skillProfile);

        void Delete(int profileId, int skillId);
    }
}
