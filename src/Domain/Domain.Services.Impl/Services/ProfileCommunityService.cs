namespace Domain.Services.Impl.Services
{
    using AutoMapper;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Contracts.ProfileByCommunity;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ProfileCommunityService : IProfileCommunityService
    {
        private readonly IRepository<ProfileCommunity> _profileByCommunityRepository;
        private readonly IRepository<Community> _communityRepository;
        private readonly IRepository<CandidateProfile> _profileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProfileCommunityService(
            IRepository<ProfileCommunity> profileByCommunityRepository,
            IRepository<Community> communityRepository,
            IRepository<CandidateProfile> profileRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this._profileByCommunityRepository = profileByCommunityRepository;
            this._unitOfWork = unitOfWork;
            this._communityRepository = communityRepository;
            this._profileRepository = profileRepository;
            this._mapper = mapper;
        }

        public CreatedProfileCommunityContract Create(CreateProfileCommunityContract profileByCommunity)
        {
            var profileByComm = this._mapper.Map<ProfileCommunity>(profileByCommunity);

            var community = this._communityRepository.Get(profileByCommunity.Community.Id);
            var profile = this._profileRepository.Get(profileByCommunity.Profile.Id);

            if(community != null && profile != null)
            {
                profileByComm.Community = community;
                profileByComm.Profile = profile;

                var createdProfileByCommunity = this._profileByCommunityRepository.Create(profileByComm);

                this._unitOfWork.Complete();

                return this._mapper.Map<CreatedProfileCommunityContract>(createdProfileByCommunity);
            }

            throw new Exception("Could not associete profile to community");
        }

        public void Delete(int id)
        {
            var profileByComm = this._profileByCommunityRepository.Query().FirstOrDefault(p => p.Id == id);

            if (profileByComm == null)
            {
                throw new Exception("Profile by community not found");
            }

            this._profileByCommunityRepository.Delete(profileByComm);

            this._unitOfWork.Complete();
        }

        public IEnumerable<ReadedProfileCommunityContract> Get(int id)
        {
            var profileByComms = this._profileByCommunityRepository.QueryEager().Where(x => x.Community.Id == id).ToList();

            return this._mapper.Map<List<ReadedProfileCommunityContract>>(profileByComms);
        }

        public void Update(UpdateProfileCommunityContract profileByCommunity)
        {
            var profileByComm = this._mapper.Map<ProfileCommunity>(profileByCommunity);

            var community = this._communityRepository.Get(profileByCommunity.CommunityContract.Id);
            var profile = this._profileRepository.Get(profileByCommunity.ProfileContract.Id);

            profileByComm.Community = community;
            profileByComm.Profile = profile;

            this._profileByCommunityRepository.Update(profileByComm);

            this._unitOfWork.Complete();
        }
    }
}
