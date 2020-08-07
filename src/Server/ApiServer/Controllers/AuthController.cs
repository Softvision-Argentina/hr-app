namespace ApiServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using ApiServer.Contracts.Login;
    using ApiServer.Contracts.User;
    using AutoMapper;
    using Core;
    using Domain.Model;
    using Domain.Services.ExternalServices.Config;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IOptions<AppSettings> appSettings;
        private IConfiguration object1;
        private IUserService object2;
        private IMapper object3;

        public AuthController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.appSettings = appSettings;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]LoginViewModel user)
        {
            if (user == null)
            {
                return this.BadRequest("Invalid client request");
            }

            var jwtSettings = new JwtSettings
            {
                Key = this.appSettings.Value.JwtSettings.Key,
                Issuer = this.appSettings.Value.JwtSettings.Issuer,
                Audience = this.appSettings.Value.JwtSettings.Audience,
                MinutesToExpiration = int.Parse(this.appSettings.Value.JwtSettings.MinutesToExpiration),
            };

            var userContract = this.userService.Authenticate(user.UserName, user.Password);

            if (userContract != null)
            {
                this.GetToken(jwtSettings, userContract, out var userViewModel, out var tokenString);
                return this.Ok(new LoginResultData
                {
                    Token = tokenString,
                    User = userViewModel,
                });
            }
            else
            {
                return this.Unauthorized();
            }
        }

        [HttpPost]
        [Route("loginExternal")]
        public IActionResult LoginExternal([FromBody]TokenViewModel jwt)
        {
            var jwtSettings = new JwtSettings
            {
                Key = this.appSettings.Value.JwtSettings.Key,
                Issuer = this.appSettings.Value.JwtSettings.Issuer,
                Audience = this.appSettings.Value.JwtSettings.Audience,
                MinutesToExpiration = int.Parse(this.appSettings.Value.JwtSettings.MinutesToExpiration),
            };

            try
            {
                var token = new JwtSecurityToken(jwt.Token);
                var email = token.Claims.First(c => c.Type == "email");

                if (token.ValidTo > DateTime.UtcNow)
                {
                    var userContract = this.userService.AuthenticateExternal(email.Value);

                    if (userContract != null)
                    {
                        this.GetToken(jwtSettings, userContract, out var userViewModel, out var tokenString);
                        return this.Ok(new LoginResultData
                        {
                            Token = tokenString,
                            User = userViewModel,
                        });
                    }
                }

                return this.Unauthorized();
            }
            catch (Exception)
            {
                return this.Unauthorized();
            }
        }

        private void GetToken(JwtSettings jwtSettings, Domain.Services.Contracts.User.ReadedUserContract user, out ReadedUserViewModel userViewModel, out string tokenString)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: new List<Claim>
                {
                          new Claim(ClaimTypes.Name, user.Id.ToString()),
                          new Claim(ClaimTypes.Role, user.Role.ToString()),
                },
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.MinutesToExpiration),
                signingCredentials: signinCredentials);

            userViewModel = this.mapper.Map<ReadedUserViewModel>(user);
            tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return this.Ok(new { Status = "OK" });
        }
    }
}