using ApiServer.Contracts.ReaddressStatus;
using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.ReaddressStatus;

namespace ApiServer.Profiles
{
    public class ReaddressStatusProfile : Profile
    {
        public ReaddressStatusProfile()
        {
            CreateMap<CreateReaddressStatusViewModel, CreateReaddressStatus>();
            CreateMap<UpdateReaddressStatusViewModel, UpdateReaddressStatus>();
            CreateMap<CreateReaddressStatus, ReaddressStatus>();
            CreateMap<UpdateReaddressStatus, ReaddressStatus>();
        }
    }
}
