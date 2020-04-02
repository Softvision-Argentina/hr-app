using System.Collections.Generic;
using Core;
using Xunit;
using ApiServer.Contracts.CandidateProfile;
using Domain.Model;
using ApiServer.FunctionalTests.Core;
using System.Net;
using Persistance.EF.Extensions;
using System.Linq;
using ApiServer.Contracts.Community;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.FunctionalTests.Controller
{
    public class CandidateProfileControllerFunctionalTest : BaseApiTest
    {
        readonly ApiFixture _fixture;

        public CandidateProfileControllerFunctionalTest(ApiFixture fixture) : base(fixture)
        {
            _fixture = fixture;
            ControllerName = "CandidateProfile";
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get] is returning Accepted [202] when does find entities")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGet_WhenEntitiesAreFound_ShouldReturnAndAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var list = new List<CandidateProfile>()
            {
                new CandidateProfile() { Name = "Testing" },
                new CandidateProfile() { Name = "Testerino" }
            };

            Context.SeedDatabaseWith(list);

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateProfileViewModel>>(HttpVerb.GET, $"{ControllerName}/");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Equal(list.Count, httpResultData.ResponseEntity.Count);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get] is returning Accepted [202] and an empty collection when does not find entities")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGet_WhenThereAreNoEntities_ShouldReturnAccepted202AndEmptyCollection()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateProfileViewModel>>(HttpVerb.GET, ControllerName);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Empty(httpResultData.ResponseEntity);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get/{id}] is returning Accepted [202] entity when Id is valid")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGetId_WhenEntityIsFound_ShouldReturnAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidate = new CandidateProfile() { Name = "Testing" };
            Context.SeedDatabaseWith(candidate);

            //Act
            var httpResultData = await HttpCallAsync<ReadedCandidateProfileViewModel>(HttpVerb.GET, $"{ControllerName}/{candidate.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.NotNull(httpResultData.ResponseEntity);
            Assert.Equal(candidate.Id, httpResultData.ResponseEntity.Id);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get/{id}] is returning Not Found [404] when id is not valid")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGetId_WhenEntityIsNotFound_ShouldReturnNotFound404()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidate = new CandidateProfile() { Id = 1, Name = "Testing" };

            //Act
            var httpResultData = await HttpCallAsync<ReadedCandidateProfileViewModel>(HttpVerb.GET, $"{ControllerName}/{candidate.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResultData.Response.StatusCode);
            Assert.Null(httpResultData.ResponseEntity);
            Assert.Equal("Not Found", httpResultData.Response.ReasonPhrase);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Post] is returning Created [201] when data is valid")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePost_WhenCreationIsSuccesfull_ShouldReturnCreated201()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var model = new CreateCandidateProfileViewModel() { Name = "Testing", Description = "Testirino" };

            //Act
            var httpResultData = await HttpCallAsync<CreatedCandidateProfileViewModel>(HttpVerb.POST, ControllerName, model);

            //Assert
            Assert.Equal(HttpStatusCode.Created, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.NotNull(httpResultData.ResponseEntity);
            Assert.True(httpResultData.ResponseEntity.Id > 0);
        }

        [Theory(DisplayName = "Verify api/CandidateProfile [Post] is returning Bad Request [400] when model is not valid")]
        [InlineData("Name")]
        [InlineData("Description")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePost_WhenCreationIsNotSuccesfullBecauseValidationError_ShouldReturnBadRequest400(string propertyName)
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var model = DataFactory.CreateInstance<CreateCandidateProfileViewModel>()
                .WithPropertyValue(propertyName, default);

            //Act
            var httpResultData = await HttpCallAsync<CreatedCandidateProfileViewModel>(HttpVerb.POST, ControllerName, model);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Post] is returning Internal Server error [500] when model is not valid")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePost_WhenCreationIsNotSuccesfullBecauseExistenceError_ShouldReturnInternalServerError500()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var model = new CandidateProfile() { Name = "Testing", Description = "Testerino" };
            Context.SeedDatabaseWith(model);

            //Act
            var httpResultData = await HttpCallAsync<CreatedCandidateProfileViewModel>(HttpVerb.POST, ControllerName, model);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Equal("The Profile already exists .", httpResultData.ResponseError.Message);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Put] is returning Accepted [202] when data is valid")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenUpdateIsSuccesfull_ShouldReturnAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidateProfile = new CandidateProfile { Name = "Testing Testerino", Description = "Description of Testing Testerino" };
            Context.SeedDatabaseWith(candidateProfile);
            var communitiesCountBeforeUpdate = Context.Community.Count();
            var updateModel = new UpdateCandidateProfileViewModel()
            {
                Name = candidateProfile.Name,
                Description = "A entirely new description for Testin Testirino",
                CommunityItems = new List<CreateCommunityViewModel>
                {
                    new CreateCommunityViewModel() { Name = "Community 1", Description = "Description Community 1"},
                    new CreateCommunityViewModel() { Name = "Community 2", Description = "Description Community 2"}
                }
            };

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.PUT, ControllerName, updateModel, candidateProfile.Id);

            //Assert
            var candidateAfterUpdate = Context.Profiles.AsNoTracking().First();
            var communitiesCountAfterUpdate = Context.Community.AsNoTracking().Count();
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotEqual(candidateProfile.Description, candidateAfterUpdate.Description);
            Assert.NotEqual(communitiesCountBeforeUpdate, communitiesCountAfterUpdate);
            Assert.Equal(updateModel.Description, candidateAfterUpdate.Description);
            Assert.Equal(updateModel.CommunityItems.Count, communitiesCountAfterUpdate);
        }

        [Theory(DisplayName = "Verify api/CandidateProfile [Put] is returning Bad Request [400] when model is not valid")]
        [InlineData("Name")]
        [InlineData("Description")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenUpdateIsNotSuccesfull_ShouldReturnInternalServerError500(string propertyName)
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidateProfile = new CandidateProfile { Name = "Testing Testerino", Description = "Description of Testing Testerino" };
            Context.SeedDatabaseWith(candidateProfile);
            var updateModel = DataFactory.CreateInstance<UpdateCandidateProfileViewModel>()
                .WithPropertyValue(propertyName, default);

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.PUT, ControllerName, updateModel, candidateProfile.Id);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Put] is returning Internal Server Error [500] when candidate profile already exists in database")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenUpdateIsSuccesfull_ShouldReturnA3ccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidateProfile = new CandidateProfile { Name = "Testing Testerino", Description = "Description of Testing Testerino" };
            Context.SeedDatabaseWith(candidateProfile);
            int invalidId = 999;

            var updateModel = new UpdateCandidateProfileViewModel()
            {
                Name = candidateProfile.Name,
                Description = "A entirely new description for Testin Testirino",
                CommunityItems = new List<CreateCommunityViewModel>
                {
                    new CreateCommunityViewModel() { Name = "Community 1", Description = "Description Community 1"},
                    new CreateCommunityViewModel() { Name = "Community 2", Description = "Description Community 2"}
                }
            };

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.PUT, ControllerName, updateModel, invalidId);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Equal("The Profile already exists .", httpResultData.ResponseError.Message);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Delete] is returning Accepted [202] when id is valid")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenDeleteIsSuccesfull_ShouldReturnAccepted()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidateProfile = new CandidateProfile { Name = "Testing Testerino", Description = "Description of Testing Testerino" };
            Context.SeedDatabaseWith(candidateProfile);
            int countBeforeDelete = Context.Profiles.AsNoTracking().Count();

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.DELETE, ControllerName, null, candidateProfile.Id);

            //Assert
            int countAfterDelete = Context.Profiles.AsNoTracking().Count();
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotEqual(countBeforeDelete, countAfterDelete);
            Assert.Equal(0, countAfterDelete);
            Assert.NotNull(httpResultData);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Delete] is returning Invalid Server [500] when id is invalid")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidateProfileDeleteId_WhenIdIsValid_ShouldReturnAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            int invalidId = 999;

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.DELETE, ControllerName, null, invalidId);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
            Assert.Equal($"Profile not found for the Profile Id: {invalidId}", httpResultData.ResponseError.Message);
        }
    }
}
