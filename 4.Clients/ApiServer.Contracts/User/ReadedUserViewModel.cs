using ApiServer.Contracts.Community;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Contracts.User
{
    public class ReadedUserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public ReadedCommunityViewModel Community { get; set; }
    }
}
