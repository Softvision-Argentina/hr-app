using ApiServer.Contracts.Login;
using ApiServer.FunctionalTests.Controller.Builder;
using ApiServer.FunctionalTests.Core;
using Core;
using Domain.Model;
using Persistance.EF.Extensions;
using Domain.Services.ExternalServices;
using Domain.Services.ExternalServices.Config;
using System.Net;
using Xunit;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using Newtonsoft.Json;
using ApiServer.Contracts.User;

namespace ApiServer.FunctionalTests.Controller
{
    [Collection("Api collection")]
    public class AuthControllerFunctionalTest : BaseApiTest
    {
        public AuthControllerFunctionalTest(ApiFixture apiFixture) : base(apiFixture)
        {
            ControllerName = "Auth";
        }

        [Fact(DisplayName = "Verify api/login [Post] is returning ok when data is valid")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenValidLoginData_ShouldReturnOk()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var user = new User { Username = "nicolas.roldan@softvision.com", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            Context.Users.Add(user);
            Context.SaveChanges();

            var model = new LoginViewModelBuilder().GetValidData();

            //Act
            HttpResultData httpResultData = await HttpCallAsync(HttpVerb.POST, model, ControllerName, "login");

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
            Assert.NotEmpty(httpResultData.ResponseString);
        }

        [Fact(DisplayName = "Verify api/login [Post] is returning unauthorized when data is invalid")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenInvalidLoginData_ShouldReturnUnauthorized()
        {
            //Arrange
            var model = new LoginViewModelBuilder().GetInvalidData();

            //Act
            HttpResultData httpResultData = await HttpCallAsync(HttpVerb.POST, model, ControllerName, "login");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        [Fact(DisplayName = "Verify api/login [Post] is returning bad request when data in null")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenNullLoginData_ShouldReturnBadRequest()
        {
            //Arrange
            LoginViewModel model = null;

            //Act
            HttpResultData httpResultData = await HttpCallAsync(HttpVerb.POST, model, ControllerName, "login");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        [Fact(DisplayName = "Verify api/loginExternal [Post] is returning Ok when valid token is provided")]
        public async Task GivenLoginExternal_WhenClientSendValidToken_ShouldReturnOk()
        {
            //here should be the SqlHelper.SetupDatabaseForTesting helper method that wipes all data from database
            string expectedUsername = "rodrigo.ramirez@softvision.com";
            string expectedRole = "Admin";
            TokenViewModel token = GetTestToken(TokenType.Valid);
            Context.Users.Add(new Domain.Model.User { Username = "rodrigo.ramirez@softvision.com", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" });
            Context.SaveChanges();
            
            HttpResultData httpResultData = await HttpCallAsync(HttpVerb.POST, token, ControllerName, "loginExternal");
            SuccessAuthData successAuthData = JsonConvert.DeserializeObject<SuccessAuthData>(httpResultData.ResponseString);

            Assert.Equal(expectedUsername, successAuthData.User.Username);
            Assert.Equal(expectedRole, successAuthData.User.Role);
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
            Assert.NotEmpty(httpResultData.ResponseString);
        }

        [Fact(DisplayName = "Verify api/loginExternal [Post] is returning Unauthorized when token expired")]
        public async Task GivenLoginExternal_WhenClientSendExpiredToken_ShouldReturnUnauthorized()
        {
            //here should be the SqlHelper.SetupDatabaseForTesting helper method that wipes all data from database
            TokenViewModel token = GetTestToken(TokenType.Invalid);
            Context.Users.Add(new Domain.Model.User { Username = "rodrigo.ramirez@softvision.com", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" });
            Context.SaveChanges();

            HttpResultData httpResultData = await HttpCallAsync(HttpVerb.POST, token, ControllerName, "loginExternal");

            Assert.Equal(HttpStatusCode.Unauthorized, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        [Fact(DisplayName = "Verify api/ping [Get] is returning ok")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenPing_ShouldReturnOk()
        {
            //Arrange
            LoginViewModel model = null;

            //Act
            HttpResultData httpResultData = await HttpCallAsync(HttpVerb.GET, model, ControllerName, "ping");

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        enum TokenType{
            Valid,
            Invalid
        }

        private TokenViewModel GetTestToken(TokenType tokenType)
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
    }
}
