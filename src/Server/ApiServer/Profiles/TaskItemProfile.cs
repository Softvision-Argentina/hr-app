// <copyright file="TaskItemProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.TaskItem;
    using AutoMapper;
    using Domain.Services.Contracts.TaskItem;

    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            this.CreateMap<CreateTaskItemViewModel, CreateTaskItemContract>();
            this.CreateMap<CreatedTaskItemContract, CreatedTaskItemViewModel>();
            this.CreateMap<ReadedTaskItemContract, ReadedTaskItemViewModel>();
            this.CreateMap<UpdateTaskItemViewModel, UpdateTaskItemContract>();
        }
    }
}
