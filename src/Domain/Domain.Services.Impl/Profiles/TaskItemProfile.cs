using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.TaskItem;

namespace Domain.Services.Impl.Profiles
{
    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            CreateMap<TaskItem, ReadedTaskItemContract>();
            CreateMap<CreateTaskItemContract, TaskItem>();
            CreateMap<Task, CreatedTaskItemContract>();
            CreateMap<UpdateTaskItemContract, TaskItem>();
        }
    }
}
