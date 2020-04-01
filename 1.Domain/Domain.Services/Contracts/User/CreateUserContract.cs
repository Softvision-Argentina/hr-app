using Domain.Model.Enum;
using Domain.Services.Contracts.Community;

namespace Domain.Services.Contracts.User
{
    public class CreateUserContract
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public Roles Role { get; set; }
        public CreateCommunityContract Community { get; set; }
        public string Password { get; set; }
    }
}
