using ApiServer.Contracts.EmployeeCasualty;
using AutoMapper;
using Domain.Services.Contracts.EmployeeCasualty;

namespace ApiServer.Profiles
{
    public class EmployeeCasualtyProfile : Profile
    {
        public EmployeeCasualtyProfile()
        {
            CreateMap<CreateEmployeeCasualtyViewModel, CreateEmployeeCasualtyContract>();
            CreateMap<CreatedEmployeeCasualtyContract, CreatedEmployeeCasualtyViewModel>();
            CreateMap<ReadedEmployeeCasualtyContract, ReadedEmployeeCasualtyViewModel>();
            CreateMap<UpdateEmployeeCasualtyViewModel, UpdateEmployeeCasualtyContract>();
        }
    }
}