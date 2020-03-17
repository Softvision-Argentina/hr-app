using System;
using System.Collections.Generic;
using System.Text;
using ApiServer.Contracts.UserDashboard;

namespace ApiServer.Contracts.Dashboard
{
    public class UpdateDashboardViewModel
    {
        public string Name { get; set; }
        public ICollection<CreateUserDashboardViewModel> UserDashboards { get; set; }
    }
}
