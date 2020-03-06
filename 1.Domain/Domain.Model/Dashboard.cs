using System;
using System.Collections.Generic;
using System.Text;
using Core;

namespace Domain.Model
{
    public class Dashboard : Entity<int>
    {
        public string Name { get; set; }
        public IList<UserDashboard> UserDashboards { get; set; }
    }
}
