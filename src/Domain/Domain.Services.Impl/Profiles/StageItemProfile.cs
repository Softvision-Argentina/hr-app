// <copyright file="StageItemProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Stage.StageItem;

    public class StageItemProfile : Profile
    {
        public StageItemProfile()
        {
            this.CreateMap<CreateStageItemContract, StageItem>();
            this.CreateMap<StageItem, CreatedStageItemContract>();
            this.CreateMap<UpdateStageItemContract, StageItem>();
        }
    }
}
