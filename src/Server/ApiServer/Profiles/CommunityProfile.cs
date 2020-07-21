using ApiServer.Contracts.Community;
using AutoMapper;
using Domain.Services.Contracts.Community;

namespace ApiServer.Profiles
{
    public class CommunityProfile : Profile
    {
        public CommunityProfile()
        {
            CreateMap<CreateCommunityViewModel, CreateCommunityContract>();
            CreateMap<CreatedCommunityContract, CreatedCommunityViewModel>();
            CreateMap<ReadedCommunityContract, ReadedCommunityViewModel>();
            CreateMap<UpdateCommunityViewModel, UpdateCommunityContract>();
        }
    }
}
