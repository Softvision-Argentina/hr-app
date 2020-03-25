using System;
using System.Collections.Generic;
using System.Text;
using Domain.Services.Contracts.UserDashboard;

namespace Domain.Services.Contracts.Dashboard
{
    public class UpdateDashboardContract
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CreateUserDashboardContract> UserDashboards { get; set; }
    }
}
