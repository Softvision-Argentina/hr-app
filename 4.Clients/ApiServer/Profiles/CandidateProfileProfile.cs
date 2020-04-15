using ApiServer.Contracts.CandidateProfile;
using Domain.Services.Contracts.CandidateProfile;

namespace ApiServer.Profiles
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
