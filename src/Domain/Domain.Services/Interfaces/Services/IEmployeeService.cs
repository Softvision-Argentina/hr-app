// <copyright file="IEmployeeService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Model;
    using Domain.Services.Contracts.Employee;

    public interface IEmployeeService
    {
        IEnumerable<ReadedEmployeeContract> List();

        void Delete(int id);

        CreatedEmployeeContract Create(CreateEmployeeContract contract);

        void UpdateEmployee(UpdateEmployeeContract contract);

        Employee GetById(int id);

        Employee GetByDNI(int dni);

        Employee GetByEmail(string email);
    }
}
