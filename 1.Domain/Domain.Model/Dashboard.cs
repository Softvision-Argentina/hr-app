using System.Collections.Generic;
using Core;

namespace Domain.Model
{
    public class Dashboard : Entity<int>
    {
        public string Name { get; set; }
        public IList<UserDashboard> UserDashboards { get; set; }
    }
}
