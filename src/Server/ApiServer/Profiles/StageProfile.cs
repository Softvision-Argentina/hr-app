// <copyright file="StageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Stage;
    using AutoMapper;
    using Domain.Services.Contracts.Stage;

    public class StageProfile : Profile
    {
        public StageProfile()
        {
            this.CreateMap<CreateStageViewModel, CreateStageContract>();
            this.CreateMap<CreatedStageContract, CreatedStageViewModel>();
            this.CreateMap<ReadedStageContract, ReadedStageViewModel>();
            this.CreateMap<UpdateStageViewModel, UpdateStageContract>();
        }
    }
}
