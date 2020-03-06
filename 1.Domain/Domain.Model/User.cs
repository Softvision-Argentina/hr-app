using Core;
using Domain.Model.Enum;
using Microsoft.AspNetCore.Identity;

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
        public Preference Preference { get; set; }
    }
}
