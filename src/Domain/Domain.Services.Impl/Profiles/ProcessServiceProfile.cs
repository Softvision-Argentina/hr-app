// <copyright file="ProcessServiceProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Process;

    public class ProcessServiceProfile : Profile
    {
        public ProcessServiceProfile()
        {
            this.CreateMap<Process, ReadedProcessContract>();
            this.CreateMap<UpdateProcessContract, Process>();
            this.CreateMap<CreateProcessContract, Process>();
            this.CreateMap<Process, CreatedProcessContract>();
        }
    }
}
