// <copyright file="PreOfferProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.PreOffer;
    using AutoMapper;
    using Domain.Services.Contracts.PreOffer;

    public class PreOfferProfile : Profile
    {
        public PreOfferProfile()
        {
            this.CreateMap<CreatePreOfferViewModel, CreatePreOfferContract>();
            this.CreateMap<CreatedPreOfferContract, CreatedPreOfferViewModel>();
            this.CreateMap<ReadedPreOfferContract, ReadedPreOfferViewModel>();
            this.CreateMap<UpdatePreOfferViewModel, UpdatePreOfferContract>();
        }
    }
}
