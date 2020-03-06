using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Contracts.UserDashboard
{
    public class CreatedUserDashboardContract
    {
        public int UserId { get; set; }
        public int DashboardId { get; set; }
    }
}
