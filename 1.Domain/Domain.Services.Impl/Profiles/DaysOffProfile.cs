using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.DaysOff;

namespace Domain.Services.Impl.Profiles
{
    public class DaysOffProfile : Profile
    {
        public DaysOffProfile()
        {
            CreateMap<DaysOff, ReadedDaysOffContract>();
            CreateMap<CreateDaysOffContract, DaysOff>();
            CreateMap<DaysOff, CreatedDaysOffContract>();
            CreateMap<UpdateDaysOffContract, DaysOff>();
        }
    }
}
