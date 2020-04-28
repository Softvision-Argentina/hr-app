using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts;

namespace Domain.Services.Impl.Profiles
{
    public class DeclineReasonProfile : Profile
    {
        public DeclineReasonProfile()
        {
            CreateMap<DeclineReason, ReadedDeclineReasonContract>();
            CreateMap<ReadedDeclineReasonContract, DeclineReason>();
            CreateMap<CreateDeclineReasonContract, DeclineReason>();
            CreateMap<DeclineReason, CreatedDeclineReasonContract>();
            CreateMap<UpdateDeclineReasonContract, DeclineReason>();
        }
    }
}
