using System.Collections.Generic;
using ApiServer.Contracts.UserDashboard;

namespace ApiServer.Contracts.Dashboard
{
    public class CreateDashboardViewModel
    {
        public string Name { get; set; }      
        public ICollection<CreateUserDashboardViewModel> UserDashboards { get; set; }
    }
}
