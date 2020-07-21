using ApiServer.Contracts.CandidateProfile;
using ApiServer.Contracts.Community;
using Domain.Model;
using Persistance.EF.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ApiServer.FunctionalTests.Fixture;
using Core.Testing.Platform;
using Xunit;

namespace ApiServer.FunctionalTests.Controller
{
    [Collection(nameof(TestType.Functional))]
    public class CommunityControllerFunctionalTest : IClassFixture<CommunityControllerFixture>
    {
        private readonly CommunityControllerFixture _fixture;
        public CommunityControllerFunctionalTest(CommunityControllerFixture fixture)
        {
            _fixture = fixture;
            //_fixture.CleanTestingDatabase();
        }

        //[Fact(DisplayName = "Verify api/Community [Get] is returning ok [200] and collection of entities when found in database")]
        //[Trait("Category", "Functional-Test")]
        //public async System.Threading.Tasks.Task GivenCommunityGet_WhenThereAreEntitiesInDatabase_ShouldRetrieveCollectionAndAccepted202()
        //{
        //    //Arrange
        //    var community = new Community() { Name = "Test community", Profile = new CandidateProfile() { Name = "Candidate Profile" } };
        //    _fixture.Seed(community);

        //    //Act
        //    var httpResultData = await _fixture.HttpCallAsync<List<ReadedCommunityViewModel>>(HttpVerb.GET, $"{_fixture.ControllerName}");

        //    //Assert
        //    Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
        //    Assert.Single(httpResultData.ResponseEntity);
        //    Assert.Equal(community.Id, httpResultData.ResponseEntity.Single().Id);
        //}

        //[Fact(DisplayName = "verify api/community [get] is returning ok [200] and empty collection when there are no entities in database")]
        //[Trait("category", "functional-test")]
        //public async System.Threading.Tasks.Task GivenCommunityGet_WhenThereAreNoEntitiesInDB_ShouldReturnEmptyCollectionAndAccepted202()
        //{
        //    //Act
        //    var httpResultData = await _fixture.HttpCallAsync<List<ReadedCommunityViewModel>>(HttpVerb.GET, $"{_fixture.ControllerName}");

        //    //Assert
        //    Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
        //    Assert.Empty(httpResultData.ResponseEntity);
        //}

