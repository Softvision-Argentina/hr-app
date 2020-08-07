// <copyright file="HrStageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Stage;
    using AutoMapper;
    using Domain.Services.Contracts.Stage;

    public class HrStageProfile : Profile
    {
        public HrStageProfile()
        {
            this.CreateMap<CreateHrStageViewModel, CreateHrStageContract>();
            this.CreateMap<CreatedHrStageContract, CreatedHrStageViewModel>();
            this.CreateMap<ReadedHrStageContract, ReadedHrStageViewModel>();
            this.CreateMap<UpdateHrStageViewModel, UpdateHrStageContract>();
        }
    }
}
