using ApiServer.Contracts;
using AutoMapper;
using Domain.Services.Contracts;
using System.Collections;
using System.Collections.Generic;

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
