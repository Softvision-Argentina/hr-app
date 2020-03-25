using Domain.Services.Contracts.Dashboard;
using Domain.Services.Contracts.User;

namespace Domain.Services.Contracts.UserDashboard
{
    public class CreateUserDashboardContract
    {
        public int UserId { get; set; }
        public CreateUserContract User { get; set; }
        public int DashboardId { get; set; }
        public CreateDashboardContract Dashboard { get; set; }
    }
}
