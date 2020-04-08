using ApiServer.Contracts.Login;
using ApiServer.FunctionalTests.Controller.Builder;
using ApiServer.FunctionalTests.Core;
using Core;
using Domain.Model;
using Domain.Services.ExternalServices;
using Domain.Services.ExternalServices.Config;
using System.Net;
using Xunit;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using Newtonsoft.Json;
using ApiServer.Contracts.User;
using Persistance.EF.Extensions;

namespace ApiServer.FunctionalTests.Controller
{
    public class AuthControllerFunctionalTest : BaseApiTest
    {
        public AuthControllerFunctionalTest(ApiFixture apiFixture) : base(apiFixture)
        {
            ControllerName = "Auth";
        }

        [Fact(DisplayName = "Verify api/login [Post] is returning ok [200] when data is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenValidLoginData_ShouldReturnOk()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var user = new User { Username = "nicolas.roldan@softvision.com", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            Context.Users.Add(user);
            Context.SaveChanges();

            var model = new LoginViewModelBuilder().GetValidData();

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.POST, $"{ControllerName}/login", model);

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
            Assert.NotEmpty(httpResultData.ResponseString);
        }

        [Fact(DisplayName = "Verify api/login [Post] is returning unauthorized [401] when data is invalid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenInvalidLoginData_ShouldReturnUnauthorized()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var model = new LoginViewModelBuilder().GetInvalidData();

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.POST, $"{ControllerName}/login", model);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        [Fact(DisplayName = "Verify api/login [Post] is returning bad request [400] when data in null")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenNullLoginData_ShouldReturnBadRequest()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            LoginViewModel model = null;

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.POST, $"{ControllerName}/login", model);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        [Fact(DisplayName = "Verify api/loginExternal [Post] is returning Ok [200] when valid token is provided")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenLoginExternal_WhenClientSendValidToken_ShouldReturnOk()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            string expectedUsername = "rodrigo.ramirez@softvision.com";
            string expectedRole = "Admin";
            var token = GetTestToken(TokenType.Valid);
            Context.Users.Add(new User { Username = "rodrigo.ramirez@softvision.com", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" });
            Context.SaveChanges();
            
            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.POST, $"{ControllerName}/loginExternal", token);
            var successAuthData = JsonConvert.DeserializeObject<SuccessAuthData>(httpResultData.ResponseString);

            //Assert
            Assert.Equal(expectedUsername, successAuthData.User.Username);
            Assert.Equal(expectedRole, successAuthData.User.Role);
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
            Assert.NotEmpty(httpResultData.ResponseString);
        }

        [Fact(DisplayName = "Verify api/loginExternal [Post] is returning Unauthorized [401] when token expired")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenLoginExternal_WhenClientSendExpiredToken_ShouldReturnUnauthorized()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            TokenViewModel token = GetTestToken(TokenType.Expired);
            Context.Users.Add(new Domain.Model.User { Username = "rodrigo.ramirez@softvision.com", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" });
            Context.SaveChanges();

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.POST, $"{ControllerName}/loginExternal", token);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        [Fact(DisplayName = "Verify api/ping [Get] is returning ok [200]")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenPing_ShouldReturnOk()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.GET, $"{ControllerName}/ping");

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        #region Auth helpers

        enum TokenType
        {
            Valid,
            Expired
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
        #endregion Auth helpers
    }
}
