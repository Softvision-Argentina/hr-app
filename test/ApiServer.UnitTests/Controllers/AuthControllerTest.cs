using ApiServer.Contracts.Login;
using ApiServer.Contracts.User;
using ApiServer.Controllers;
using AutoMapper;
using Core;
using Core.ExtensionHelpers;
using Domain.Services.Contracts.User;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Domain.Model;
using Microsoft.Extensions.Options;

namespace ApiServer.UnitTests.Controllers
{
    public class AuthControllertTest
    {
        private AuthController controller;
        private Mock<IUserService> mockService;
        private Mock<IConfiguration> mockConfig;
        private Mock<IMapper> mockMapper;
        private Mock<IOptions<AppSettings>> mockAppsettings;


        public AuthControllertTest()
        {
            mockService = new Mock<IUserService>();
            mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<ReadedUserViewModel>(It.IsAny<ReadedUserContract>())).Returns(new ReadedUserViewModel());
            SetUpAppSettingsMock();
            controller = new AuthController(mockService.Object, mockMapper.Object, mockAppsettings.Object);
        }

        [Fact(DisplayName = "Verify that method 'Login' returns ActionResult when data is valid")]
        public void Should_Login_When_DataIsValid()
        {
            var testUser = new LoginViewModel();
            testUser.UserName = "testUser";
            testUser.Password = "1234";
            var expectedValue = new ReadedUserViewModel();

            mockService.Setup(_ => _.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new ReadedUserContract());

            var result = controller.Login(testUser);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var resultUserData = ((result as OkObjectResult).Value as LoginResultData).User;
            var resultAsJson = JsonConvert.SerializeObject(resultUserData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.Authenticate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'Login' returns Unauthorized when data is invalid")]
        public void Should_Login_When_DataIsInvalid()
        {
            var testUser = new LoginViewModel();
            testUser.UserName = "testUser";
            testUser.Password = "12345";
            var expectedStatusCode = 401;

            var result = controller.Login(testUser);

            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(expectedStatusCode, (result as UnauthorizedResult).StatusCode);
            mockService.Verify(_ => _.Authenticate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'LoginExternal' returns ActionResult when data is valid")]
        public void Should_LoginExternal_When_DataIsValid()
        {
            var testToken = new TokenViewModel();   

            //testToken = {"email": "testMail", "exp": 4079184853} --> exp = 6/4/2099
            testToken.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRlc3RNYWlsIiwiZXhwIjo0MDc5MTg0ODUzfQ.rkWON_tlcNaMZDlHCpFFdBAdfbw94LpmI1SU4NRNgNM";
            var expectedValue = new ReadedUserViewModel();

            mockService.Setup(_ => _.AuthenticateExternal(It.IsAny<string>())).Returns(new ReadedUserContract());

            var result = controller.LoginExternal(testToken);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var resultUserData = ((result as OkObjectResult).Value as LoginResultData).User;
            var resultAsJson = JsonConvert.SerializeObject(resultUserData);
            var expectedValueAsJson = JsonConvert.SerializeObject(expectedValue);

            Assert.Equal(expectedValueAsJson, resultAsJson);
            mockService.Verify(_ => _.AuthenticateExternal(It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that method 'LoginExternal' returns NotFoundObjectResult when data is invalid")]
        public void Should_LoginExternal_When_DataIsInvalid()
        {
            var testToken = new TokenViewModel();

            //testToken = {"email": "testMail", "exp": 1554576853} --> exp = 6/4/2019
            testToken.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InRlc3RNYWlsIiwiZXhwIjoxNTU0NTc2ODUzfQ.rITeTYpG1YW5M1egFSl-oNp7eI5b6tFIQlMwmqGoTeQ";
            var expectedStatusCode = 401;

            var result = controller.LoginExternal(testToken);

            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(expectedStatusCode, (result as UnauthorizedResult).StatusCode);
            mockService.Verify(_ => _.AuthenticateExternal(It.IsAny<string>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that method 'Ping' returns OkObjectResult")]
        public void Should_Ping()
        {
            var result = controller.Ping();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(new { Status = "OK" }.ToQueryString(), (result as OkObjectResult).Value.ToQueryString());
        }

        private void SetUpAppSettingsMock()
        {
            AppSettings configSettings = new AppSettings();
            var jwtSettings = new ConfigurationKeys.JwtSettings() {
                Audience = "http://localhost:61059", 
                Issuer = "http://localhost:61059",
                Key = "Eh1aFJbPXs6jTCo2JdyyR2FEUzdeiXBLA18tWG0jF7A_PFZ6qt9B6_HwkaUfgA14ErHHp-VKWBmUZTtTs7wWrcbw1rynto1RqQnKLtqM4dcOd7SKkl9wi18e-TDf8DovC7teVZQhXd2uAEZxtCI9C6YG3i6T2fXeqddf52sElqpEufpAuthnqDh35A2TVzRZr90bIhUhb6YywxWV1J0-mUxjZX-lI6tDq5d3A5jOZN2Q2STshLOAd5y5sGm1cDKCtWy3TiJ9Nxjj4_yfEABMHxm5kOrtD8gLE6Fa5VvnlKKWvW-BKJyHjv7EcWlzeQhqr_ZhTjOxK6e_7vvR8FXx0Q", 
                MinutesToExpiration = "60" };
            configSettings.JwtSettings = jwtSettings;
            IOptions<AppSettings> appSettings = Options.Create(configSettings);
            this.mockAppsettings = new Mock<IOptions<AppSettings>>();
            mockAppsettings.Setup(x => x.Value).Returns(configSettings);
        }
    }
}