using ApiServer.Contracts.Dashboard;
using ApiServer.Contracts.User;

namespace ApiServer.Contracts.UserDashboard
{
    public class ReadedUserDashboardViewModel
    {
        public int UserId { get; set; }
        public ReadedUserViewModel User { get; set; }
        public int DashboardId { get; set; }
        public ReadedDashboardViewModel Dashboard { get; set; }
    }
}
