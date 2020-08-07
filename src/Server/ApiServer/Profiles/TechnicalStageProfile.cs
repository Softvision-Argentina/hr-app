// <copyright file="TechnicalStageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Stage;
    using AutoMapper;
    using Domain.Services.Contracts.Stage;

    public class TechnicalStageProfile : Profile
    {
        public TechnicalStageProfile()
        {
            this.CreateMap<CreateTechnicalStageViewModel, CreateTechnicalStageContract>();
            this.CreateMap<CreatedTechnicalStageContract, CreatedTechnicalStageViewModel>();
            this.CreateMap<ReadedTechnicalStageContract, ReadedTechnicalStageViewModel>();
            this.CreateMap<UpdateTechnicalStageViewModel, UpdateTechnicalStageContract>();
        }
    }
}
