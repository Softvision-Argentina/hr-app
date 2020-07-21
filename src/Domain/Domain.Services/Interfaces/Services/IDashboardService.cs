using Domain.Services.Contracts.Dashboard;
using System.Collections.Generic;

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
