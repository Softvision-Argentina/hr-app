using ApiServer.Contracts.Seed;
using ApiServer.Tests.Seed.Builder;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiServer.Tests.Seed
{
    [Collection("Api collection")]
    public class DumyControllerTests : BaseApiTest
    {
        public DumyControllerTests(ApiFixture apiFixture) : base(apiFixture)
        {
            ControllerName = "Dummies";
        }
        
        [Fact(DisplayName = "Verify [Post] is working when data is valid")]
        [Trait("Category", "API-Tasks")]
        public async Task When_CreateIsCalled_ShouldReturnSuccess()
        {
            //Arrange
            var model = new CreateDummyViewModelBuilder().Build();

            // Act
            var response = await Client.PostAsync($"/api/{ControllerName}/",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            AssertSuccess(response, responseString);
            Assert.NotEqual(Guid.Empty, JsonConvert.DeserializeObject<CreatedDummyViewModel>(responseString).Id);
        }

        [Fact(DisplayName = "Verify [Get] is working for retrieve one task")]
        [Trait("Category", "API-Tasks")]
        public async Task When_GetCalled_WithId_ShouldReturnATask()
        {
            //Arrange
            var createVm = new CreateDummyViewModelBuilder().Build();
            var model = await CreateAsync<CreateDummyViewModel, CreatedDummyViewModel>(createVm);

            //Act
            var response = await Client.GetAsync($"/api/{ControllerName}/{model.Id}");

            var responseString = await response.Content.ReadAsStringAsync();

            //Assert
            AssertSuccess(response, responseString);
            var returnedValue = JsonConvert.DeserializeObject<ReadedDummyViewModel>(responseString);
            Assert.Equal(model.Id, returnedValue.Id);
            Assert.Equal(createVm.Name, returnedValue.Name);
        }
    }
}
