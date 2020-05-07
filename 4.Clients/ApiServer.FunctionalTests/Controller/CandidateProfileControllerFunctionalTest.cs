using System.Collections.Generic;
using Core;
using Xunit;
using ApiServer.Contracts.CandidateProfile;
using Domain.Model;
using System.Net;
using ApiServer.Contracts.Community;
using ApiServer.FunctionalTests.Fixture;
using Core.Persistance;
using Persistance.EF.Extensions;

namespace ApiServer.FunctionalTests.Controller
{
    [Collection(nameof(EnvironmentType.Functional))]
    public class CandidateProfileControllerFunctionalTest : IClassFixture<CandidateProfileControllerFixture>
    {
        private readonly CandidateProfileControllerFixture _fixture;

        public CandidateProfileControllerFunctionalTest(CandidateProfileControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get] is returning Accepted [202] when does find entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGet_WhenEntitiesAreFound_ShouldReturnAndAccepted202()
        {
            //Arrange
            var profile = new CandidateProfile() { Name = "Test" };
            await _fixture.SeedAsync(profile);
            var candidateProfileCount = await _fixture.GetCountAsync<CandidateProfile>();

            //Act
            var httpResultData = await _fixture.HttpCallAsync<List<ReadedCandidateProfileViewModel>>(HttpVerb.GET, $"{_fixture.ControllerName}/");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Equal(candidateProfileCount, httpResultData.ResponseEntity.Count);

            //Clean
            await _fixture.DeleteAsync<CandidateProfile>();
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get] is returning Accepted [202] and an empty collection when does not find entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGet_WhenThereAreNoEntities_ShouldReturnAccepted202AndEmptyCollection()
        {
            //Act
            var httpResultData = await _fixture.HttpCallAsync<List<ReadedCandidateProfileViewModel>>(HttpVerb.GET, _fixture.ControllerName);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Empty(httpResultData.ResponseEntity);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get/{id}] is returning Accepted [202] entity when Id is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGetId_WhenEntityIsFound_ShouldReturnAccepted202()
        {
            //Act
            var profile = new CandidateProfile() { Name = "Test" };
            await _fixture.SeedAsync(profile);
            var httpResultData = await _fixture.HttpCallAsync<ReadedCandidateProfileViewModel>(HttpVerb.GET, $"{_fixture.ControllerName}/{profile.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.NotNull(httpResultData.ResponseEntity);
            Assert.Equal(profile.Id, httpResultData.ResponseEntity.Id);
            
            //Clean
            await _fixture.DeleteAsync<CandidateProfile>();
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get/{id}] is returning Not Found [404] when id is not valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGetId_WhenEntityIsNotFound_ShouldReturnNotFound404()
        {
            //Arrange
            var candidate = new CandidateProfile() { Id = 999, Name = "Testing" };

            //Act
            var httpResultData = await _fixture.HttpCallAsync<ReadedCandidateProfileViewModel>(HttpVerb.GET, $"{_fixture.ControllerName}/{candidate.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResultData.Response.StatusCode);
            Assert.Null(httpResultData.ResponseEntity);
            Assert.Equal("Not Found", httpResultData.Response.ReasonPhrase);
        }

        
        [Fact(DisplayName = "Verify api/CandidateProfile [Post] is returning Created [201] when data is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePost_WhenCreationIsSuccesfull_ShouldReturnCreated201()
        {
            //Arrange
            var model = new CreateCandidateProfileViewModel() { Name = "Testing", Description = "Testirino" };

            //Act
            var httpResultData = await _fixture.HttpCallAsync<CreatedCandidateProfileViewModel>(HttpVerb.POST, _fixture.ControllerName, model);

            //Assert
            Assert.Equal(HttpStatusCode.Created, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.NotNull(httpResultData.ResponseEntity);
            Assert.True(httpResultData.ResponseEntity.Id > 0);

            //Clean
            await _fixture.DeleteAsync<CandidateProfile>();
        }

        [Theory(DisplayName = "Verify api/CandidateProfile [Post] is returning Bad Request [400] when model is not valid")]
        [InlineData("Name")]
        [InlineData("Description")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePost_WhenCreationIsNotSuccesfullBecauseValidationError_ShouldReturnBadRequest400(string propertyName)
        {
            //Arrange
            var model = DataFactory.CreateInstance<CreateCandidateProfileViewModel>()
                .WithPropertyValue(propertyName, default);

            //Act
            var httpResultData = await _fixture.HttpCallAsync<CreatedCandidateProfileViewModel>(HttpVerb.POST, _fixture.ControllerName, model);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Post] is returning Internal Server error [500] when model is not valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePost_WhenCreationIsNotSuccesfullBecauseExistenceError_ShouldReturnInternalServerError500()
        {
            //Arrange
            var candidate = new CandidateProfile() { Name = "Valid model", Description = "Valid model" };
            await _fixture.SeedAsync(candidate);
            var model = await _fixture.GetAsync<CandidateProfile>(candidate.Id);

            //Act
            var httpResultData = await _fixture.HttpCallAsync<CreatedCandidateProfileViewModel>(HttpVerb.POST, _fixture.ControllerName, model);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Equal("The Profile already exists .", httpResultData.ResponseError.Message);

            //Clean
            await _fixture.DeleteAsync<CandidateProfile>();
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Put] is returning Accepted [202] when data is valid", Skip = "Check community update is not working properly")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenUpdateIsSuccesfull_ShouldReturnAccepted202()
        {
            //Arrange
            var profileInDb = new CandidateProfile()
            {
                Name = "Test",
                Description = "Test",
                CommunityItems = new List<Community>()
                {
                    new Community() { Name = "Community", Description = "Old community description"}
                }
            };

            await _fixture.SeedAsync(profileInDb);

            var updateModel = new UpdateCandidateProfileViewModel()
            {
                Name = profileInDb.Name,
                Description = "New description",
                //CommunityItems =  (??)
            };

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.PUT, _fixture.ControllerName, updateModel, profileInDb.Id);

            //Assert
            var profileAfterUpdate = (CandidateProfile)await _fixture.GetEagerAsync(profileInDb.Id);
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(updateModel.Description, profileAfterUpdate.Description);
            //Assert.Equal(updateModel.CommunityItems.Single().Description, profileAfterUpdate.CommunityItems.Single().Description);

            //Clean
            _fixture.Context.DeleteAllEntities();
        }

        [Theory(DisplayName = "Verify api/CandidateProfile [Put] is returning Bad Request [400] when model is not valid")]
        [InlineData("Name")]
        [InlineData("Description")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenUpdateIsNotSuccesfull_ShouldReturnInternalServerError500(string propertyName)
        {
            //Arrange
            var profileInDb = new CandidateProfile() { Name = "Test", Description = "Test" };
            await _fixture.SeedAsync(profileInDb);
            var profile = _fixture.GetAsync<CandidateProfile>(profileInDb.Id);
            var updateModel = DataFactory.CreateInstance<UpdateCandidateProfileViewModel>()
                .WithPropertyValue(propertyName, default);

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.PUT, _fixture.ControllerName, updateModel, profile.Id);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);

            //Clean
            await _fixture.DeleteAsync<CandidateProfile>();
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Put] is returning Internal Server Error [500] when candidate profile already exists in database")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenUpdateIsSuccesfull_ShouldReturnAccepted2302()
        {
            //Arrange
            var profileInDb = new CandidateProfile() { Name = "test", Description = "test" };
            await _fixture.SeedAsync(profileInDb);
            var profile = await _fixture.GetAsync<CandidateProfile>(profileInDb.Id);
            var invalidId = 999;

            var updateModel = new UpdateCandidateProfileViewModel()
            {
                Name = profile.Name,
                Description = "A entirely new description for Testin Testirino",
                CommunityItems = new List<CreateCommunityViewModel>
                    {
                        new CreateCommunityViewModel() { Name = "Community 1", Description = "Description Community 1"},
                        new CreateCommunityViewModel() { Name = "Community 2", Description = "Description Community 2"}
                    }
            };

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.PUT, _fixture.ControllerName, updateModel, invalidId);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Equal("The Profile already exists .", httpResultData.ResponseError.Message);

            //Clean
            await _fixture.DeleteAsync<CandidateProfile>();
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Delete] is returning Accepted [202] when id is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenDeleteIsSuccesfull_ShouldReturnAccepted()
        {
            //Arrange
            var profileInDb = new CandidateProfile() { Name = "Test", Description = "Test" };
            await _fixture.SeedAsync(profileInDb);
            var profile = await _fixture.GetAsync<CandidateProfile>(profileInDb.Id);
            int countBeforeDelete = await _fixture.GetCountAsync<CandidateProfile>();

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.DELETE, _fixture.ControllerName, null, profile.Id);

            //Assert
            int countAfterDelete = await _fixture.GetCountAsync<CandidateProfile>();
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotEqual(countBeforeDelete, countAfterDelete);
            Assert.Equal(0, countAfterDelete);
            Assert.NotNull(httpResultData);

            //Clean
            await _fixture.DeleteAsync<CandidateProfile>();
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Delete] is returning Invalid Server [500] when id is invalid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfileDeleteId_WhenIdIsValid_ShouldReturnAccepted202()
        {
            //Arrange
            int invalidId = 999;

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.DELETE, _fixture.ControllerName, null, invalidId);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
            Assert.Equal($"Profile not found for the Profile Id: {invalidId}", httpResultData.ResponseError.Message);
        }
    }
}
