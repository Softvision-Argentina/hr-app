using ApiServer.Contracts.UserDashboard;
using System.Collections.Generic;

namespace ApiServer.Contracts.Dashboard
{
    public class ReadedDashboardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ReadedUserDashboardViewModel> UserDashboards { get; set; }
    }
}
