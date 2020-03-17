using System.Collections.Generic;
using ApiServer.Contracts.UserDashboard;

namespace ApiServer.Contracts.Dashboard
{
    public class ReadedDashboardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ReadedUserDashboardViewModel> UserDashboards { get; set; }
    }
}
