using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ApiServer.Contracts.Login;
using ApiServer.Contracts.User;
using AutoMapper;
using Domain.Services.ExternalServices.Config;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(
            IConfiguration configuration, 
            IUserService userService, 
            IMapper mapper)
        {
            this._configuration = configuration;
            this._userService = userService;
            this._mapper = mapper;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginViewModel user)
        {
            if (user == null)
                return BadRequest("Invalid client request");

            var jwtSettings = new JwtSettings
            {
                Key = _configuration["jwtSettings:key"],
                Issuer = _configuration["jwtSettings:issuer"],
                Audience = _configuration["jwtSettings:audience"],
                MinutesToExpiration = int.Parse(_configuration["jwtSettings:minutesToExpiration"])
            };

            var userContract = _userService.Authenticate(user.UserName, user.Password);

            if (userContract != null)
            {
                GetToken(jwtSettings, userContract, out var userViewModel, out var tokenString);
                return Ok(new { Token = tokenString, User = userViewModel });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost, Route("loginExternal")]
        public IActionResult LoginExternal([FromBody]TokenViewModel jwt)
        {
            var jwtSettings = new JwtSettings
            {
                Key = _configuration["jwtSettings:key"],
                Issuer = _configuration["jwtSettings:issuer"],
                Audience = _configuration["jwtSettings:audience"],
                MinutesToExpiration = int.Parse(_configuration["jwtSettings:minutesToExpiration"])
            };

            try
            {
                var token = new JwtSecurityToken(jwt.Token);
                var email = token.Claims.First(c => c.Type == "email");
                
                if (token.ValidTo > DateTime.UtcNow)
                {
                    var userContract = this._userService.Authenticate(email.Value);

                    if (userContract != null)
                    {
                        GetToken(jwtSettings, userContract, out var userViewModel, out var tokenString);
                        return Ok(new { Token = tokenString, User = userViewModel });
                    }
                }

                return Unauthorized();
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        private void GetToken(JwtSettings jwtSettings, Domain.Services.Contracts.User.ReadedUserContract _user, out ReadedUserViewModel userViewModel, out string tokenString)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: new List<Claim> {
                          new Claim(ClaimTypes.Name, _user.Id.ToString()),
                          new Claim(ClaimTypes.Role, _user.Role.ToString())
                },
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.MinutesToExpiration),
                signingCredentials: signinCredentials
            );

            userViewModel = _mapper.Map<ReadedUserViewModel>(_user);
            tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }


        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { Status = "OK" });
        }
    }
}