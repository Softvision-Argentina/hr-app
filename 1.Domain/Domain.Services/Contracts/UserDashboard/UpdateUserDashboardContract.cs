using Domain.Services.Contracts.Dashboard;
using Domain.Services.Contracts.User;

namespace Domain.Services.Contracts.UserDashboard
{
    public class UpdateUserDashboardContract
    {
        public int UserId { get; set; }
        public ReadedUserContract User { get; set; }
        public int DashboardId { get; set; }
        public ReadedDashboardContract Dashboard { get; set; }
    }
}
