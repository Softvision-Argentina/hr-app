using ApiServer.Contracts.CandidateProfile;
using ApiServer.Contracts.Community;
using ApiServer.FunctionalTests.Core;
using Core;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace ApiServer.FunctionalTests.Controller
{
    public class CommunityControllerFunctionalTest : BaseApiTest
    {
        public CommunityControllerFunctionalTest(ApiFixture apiFixture) : base(apiFixture)
        {
            ControllerName = "Community";
        }

        [Fact(DisplayName = "Verify api/Community [Get] is returning ok [200] and collection of entities when found in database")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityGet_WhenThereAreEntitiesInDatabase_ShouldRetrieveCollectionAndAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var community = new Community() { Name = "Test community", Profile = new CandidateProfile() { Name = "Candidate Profile" } };
            Context.SeedDatabaseWith(community);

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCommunityViewModel>>(HttpVerb.GET, $"{ControllerName}");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Single(httpResultData.ResponseEntity);
            Assert.Equal(community.Id, httpResultData.ResponseEntity.Single().Id);
        }

        [Fact(DisplayName = "Verify api/Community [Get] is returning ok [200] and empty collection when there are no entities in database")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityGet_WhenThereAreNotEntitiesInDatabase_ShouldReturnEmptyCollectionAndAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCommunityViewModel>>(HttpVerb.GET, $"{ControllerName}");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Empty(httpResultData.ResponseEntity);
        }

        [Fact(DisplayName = "Verify api/Community [Get/{Id}] is returning ok [200] and matching entity when call is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityGetId_WhenThereIsAMatchingEntityInDatabase_ShouldRetrieveItAndReturnAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var community = new Community() { Name = "Test community", Profile = new CandidateProfile() { Name = "Candidate Profile" } };
            Context.SeedDatabaseWith(community);

            //Act
            var httpResultData = await HttpCallAsync<ReadedCommunityViewModel>(HttpVerb.GET, $"{ControllerName}/{community.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(community.Id, httpResultData.ResponseEntity.Id);
        }

        [Fact(DisplayName = "Verify api/Community [Get/{Id}] is returning Not Found [404] and entity Id when entity is not found")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityGetId_WhenThereIsNotAMatchingEntityInDatabase_ShouldReturnNotFound404()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            int invalidId = 999;

            //Act
            var httpResultData = await HttpCallAsync<ReadedCommunityViewModel>(HttpVerb.GET, $"{ControllerName}/{invalidId}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResultData.Response.StatusCode);
            Assert.Equal("Not Found", httpResultData.Response.ReasonPhrase);
        }

        [Fact(DisplayName = "Verify api/Community [Post] is returning Created [201] and new entity when data is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityPost_WhenViewmodelPostedIsValid_ShouldReturnEntityAndCreated201()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var profile = new CandidateProfile() { Name = "Testing" };
            Context.SeedDatabaseWith(profile);
            var profileVm = new CreateCandidateProfileViewModel() { Name = "Rodrigo" };
            var model = new CreateCommunityViewModel()
            {
                Name = "Testing",
                Description = "Testing",
                Profile = profileVm,
                ProfileId = profile.Id
            };

            //Act
            var httpResultData = await HttpCallAsync<CreatedCommunityViewModel>(HttpVerb.POST, $"{ControllerName}", model);

            //Assert
            Assert.Equal(HttpStatusCode.Created, httpResultData.Response.StatusCode);
            Assert.True(httpResultData.ResponseEntity.Id > 0);
        }

        [Theory(DisplayName = "Verify api/Community [Post] is returning error when viewmodel posted is not valid")]
        [Trait("Category", "Functional-Test")]
        [InlineData("Name")]
        [InlineData("Description")]
        public async System.Threading.Tasks.Task GivenCommunityPost_WhenViewmodelPostedIsInvalid_ShouldReturnErrorBadRequest400(string propertyName)
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var profileVm = new CreateCandidateProfileViewModel() { Name = "Rodrigo" };
            var model = new CreateCommunityViewModel()
            {
                Name = "Testing",
                Description = "Testing",
                Profile = profileVm,
                ProfileId = 1
            };

            model.WithPropertyValue(propertyName, default);

            //Act
            var httpResultData = await HttpCallAsync<CreatedCommunityViewModel>(HttpVerb.POST, $"{ControllerName}", model);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.Equal("Bad Request", httpResultData.Response.ReasonPhrase);
            Assert.True(httpResultData.ResponseEntity.Id == default);
        }

        [Fact(DisplayName = "Verify api/Community [Put] is returning Accepted [202] and return updated id")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityPut_WhenViewModelPutIsValid_ShouldReturnIdAndAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var profile = new CandidateProfile() { Name = "Testing" };
            var wrongCommunity = new Community() { Name = "Outdated wrong community name, should be updated", Description = "Outdated wrong description name, should be updated", Profile = profile };
            var rightCommunity = new Community() { Name = "Valid community name, should not change on update", Description = "Valid community description, should not change on update", Profile = profile };
            
            Context.SeedDatabaseWith(new List<Community> { wrongCommunity, rightCommunity });

            var model = new UpdateCommunityViewModel()
            {
                Name = "New Community Name",
                Description = "New Description Name",
                ProfileId = profile.Id,
                //Profile = new UpdateCandidateProfileViewModel() { Name = "This profile would be created", Description = "This profile would be created" }
            };

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.PUT, $"{ControllerName}", model, wrongCommunity.Id);

            //Assert
            var communityFromDatabase = Context.Community.AsNoTracking().Where(_ => _.Id == wrongCommunity.Id).First();
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(model.Name, communityFromDatabase.Name);
            Assert.Equal(model.Description, communityFromDatabase.Description);
        }

        [Theory(DisplayName = "Verify api/Community [Put] is returning error Bad Request 400 when viewmodel is not valid")]
        [Trait("Category", "Functional-Test")]
        [InlineData("Name")]
        [InlineData("Description")]
        public async System.Threading.Tasks.Task GivenCommunityPut_WhenViewModelPutIsNotValid_ShouldReturnErrorBadRequest400(string propertyName)
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var model = new UpdateCommunityViewModel()
            {
                Name = "Test",
                Description = "Test",
                ProfileId = 1
            };

            model.WithPropertyValue(propertyName, default);

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.PUT, $"{ControllerName}", model, 1);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotEmpty(httpResultData.ResponseString);
        }

        [Fact(DisplayName = "Verify api/login [Delete] deletes entity in database when Id is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityDelete_WhenIdMatchesEntityInDatabase_ShouldDeleteItAndReturnAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var profile = new CandidateProfile() { Name = "Testing" };
            var wrongCommunity = new Community() { Name = "Wrong community should be deleted", Description = "Wrong community should be deleted", Profile = profile };
            Context.SeedDatabaseWith(wrongCommunity);
            int beforeDeleteCommunityCount = GetCommunityCount();

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.DELETE, $"{ControllerName}", null, wrongCommunity.Id);
            int afterDeleteCommunityCount = GetCommunityCount();

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(1, beforeDeleteCommunityCount);
            Assert.Equal(0, afterDeleteCommunityCount);
            Assert.NotEqual(afterDeleteCommunityCount, beforeDeleteCommunityCount);
        }

        [Fact(DisplayName = "Verify api/login [Delete] is returning not found when there is no valid Id")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityDelete_WhenIdDoesNotMatchesEntityInDatabase_ShouldReturnException()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            int invalidId = 999;

            //Act
            int beforeDeleteCommunityCount = GetCommunityCount();
            var httpResultData = await HttpCallAsync<object>(HttpVerb.DELETE, $"{ControllerName}", null, invalidId);
            int afterDeleteCommunityCount = GetCommunityCount();

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
            Assert.Equal(beforeDeleteCommunityCount, afterDeleteCommunityCount);
            Assert.Equal($"Community not found for the CommunityId: {invalidId}", httpResultData.ResponseError.Message);
        }

        [Fact(DisplayName = "Verify api/login [Ping] is Ok [200]")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityPing_WhenIsValidCall_ShouldReturnOk200()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.GET, $"{ControllerName}/ping");

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }

        private int GetCommunityCount()
        {
            return Context.Community.AsNoTracking().Count();
        }
    }
}
