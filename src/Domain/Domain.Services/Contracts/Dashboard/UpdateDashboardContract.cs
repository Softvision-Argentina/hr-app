using Domain.Services.Contracts.UserDashboard;
using System.Collections.Generic;

namespace Domain.Services.Contracts.Dashboard
{
    public class UpdateDashboardContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CreateUserDashboardContract> UserDashboards { get; set; }
    }
}
