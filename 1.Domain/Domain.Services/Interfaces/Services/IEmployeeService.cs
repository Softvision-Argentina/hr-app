﻿using Domain.Model;
using Domain.Services.Contracts.Employee;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
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
