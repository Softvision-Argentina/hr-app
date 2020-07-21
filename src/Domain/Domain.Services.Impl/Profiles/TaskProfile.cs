using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Task;

namespace Domain.Services.Impl.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Task, ReadedTaskContract>();
            CreateMap<CreateTaskContract, Task>();
            CreateMap<Task, CreatedTaskContract>();
            CreateMap<UpdateTaskContract, Task>();
        }
    }
}
