// <copyright file="TaskProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Task;
    using AutoMapper;
    using Domain.Services.Contracts.Task;

    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            this.CreateMap<CreateTaskViewModel, CreateTaskContract>();
            this.CreateMap<CreatedTaskContract, CreatedTaskViewModel>();
            this.CreateMap<ReadedTaskContract, ReadedTaskViewModel>();
            this.CreateMap<UpdateTaskViewModel, UpdateTaskContract>();
        }
    }
}
