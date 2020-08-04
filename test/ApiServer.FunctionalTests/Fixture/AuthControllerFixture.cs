using System;
using System.Collections.Generic;
using System.Security.Claims;
using ApiServer.Contracts.Login;
using Core.Testing.Platform;
using Domain.Services.ExternalServices;
using Domain.Services.ExternalServices.Config;
using Microsoft.Extensions.Configuration;
using Domain.Model;
using Microsoft.Extensions.Options;

namespace ApiServer.FunctionalTests.Fixture
{
    public class AuthControllerFixture : BaseFunctionalTestFixture
    {
        public IConfiguration Configuration { get; }
        private readonly AppSettings _appSettings;

        public AuthControllerFixture()
        {
            Configuration = Server.Host.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            ControllerName = "Auth";
            _appSettings = Configuration.Get<AppSettings>();
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
                return tokenType == TokenType.Valid ? int.Parse(_appSettings.JwtSettings.MinutesToExpiration) : 0;
            }

            var jwtSettings = new JwtSettings
            {
                Key = _appSettings.JwtSettings.Key,
                Issuer = _appSettings.JwtSettings.Issuer,
                Audience = _appSettings.JwtSettings.Audience,
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
            Client.Dispose();
            Server.Dispose();
        }

    }
}
