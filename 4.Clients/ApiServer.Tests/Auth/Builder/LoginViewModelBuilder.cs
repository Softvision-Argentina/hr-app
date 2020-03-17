using ApiServer.Contracts.Login;
using System;

namespace ApiServer.Tests.Candidates.Builder
{
    public class LoginViewModelBuilder
    {
        private string Username { get; set; }
        private string Password { get; set; }

        public LoginViewModelBuilder()
        {
            Username = $"{Guid.NewGuid()}";
            Password = $"{Username}#password";
        }

        public LoginViewModel GetInvalidData()
        {
            return new LoginViewModel()
            {
                UserName = this.Username,
                Password = this.Password
            };
        }

        public LoginViewModel GetValidData()
        {
            return new LoginViewModel()
            {
                UserName = "nicolas.roldan@softvision.com",
                Password = "1234"
            };
        }
    }
}
