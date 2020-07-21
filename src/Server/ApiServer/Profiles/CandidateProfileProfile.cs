using ApiServer.Contracts.CandidateProfile;
using ApiServer.Profiles;
using Domain.Services.Contracts.CandidateProfile;

namespace ApiServer.CandidateProfiles
{
    public class CandidateProfileProfile : CandidateProfile
    {
        public CandidateProfileProfile()
        {
            CreateMap<CreateCandidateProfileViewModel, CreateCandidateProfileContract>();
            CreateMap<CreatedCandidateProfileContract, CreatedCandidateProfileViewModel>();
            CreateMap<ReadedCandidateProfileContract, ReadedCandidateProfileViewModel>();
            CreateMap<UpdateCandidateProfileViewModel, UpdateCandidateProfileContract>();
        }

    }
}
