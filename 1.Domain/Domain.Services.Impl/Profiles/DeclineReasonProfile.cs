using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

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
