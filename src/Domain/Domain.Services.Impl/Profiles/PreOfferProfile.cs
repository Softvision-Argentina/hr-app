// <copyright file="PreOfferProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.PreOffer;

    public class PreOfferProfile : Profile
    {
        public PreOfferProfile()
        {
            this.CreateMap<PreOffer, ReadedPreOfferContract>();
            this.CreateMap<ReadedPreOfferContract, PreOffer>();
            this.CreateMap<CreatePreOfferContract, PreOffer>();
            this.CreateMap<DeclineReason, CreatedPreOfferContract>();
            this.CreateMap<UpdatePreOfferContract, PreOffer>();
        }
    }
}
