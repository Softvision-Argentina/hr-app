// <copyright file="AuthControllerFixture.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.FunctionalTests.Fixture
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using ApiServer.Contracts.Login;
    using Core.Testing.Platform;
    using Domain.Model;
    using Domain.Services.ExternalServices;
    using Domain.Services.ExternalServices.Config;
    using Microsoft.Extensions.Configuration;

    public class AuthControllerFixture : BaseFunctionalTestFixture
    {
        public IConfiguration Configuration { get; }

        private readonly AppSettings appSettings;

        public AuthControllerFixture()
        {
            this.Configuration = this.Server.Host.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            this.ControllerName = "Auth";
            this.appSettings = this.Configuration.Get<AppSettings>();
        }

        public enum TokenType
        {
            Valid,
            Expired,
        }

        public TokenViewModel GetTestToken(TokenType tokenType)
        {
            int GetTokenExpiration()
            {
                return tokenType == TokenType.Valid ? int.Parse(this.appSettings.JwtSettings.MinutesToExpiration) : 0;
            }

            var jwtSettings = new JwtSettings
            {
                Key = this.appSettings.JwtSettings.Key,
                Issuer = this.appSettings.JwtSettings.Issuer,
                Audience = this.appSettings.JwtSettings.Audience,
                MinutesToExpiration = GetTokenExpiration(),
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
            this.Client.Dispose();
            this.Server.Dispose();
        }
    }
}
