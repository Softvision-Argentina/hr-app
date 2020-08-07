// <copyright file="IDashboardService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Dashboard;

    public interface IDashboardService
    {
        CreatedDashboardContract Create(CreateDashboardContract contract);

        ReadedDashboardContract Read(int id);

        void Update(UpdateDashboardContract contract);

        void Delete(int id);

        IEnumerable<ReadedDashboardContract> List();
    }
}
