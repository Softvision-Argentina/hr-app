using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Employee;

namespace Domain.Services.Impl.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, ReadedEmployeeContract>().ForMember(contract => contract.UserId, opt => opt.MapFrom(employee => employee.User.Id));
            CreateMap<CreateEmployeeContract, Employee>();
            CreateMap<Employee, CreatedEmployeeContract>();
            CreateMap<UpdateEmployeeContract, Employee>();
        }
    }
}
