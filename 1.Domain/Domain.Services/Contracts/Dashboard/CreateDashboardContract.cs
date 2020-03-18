﻿using System.Collections.Generic;
using Domain.Services.Contracts.UserDashboard;

namespace Domain.Services.Contracts.Dashboard
{
    public class CreateDashboardContract
    {
        public string Name { get; set; }        
        public ICollection<CreateUserDashboardContract> UserDashboards { get; set; }
    }
}