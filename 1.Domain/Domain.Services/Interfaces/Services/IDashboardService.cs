using System.Collections.Generic;
using Domain.Services.Contracts.Dashboard;

namespace Domain.Services.Interfaces.Services
{
    public interface IDashboardService
    {
        CreatedDashboardContract Create(CreateDashboardContract contract);
        ReadedDashboardContract Read(int id);
        void Update(UpdateDashboardContract contract);
        void Delete(int id);
        IEnumerable<ReadedDashboardContract> List();
    }
}
