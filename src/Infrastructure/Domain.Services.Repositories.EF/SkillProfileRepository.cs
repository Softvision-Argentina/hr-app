// <copyright file="SkillRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    public class SkillProfileRepository : ISkillProfileRepository
    {
        private readonly DataBaseContext _dbContext;

        public SkillProfileRepository(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SkillProfile Create(SkillProfile skillProfile)
        {
            return _dbContext.SkillProfile.Add(skillProfile).Entity;
        }

        public void Delete(SkillProfile skillProfile)
        {
            _dbContext.SkillProfile.RemoveRange(skillProfile);
        }

        public List<SkillProfile> GetAll(int id)
        {
            return _dbContext.SkillProfile
                .Where(x => x.ProfileId == id)
                .Include(x => x.Profile)
                .Include(x => x.Skill)
                .ToList();
        }

        public SkillProfile Get(int profileId, int skillId)
        {
            var skillProfile = _dbContext.SkillProfile
                .Where(x => x.ProfileId == profileId && x.SkillId == skillId)
                .Include(x => x.Profile)
                .Include(x => x.Skill)
                .FirstOrDefault();

            return skillProfile;
        }

        public void Update(SkillProfile skillProfile)
        {
            _dbContext.SkillProfile.Update(skillProfile);
        }
    }
}
