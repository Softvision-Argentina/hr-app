using System.Net;
using Xunit;
using ApiServer.FunctionalTests.Fixture;
using Core.Testing.Platform;
using Newtonsoft.Json;
using ApiServer.Contracts.Login;
using Domain.Model;
using ApiServer.Contracts.User;

namespace ApiServer.FunctionalTests.Controller
{
    [Collection(nameof(TestType.Functional))]
    public class AuthControllerFunctionalTest : IClassFixture<AuthControllerFixture>
    {
        private readonly AuthControllerFixture _fixture;
        public AuthControllerFunctionalTest(AuthControllerFixture fixture)
        {
            _fixture = fixture;
            //_fixture.CleanTestingDatabase();
        }

        [Fact(DisplayName = "Verify api/login [Post] is returning ok [200] when data is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenValidLoginData_ShouldReturnOk()
        {
            //Arrange
            var user = new User() { Username = "rodrigo.ramirez@softvision.com", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            _fixture.Seed(user);
            var model = new LoginViewModel() { UserName = user.Username, Password = "1234" /*unhashed test password*/ };

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.POST, $"{_fixture.ControllerName}/login", model);

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
            var model = new LoginViewModel() { UserName = "Invalid username", Password = "Invalid Password" };

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.POST, $"{_fixture.ControllerName}/login", model);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        [Fact(DisplayName = "Verify api/login [Post] is returning bad request [400] when data in null")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenNullLoginData_ShouldReturnBadRequest()
        {
            //Arrange
            LoginViewModel model = null;

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.POST, $"{_fixture.ControllerName}/login", model);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        [Fact(DisplayName = "Verify api/loginExternal [Post] is returning Ok [200] when valid token is provided")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenLoginExternal_WhenClientSendValidToken_ShouldReturnOk()
        {
            //Arrange
            var user = new User() { Username = "rodrigo.ramirez@softvision.com", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            _fixture.Seed(user);
            const string expectedUsername = "rodrigo.ramirez@softvision.com";
            const string expectedRole = "Admin";
            var token = _fixture.GetTestToken(AuthControllerFixture.TokenType.Valid);

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.POST, $"{_fixture.ControllerName}/loginExternal", token);
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
            var user = new User() { Username = "rodrigo.ramirez@softvision.com", Password = "03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4" };
            _fixture.Seed(user);
            TokenViewModel token = _fixture.GetTestToken(AuthControllerFixture.TokenType.Expired);

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.POST, $"{_fixture.ControllerName}/loginExternal", token);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        [Fact(DisplayName = "Verify api/ping [Get] is returning ok [200]")]
        [Trait("Category", "Functional-Test")]

        public async System.Threading.Tasks.Task GivenPing_ShouldReturnOk()
        {
            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.GET, $"{_fixture.ControllerName}/ping");

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }
    }
}


