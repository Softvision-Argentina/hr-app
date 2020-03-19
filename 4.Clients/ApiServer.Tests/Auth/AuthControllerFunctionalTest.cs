using ApiServer.Contracts.Login;
using ApiServer.Tests.Candidates.Builder;
using Core;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ApiServer.Tests.Seed
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
        public async Task GivenValidLoginData_ShouldReturnOk()
        {
            //Arrange
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
        public async Task GivenInvalidLoginData_ShouldReturnUnauthorized()
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
        public async Task GivenNullLoginData_ShouldReturnBadRequest()
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
        public async Task GivenPing_ShouldReturnOk()
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
