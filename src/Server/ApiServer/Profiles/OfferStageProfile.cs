// <copyright file="OfferStageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Stage;
    using AutoMapper;
    using Domain.Services.Contracts.Stage;

    public class OfferStageProfile : Profile
    {
        public OfferStageProfile()
        {
            this.CreateMap<CreateOfferStageViewModel, CreateOfferStageContract>();
            this.CreateMap<CreatedOfferStageContract, CreatedOfferStageViewModel>();
            this.CreateMap<ReadedOfferStageContract, ReadedOfferStageViewModel>();
            this.CreateMap<UpdateOfferStageViewModel, UpdateOfferStageContract>();
        }
    }
}
