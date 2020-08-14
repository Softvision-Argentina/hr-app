namespace Domain.Services.Impl.Services
{
    using AutoMapper;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Contracts.SkillProfile;
    using Domain.Services.Interfaces.Repositories;
    using Domain.Services.Interfaces.Services;
    using System;
    using System.Collections.Generic;

    public class SkillProfileService : ISkillProfileService
    {
        private readonly ISkillProfileRepository _skillProfileRepository;
        private readonly IRepository<SkillType> _skillRepository;
        private readonly IRepository<CandidateProfile> _profileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SkillProfileService(
            ISkillProfileRepository skillProfileRepository,
            IRepository<SkillType> skillRepository,
            IRepository<CandidateProfile> profileRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this._skillProfileRepository = skillProfileRepository;
            this._unitOfWork = unitOfWork;
            this._skillRepository = skillRepository;
            this._profileRepository = profileRepository;
            this._mapper = mapper;
        }

        public CreatedSkillProfileContract Create(CreateSkillProfileContract skillProfileContract)
        {
            var skillProfile = this._mapper.Map<SkillProfile>(skillProfileContract);

            var skill = this._skillRepository.Get(skillProfile.SkillId);
            var profile = this._profileRepository.Get(skillProfile.ProfileId);

            if (skill != null && profile != null)
            {
                skillProfile.Skill = skill;
                skillProfile.Profile = profile;

                var createdSkillProfile = this._skillProfileRepository.Create(skillProfile);

                this._unitOfWork.Complete();

                return this._mapper.Map<CreatedSkillProfileContract>(createdSkillProfile);
            }

            throw new Exception("Could not associete profile to community");
        }

        public void Delete(int profileId, int skillId)
        {
            var skillProfile = this._skillProfileRepository.Get(profileId, skillId);

            if (skillProfile == null)
            {
                throw new Exception("Skill by profile not found");
            }

            this._skillProfileRepository.Delete(skillProfile);

            this._unitOfWork.Complete();
        }

        public IEnumerable<ReadedSkillProfileContract> GetAll(int id)
        {
            var skillProfiles = this._skillProfileRepository.GetAll(id);

            return this._mapper.Map<List<ReadedSkillProfileContract>>(skillProfiles);
        }

        public void Update(int profileId, int skillId, UpdateSkillProfileContract skillProfileContract)
        {
            var skillProfile = this._skillProfileRepository.Get(profileId, skillId);

            this._skillProfileRepository.Delete(skillProfile);

            this._unitOfWork.Complete();

            var skill = this._skillRepository.Get(skillProfileContract.SkillId);
            var profile = this._profileRepository.Get(skillProfileContract.ProfileId);

            skillProfile.Skill = skill;
            skillProfile.Profile = profile;

            this._skillProfileRepository.Create(skillProfile);

            this._unitOfWork.Complete();
        }
    }
}
