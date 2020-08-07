// <copyright file="EmployeeProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Employee;

    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            this.CreateMap<Employee, ReadedEmployeeContract>().ForMember(contract => contract.UserId, opt => opt.MapFrom(employee => employee.User.Id));
            this.CreateMap<CreateEmployeeContract, Employee>();
            this.CreateMap<Employee, CreatedEmployeeContract>();
            this.CreateMap<UpdateEmployeeContract, Employee>();
        }
    }
}
