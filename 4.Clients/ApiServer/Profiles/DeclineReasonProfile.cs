using ApiServer.Contracts;
using AutoMapper;
using Domain.Services.Contracts;

namespace ApiServer.Profiles
{
    public class DeclineReasonProfile : Profile
    {
        public DeclineReasonProfile()
        {
            CreateMap<CreateDeclineReasonViewModel, CreateDeclineReasonContract>();
            CreateMap<CreatedDeclineReasonContract, CreatedDeclineReasonViewModel>();
            CreateMap<ReadedDeclineReasonContract, ReadedDeclineReasonViewModel>();
            CreateMap<UpdateDeclineReasonViewModel, UpdateDeclineReasonContract>();
        }
    }
}
