// <copyright file="ProcessProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Process;
    using AutoMapper;
    using Domain.Services.Contracts.Process;

    public class ProcessProfile : Profile
    {
        public ProcessProfile()
        {
            this.CreateMap<ReadedProcessContract, ReadedProcessViewModel>();
            this.CreateMap<CreateProcessViewModel, CreateProcessContract>();
            this.CreateMap<CreatedProcessContract, CreatedProcessViewModel>();
            this.CreateMap<UpdateProcessViewModel, UpdateProcessContract>();
            this.CreateMap<ReadedProcessContract, TableProcessViewModel>();
        }
    }
}
