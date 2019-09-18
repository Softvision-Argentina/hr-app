﻿using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Services.Contracts.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Profiles
{
    public class OfferStageProfile : Profile
    {
        public OfferStageProfile()
        {
            CreateMap<CreateOfferStageViewModel, CreateOfferStageContract>();
            CreateMap<CreatedOfferStageContract, CreatedOfferStageViewModel>();
            CreateMap<ReadedOfferStageContract, ReadedOfferStageViewModel>();
            CreateMap<UpdateOfferStageViewModel, UpdateOfferStageContract>();
        }
    }
}
