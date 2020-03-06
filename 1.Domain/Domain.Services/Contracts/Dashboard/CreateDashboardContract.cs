using System;
using System.Collections.Generic;
using System.Text;
using Domain.Model;
using Domain.Services.Contracts.UserDashboard;

namespace Domain.Services.Contracts.Dashboard
{
    public class CreateDashboardContract
    {
        public string Name { get; set; }
        
        public IList<CreateUserDashboardContract> UserDashboards { get; set; }
    }
}
