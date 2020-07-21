using ApiServer.Contracts.TaskItem;
using AutoMapper;
using Domain.Services.Contracts.TaskItem;

namespace ApiServer.Profiles
{
    public class TaskItemProfile: Profile
    {
        public TaskItemProfile()
        {
            CreateMap<CreateTaskItemViewModel, CreateTaskItemContract>();
            CreateMap<CreatedTaskItemContract, CreatedTaskItemViewModel>();
            CreateMap<ReadedTaskItemContract, ReadedTaskItemViewModel>();
            CreateMap<UpdateTaskItemViewModel, UpdateTaskItemContract>();
        }
    }
}
