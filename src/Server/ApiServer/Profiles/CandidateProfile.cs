using ApiServer.Contracts.Candidates;
using AutoMapper;
using Domain.Services.Contracts.Candidate;

namespace ApiServer.Profiles
{
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            CreateMap<CreateCandidateViewModel, CreateCandidateContract>();
            CreateMap<CreatedCandidateContract, CreatedCandidateViewModel>();
            CreateMap<ReadedCandidateContract, ReadedCandidateViewModel>();
            CreateMap<ReadedCandidateAppContract, ReadedCandidateAppViewModel>();
            CreateMap<UpdateCandidateViewModel, UpdateCandidateContract>();
        }
    }
}
