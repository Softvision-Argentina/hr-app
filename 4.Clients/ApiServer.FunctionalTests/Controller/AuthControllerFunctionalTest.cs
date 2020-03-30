using ApiServer.Contracts.Login;
using ApiServer.FunctionalTests.Controller.Builder;
using ApiServer.FunctionalTests.Core;
using Core;
using Domain.Model;
using Persistance.EF.Extensions;
using System.Net;
using Xunit;

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
    }
}
