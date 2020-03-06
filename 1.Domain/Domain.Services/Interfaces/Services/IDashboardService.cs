using System;
using System.Collections.Generic;
using System.Text;
using Domain.Services.Contracts.Dashboard;

namespace Domain.Services.Interfaces.Services
{
    public interface IDashboardService
    {
        CreatedDashboardContract Create(CreateDashboardContract contract);
        ReadedDashboardContract Read(int Id);
        void Update(UpdateDashboardContract contract);
        void Delete(int Id);
        IEnumerable<ReadedDashboardContract> List();
    }
}
