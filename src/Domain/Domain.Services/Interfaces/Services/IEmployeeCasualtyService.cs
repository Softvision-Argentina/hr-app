// <copyright file="IEmployeeCasualtyService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.EmployeeCasualty;

    public interface IEmployeeCasualtyService
    {
        CreatedEmployeeCasualtyContract Create(CreateEmployeeCasualtyContract contract);

        ReadedEmployeeCasualtyContract Read(int id);

        void Update(UpdateEmployeeCasualtyContract contract);

        void Delete(int id);

        IEnumerable<ReadedEmployeeCasualtyContract> List();
    }
}
