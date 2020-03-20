using ApiServer.Contracts.Login;
using System;

namespace ApiServer.Tests.Candidates.Builder
{
    public class LoginViewModelBuilder
    {
        readonly string _username;
        readonly string _password;

        public LoginViewModelBuilder()
        {
            _username = $"{Guid.NewGuid()}";
            _password = $"{_username}#password";
        }

        public LoginViewModel GetInvalidData()
        {
            return new LoginViewModel()
            {
                UserName = this._username,
                Password = this._password
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
