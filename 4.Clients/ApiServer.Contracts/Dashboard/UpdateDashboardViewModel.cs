using ApiServer.Contracts.UserDashboard;
using System.Collections.Generic;

namespace ApiServer.Contracts.Dashboard
{
    public class UpdateDashboardViewModel
    {
        public string Name { get; set; }
        public ICollection<CreateUserDashboardViewModel> UserDashboards { get; set; }
    }
}
