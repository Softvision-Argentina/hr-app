// <copyright file="INotificationRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Domain.Model;

    public interface ISkillProfileRepository
    {
        List<SkillProfile> GetAll(int id);

        SkillProfile Get(int profileId, int skillId);

        SkillProfile Create(SkillProfile skillProfile);

        void Update(SkillProfile skillProfile);

        void Delete(SkillProfile skillProfile);
    }
}
