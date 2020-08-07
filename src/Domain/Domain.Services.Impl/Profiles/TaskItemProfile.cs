// <copyright file="TaskItemProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.TaskItem;

    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            this.CreateMap<TaskItem, ReadedTaskItemContract>();
            this.CreateMap<CreateTaskItemContract, TaskItem>();
            this.CreateMap<Task, CreatedTaskItemContract>();
            this.CreateMap<UpdateTaskItemContract, TaskItem>();
        }
    }
}
