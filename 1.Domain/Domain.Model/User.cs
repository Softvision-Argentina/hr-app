using Core;
using Domain.Model.Enum;
using System.Collections.Generic;

namespace Domain.Model
{
    public class User : Entity<int> 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public Roles Role { get; set; }
        public string Password { get; set; }
        public Community Community { get; set; }
        public IList<UserDashboard> UserDashboards { get; set; }
        public IList<Candidate> Candidates { get; set; }
        public IList<Task> Tasks { get; set; }
    }
}
