using System;
using System.Collections.Generic;
using System.Text;
using Domain.Services.Contracts.UserDashboard;

namespace Domain.Services.Contracts.Dashboard
{
    public class ReadedDashboardContract
    {
        public int Id { get; set; }
        public string Name { get; set; }       
        public ICollection<ReadedUserDashboardContract> UserDashboards { get; set; }
    }
}