        [Fact(DisplayName = "Verify api/Community [Get/{Id}] is returning ok [200] and matching entity when call is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityGetId_WhenThereIsAMatchingEntityInDatabase_ShouldRetrieveItAndReturnAccepted202()
        {
            //Arrange
            var community = new Community() { Name = "Test community", Profile = new CandidateProfile() { Name = "Candidate Profile" } };
            _fixture.Seed(community);

            //Act
            var httpResultData = await _fixture.HttpCallAsync<ReadedCommunityViewModel>(HttpVerb.GET, $"{_fixture.ControllerName}/{community.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(community.Id, httpResultData.ResponseEntity.Id);
        }

        [Fact(DisplayName = "Verify api/Community [Get/{Id}] is returning Not Found [404] and entity Id when entity is not found")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityGetId_WhenThereIsNotAMatchingEntityInDatabase_ShouldReturnNotFound404()
        {
            //Arrange
            int invalidId = 999;

            //Act
            var httpResultData = await _fixture.HttpCallAsync<ReadedCommunityViewModel>(HttpVerb.GET, $"{_fixture.ControllerName}/{invalidId}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResultData.Response.StatusCode);
            Assert.Equal("Not Found", httpResultData.Response.ReasonPhrase);
        }

        [Fact(DisplayName = "Verify api/Community [Post] is returning Created [201] and new entity when data is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityPost_WhenViewmodelPostedIsValid_ShouldReturnEntityAndCreated201()
        {
            //Arrange
            var profile = new CandidateProfile() { Name = "Testing" };
            _fixture.Seed(profile);
            var profileVm = new CreateCandidateProfileViewModel() { Name = "Rodrigo" };
            var model = new CreateCommunityViewModel()
            {
                Name = "Testing",
                Description = "Testing",
                Profile = profileVm,
                ProfileId = profile.Id
            };

            //Act
            var httpResultData = await _fixture.HttpCallAsync<CreatedCommunityViewModel>(HttpVerb.POST, $"{_fixture.ControllerName}", model);

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
            var profileVm = new CreateCandidateProfileViewModel() { Name = "Rodrigo" };
            var model = new CreateCommunityViewModel()
            {
                Name = "Testing",
                Description = "Testing",
                Profile = profileVm,
                ProfileId = 1
            };

            model.WithPropertyValue(propertyName, null);

            //Act
            var httpResultData = await _fixture.HttpCallAsync<CreatedCommunityViewModel>(HttpVerb.POST, $"{_fixture.ControllerName}", model);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.Equal("Bad Request", httpResultData.Response.ReasonPhrase);
            Assert.True(httpResultData.ResponseEntity.Id == 0);
        }

        [Fact(DisplayName = "Verify api/Community [Put] is returning Accepted [202] and return updated id")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityPut_WhenViewModelPutIsValid_ShouldReturnIdAndAccepted202()
        {
            //Arrange
            var profile = new CandidateProfile() { Name = "Testing" };
            _fixture.Seed(profile);
            var wrongCommunity = new Community() { Name = "Outdated wrong community name, should be updated", Description = "Outdated wrong description name, should be updated", ProfileId = profile.Id};
            var rightCommunity = new Community() { Name = "Valid community name, should not change on update", Description = "Valid community description, should not change on update", ProfileId = profile.Id };
            var communityList = new List<Community> { wrongCommunity, rightCommunity };
            _fixture.Seed(communityList);

            var model = new UpdateCommunityViewModel()
            {
                Name = "New Community Name",
                Description = "New Description Name",
                ProfileId = profile.Id
            };

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.PUT, $"{_fixture.ControllerName}", model, wrongCommunity.Id);

            //Assert
            var communityFromDatabase = _fixture.Get<Community>(wrongCommunity.Id);
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
            var model = new UpdateCommunityViewModel()
            {
                Name = "Test",
                Description = "Test",
                ProfileId = 1
            };

            model.WithPropertyValue(propertyName, null);

            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.PUT, $"{_fixture.ControllerName}", model, 1);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotEmpty(httpResultData.ResponseString);
        }

        //[Fact(DisplayName = "Verify api/login [Delete] deletes entity in database when Id is valid")]
        //[Trait("Category", "Functional-Test")]
        //public async System.Threading.Tasks.Task GivenCommunityDelete_WhenIdMatchesEntityInDatabase_ShouldDeleteItAndReturnAccepted202()
        //{
        //    //Arrange
        //    var profile = new CandidateProfile() { Name = "Testing" };
        //    var wrongCommunity = new Community() { Name = "Wrong community should be deleted", Description = "Wrong community should be deleted", Profile = profile };
        //    _fixture.Seed(wrongCommunity);
        //    int beforeDeleteCommunityCount = _fixture.GetCount<Community>();

        //    //Act
        //    var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.DELETE, $"{_fixture.ControllerName}", null, wrongCommunity.Id);
        //    int afterDeleteCommunityCount = _fixture.GetCount<Community>();

        //    //Assert
        //    Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
        //    Assert.Equal(1, beforeDeleteCommunityCount);
        //    Assert.Equal(0, afterDeleteCommunityCount);
        //    Assert.NotEqual(afterDeleteCommunityCount, beforeDeleteCommunityCount);
       // }

        //[Fact(DisplayName = "Verify api/login [Delete] is returning not found when there is no valid Id")]
        //[Trait("Category", "Functional-Test")]
        //public async System.Threading.Tasks.Task GivenCommunityDelete_WhenIdDoesNotMatchesEntityInDatabase_ShouldReturnException()
        //{
        //    //Arrange
        //    var invalidId = 999;

        //    //Act
        //    var beforeDeleteCommunityCount = _fixture.GetCount<Community>();
        //    var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.DELETE, $"{_fixture.ControllerName}", null, invalidId);
        //    var afterDeleteCommunityCount = _fixture.GetCount<Community>();

        //    //Assert
        //    Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
        //    Assert.Equal(beforeDeleteCommunityCount, afterDeleteCommunityCount);
        //    Assert.Equal($"Community not found for the CommunityId: {invalidId}", httpResultData.ResponseError.ExceptionMessage);
        //}

        [Fact(DisplayName = "Verify api/login [Ping] is Ok [200]")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCommunityPing_WhenIsValidCall_ShouldReturnOk200()
        {
            //Act
            var httpResultData = await _fixture.HttpCallAsync<object>(HttpVerb.GET, $"{_fixture.ControllerName}/ping");

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.Response);
        }
    }
}
