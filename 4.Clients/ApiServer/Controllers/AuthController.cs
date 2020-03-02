using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiServer.Contracts.Login;
using ApiServer.Contracts.User;
using AutoMapper;
using Domain.Services.ExternalServices.Config;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public IConfiguration _configuration { get; }
        readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(IConfiguration configuration, IUserService userService, IMapper mapper)
        {
            this._configuration = configuration;
            this._userService = userService;
            this._mapper = mapper;
        }

        // GET api/values
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginViewModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            var jwtSettings = new JwtSettings
            {
                Key = _configuration["jwtSettings:key"],
                Issuer = _configuration["jwtSettings:issuer"],
                Audience = _configuration["jwtSettings:audience"],
                MinutesToExpiration = int.Parse(_configuration["jwtSettings:minutesToExpiration"])
            };

            var _user = _userService.Authenticate(user.UserName, user.Password);

            if (_user != null)
            {
                ReadedUserViewModel userViewModel;
                string tokenString;
                GetToken(jwtSettings, _user, out userViewModel, out tokenString);
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
                var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(jwt.Token);
                var email = token.Claims.First(c => c.Type == "email");
                var exp = token.Claims.First(c => c.Type == "exp");
                //var emailVerified = token.Claims.First(c => c.Type == "email_verified");
                if (token.ValidTo > DateTime.UtcNow)
                {
                    var _user = this._userService.Authenticate(email.Value);

                    if (_user != null)
                    {
                        ReadedUserViewModel userViewModel;
                        string tokenString;
                        GetToken(jwtSettings, _user, out userViewModel, out tokenString);
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
                expires: DateTime.Now.AddMinutes(jwtSettings.MinutesToExpiration),
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