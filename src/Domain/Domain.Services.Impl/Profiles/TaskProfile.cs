// <copyright file="TaskProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Task;

    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            this.CreateMap<Task, ReadedTaskContract>();
            this.CreateMap<CreateTaskContract, Task>();
            this.CreateMap<Task, CreatedTaskContract>();
            this.CreateMap<UpdateTaskContract, Task>();
        }
    }
}
