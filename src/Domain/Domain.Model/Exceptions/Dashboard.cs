using Core;
using System.Collections.Generic;

namespace Domain.Model
{
    public class Dashboard : Entity<int>
    {
        public string Name { get; set; }
        public IList<UserDashboard> UserDashboards { get; set; }
    }
}
