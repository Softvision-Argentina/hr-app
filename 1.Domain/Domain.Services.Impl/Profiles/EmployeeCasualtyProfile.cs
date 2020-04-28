using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.EmployeeCasualty;

namespace Domain.Services.Impl.Profiles
{
    public class EmployeeCasualtyProfile : Profile
    {
        public EmployeeCasualtyProfile()
        {
            CreateMap<EmployeeCasualty, ReadedEmployeeCasualtyContract>();
            CreateMap<CreateEmployeeCasualtyContract, EmployeeCasualty>();
            CreateMap<EmployeeCasualty, CreatedEmployeeCasualtyContract>();
            CreateMap<UpdateEmployeeCasualtyContract, EmployeeCasualty>();
        }
    }
}
