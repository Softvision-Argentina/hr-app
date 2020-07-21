using ApiServer.Contracts.Task;
using AutoMapper;
using Domain.Services.Contracts.Task;

namespace ApiServer.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskViewModel, CreateTaskContract>();
            CreateMap<CreatedTaskContract, CreatedTaskViewModel>();
            CreateMap<ReadedTaskContract, ReadedTaskViewModel>();
            CreateMap<UpdateTaskViewModel, UpdateTaskContract>();
        }
    }
}
