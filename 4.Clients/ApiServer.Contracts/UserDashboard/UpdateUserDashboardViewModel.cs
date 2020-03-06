using System;
using System.Collections.Generic;
using System.Text;
using ApiServer.Contracts.Dashboard;
using ApiServer.Contracts.User;

namespace ApiServer.Contracts.UserDashboard
{
    public class UpdateUserDashboardViewModel
    {
        public int UserId { get; set; }
        public UpdateUserViewModel User { get; set; }

        public int DashboardId { get; set; }
        public UpdateDashboardViewModel Dashboard { get; set; }
    }
}
