using ApiServer.Contracts.Dashboard;
using ApiServer.Contracts.User;

namespace ApiServer.Contracts.UserDashboard
{
    public class CreateUserDashboardViewModel
    {
        public int UserId { get; set; }
        public CreateUserViewModel User { get; set; }
        public int DashboardId { get; set; }
        public CreateDashboardViewModel Dashboard { get; set; }
    }
}
