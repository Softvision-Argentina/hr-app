using ApiServer.Contracts.DaysOff;
using AutoMapper;
using Domain.Services.Contracts.DaysOff;

namespace ApiServer.Profiles
{
    public class DaysOffProfile : Profile
    {
        public DaysOffProfile()
        {
            CreateMap<CreateDaysOffViewModel, CreateDaysOffContract>();
            CreateMap<CreatedDaysOffContract, CreatedDaysOffViewModel>();
            CreateMap<ReadedDaysOffContract, ReadedDaysOffViewModel>();
            CreateMap<UpdateDaysOffViewModel, UpdateDaysOffContract>();
        }
    }
}