// <copyright file="JwtSecurityTokenProvider.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.ExternalServices
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Core;
    using Domain.Services.ExternalServices.Config;
    using Microsoft.IdentityModel.Tokens;

    public class JwtSecurityTokenProvider : ISecurityTokenProvider
    {
        private readonly JwtSettings settings;

        public JwtSecurityTokenProvider(JwtSettings settings)
        {
            this.settings = settings;
        }

        public string BuildSecurityToken(string userName, List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.settings.Key));
            var jwtClaims = new List<Claim>();
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName));
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            jwtClaims.Add(new Claim("isAuthenticated", "true"));

            foreach (var claim in claims)
            {
                jwtClaims.Add(claim);
            }

            var token = new JwtSecurityToken(
                issuer: this.settings.Issuer,
                audience: this.settings.Audience,
                claims: jwtClaims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(this.settings.MinutesToExpiration),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
