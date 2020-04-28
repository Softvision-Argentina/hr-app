using System;
using System.Collections.Generic;
using System.Security.Claims;
using ApiServer.FunctionalTests.Core;
using Persistance.EF.Extensions;
using ApiServer.Contracts.Login;
using Domain.Services.ExternalServices;
using Domain.Services.ExternalServices.Config;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Task = System.Threading.Tasks.Task;

namespace ApiServer.FunctionalTests.Fixture
{
    public class AuthControllerFixture : BaseFunctionalTestFixture
    {
        public IConfiguration Configuration { get; }
        public AuthControllerFixture()
        {
            Configuration = Server.Host.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            ControllerName = "Auth";
        }

        public enum TokenType
        {
            Valid,
            Expired
        }
        public TokenViewModel GetTestToken(TokenType tokenType)
        {
            int GetTokenExpiration()
            {
                return tokenType == TokenType.Valid ? int.Parse(Configuration["jwtSettings:minutesToExpiration"]) : 0;
            }

            var jwtSettings = new JwtSettings
            {
                Key = Configuration["jwtSettings:key"],
                Issuer = Configuration["jwtSettings:issuer"],
                Audience = Configuration["jwtSettings:audience"],
                MinutesToExpiration = GetTokenExpiration()
            };

            var tokenProvider = new JwtSecurityTokenProvider(jwtSettings);
            var emailClaim = new Claim("email", "rodrigo.ramirez@softvision.com");
            var expClaim = new Claim("exp", DateTime.MaxValue.ToString());
            var claims = new List<Claim>() { expClaim, emailClaim };
            var token = tokenProvider.BuildSecurityToken("rodrigo.ramirez", claims);
            var tokenViewModel = new TokenViewModel { Token = token };

            return tokenViewModel;
        }
        public void Dispose()
        {
            Context.Dispose();
            Client.Dispose();
            Server.Dispose();
        }

    }
}
