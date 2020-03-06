using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class UserDashboard
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int DashboardId { get; set; }
        public Dashboard Dashboard { get; set; }
    }
}
