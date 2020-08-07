// <copyright file="PreOfferStageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Stage;
    using AutoMapper;
    using Domain.Services.Contracts.Stage;

    public class PreOfferStageProfile : Profile
    {
        public PreOfferStageProfile()
        {
            this.CreateMap<CreatePreOfferStageViewModel, CreatePreOfferStageContract>();
            this.CreateMap<CreatedPreOfferStageContract, CreatedPreOfferStageViewModel>();
            this.CreateMap<ReadedPreOfferStageContract, ReadedPreOfferStageViewModel>();
            this.CreateMap<UpdatePreOfferStageViewModel, UpdatePreOfferStageContract>();
        }
    }
}
