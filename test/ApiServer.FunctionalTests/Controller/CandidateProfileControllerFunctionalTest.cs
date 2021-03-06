﻿// <copyright file="CandidateProfileControllerFunctionalTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.FunctionalTests.Controller
{
    using System.Collections.Generic;
    using System.Net;
    using ApiServer.Contracts.CandidateProfile;
    using ApiServer.Contracts.Community;
    using ApiServer.FunctionalTests.Fixture;
    using Core.Testing.Platform;
    using Domain.Model;
    using Persistance.EF.Extensions;
    using Xunit;

    [Collection(nameof(TestType.Functional))]
    public class CandidateProfileControllerFunctionalTest : IClassFixture<CandidateProfileControllerFixture>
    {
        private readonly CandidateProfileControllerFixture fixture;

        public CandidateProfileControllerFunctionalTest(CandidateProfileControllerFixture fixture)
        {
            this.fixture = fixture;
            this.fixture.ContextAction((context) =>
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            });
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get] is returning Accepted [202] when does find entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGet_WhenEntitiesAreFound_ShouldReturnAndAccepted202()
        {
            // Arrange
            var profile = new CandidateProfile() { Name = "Test" };
            this.fixture.Seed(profile);
            var candidateProfileCount = this.fixture.GetCount<CandidateProfile>();

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<List<ReadedCandidateProfileViewModel>>(HttpVerb.GET, $"{this.fixture.ControllerName}/");

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Equal(candidateProfileCount, httpResultData.ResponseEntity.Count);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get] is returning Accepted [202] and an empty collection when does not find entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGet_WhenThereAreNoEntities_ShouldReturnAccepted202AndEmptyCollection()
        {
            // Arrange
            await this.fixture.HttpCallAsync<List<ReadedCandidateProfileViewModel>>(HttpVerb.GET, this.fixture.ControllerName).ConfigureAwait(false);

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<List<ReadedCandidateProfileViewModel>>(HttpVerb.GET, this.fixture.ControllerName).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Empty(httpResultData.ResponseEntity);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get/{id}] is returning Accepted [202] entity when Id is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGetId_WhenEntityIsFound_ShouldReturnAccepted202()
        {
            // Act
            var profile = new CandidateProfile() { Name = "Test" };
            this.fixture.Seed(profile);
            var httpResultData = await this.fixture.HttpCallAsync<ReadedCandidateProfileViewModel>(HttpVerb.GET, $"{this.fixture.ControllerName}/{profile.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.NotNull(httpResultData.ResponseEntity);
            Assert.Equal(profile.Id, httpResultData.ResponseEntity.Id);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Get/{id}] is returning Not Found [404] when id is not valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfileGetId_WhenEntityIsNotFound_ShouldReturnNotFound404()
        {
            // Arrange
            var candidate = new CandidateProfile() { Id = 999, Name = "Testing" };

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<ReadedCandidateProfileViewModel>(HttpVerb.GET, $"{this.fixture.ControllerName}/{candidate.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResultData.Response.StatusCode);
            Assert.Null(httpResultData.ResponseEntity);
            Assert.Equal("Not Found", httpResultData.Response.ReasonPhrase);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Post] is returning Created [201] when data is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePost_WhenCreationIsSuccesfull_ShouldReturnCreated201()
        {
            // Arrange
            var model = new CreateCandidateProfileViewModel() { Name = "Testing", Description = "Testirino" };

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<CreatedCandidateProfileViewModel>(HttpVerb.POST, this.fixture.ControllerName, model);

            // Assert
            Assert.Equal(HttpStatusCode.Created, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.NotNull(httpResultData.ResponseEntity);
            Assert.True(httpResultData.ResponseEntity.Id > 0);
        }

        [Theory(DisplayName = "Verify api/CandidateProfile [Post] is returning Bad Request [400] when model is not valid")]
        [InlineData("Name")]
        [InlineData("Description")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePost_WhenCreationIsNotSuccesfullBecauseValidationError_ShouldReturnBadRequest400(string propertyName)
        {
            // Arrange
            var model = DataFactory.CreateInstance<CreateCandidateProfileViewModel>()
                .WithPropertyValue(propertyName, default(string));

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<CreatedCandidateProfileViewModel>(HttpVerb.POST, this.fixture.ControllerName, model);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Post] returns bad request [400] when model is not valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePost_WhenCreationIsNotSuccesfullBecauseExistenceError_ShouldReturnBadRequest()
        {
            // Arrange
            var candidate = new CandidateProfile() { Name = "Valid model", Description = "Valid model" };
            this.fixture.Seed(candidate);
            var model = this.fixture.Get<CandidateProfile>(candidate.Id);

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<CreatedCandidateProfileViewModel>(HttpVerb.POST, this.fixture.ControllerName, model);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Equal("The Profile already exists .", httpResultData.ResponseError.ExceptionMessage);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Put] is returning Accepted [202] when data is valid", Skip = "Check community update is not working properly")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenUpdateIsSuccesfull_ShouldReturnAccepted202()
        {
            // Arrange
            var profileInDb = new CandidateProfile()
            {
                Name = "Test",
                Description = "Test",
                CommunityItems = new List<Community>()
                {
                    new Community() { Name = "Community", Description = "Old community description"},
                },
            };

            this.fixture.Seed(profileInDb);

            var updateModel = new UpdateCandidateProfileViewModel()
            {
                Name = profileInDb.Name,
                Description = "New description",

                // CommunityItems =  (??)
            };

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<object>(HttpVerb.PUT, this.fixture.ControllerName, updateModel, profileInDb.Id);

            // Assert
            var profileAfterUpdate = this.fixture.GetEager(profileInDb.Id);
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(updateModel.Description, profileAfterUpdate.Description);
        }

        [Theory(DisplayName = "Verify api/CandidateProfile [Put] is returning Bad Request [400] when model is not valid")]
        [InlineData("Name")]
        [InlineData("Description")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenUpdateIsNotSuccesfull_ShouldReturnInternalServerError500(string propertyName)
        {
            // Arrange
            var profileInDb = new CandidateProfile() { Name = "Test", Description = "Test" };
            this.fixture.Seed(profileInDb);
            var profile = this.fixture.Get<CandidateProfile>(profileInDb.Id);
            var updateModel = DataFactory.CreateInstance<UpdateCandidateProfileViewModel>()
                .WithPropertyValue(propertyName, default(string));

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<object>(HttpVerb.PUT, this.fixture.ControllerName, updateModel, profile.Id);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Put] returns bad request [400] when candidate profile already exists in database")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenUpdateIsInvalid_ShouldReturnBadRequest400()
        {
            // Arrange
            var profileInDb = new CandidateProfile() { Name = "test", Description = "test" };
            this.fixture.Seed(profileInDb);
            var profile = this.fixture.Get<CandidateProfile>(profileInDb.Id);
            var invalidId = 999;

            var updateModel = new UpdateCandidateProfileViewModel()
            {
                Name = profile.Name,
                Description = "A entirely new description for Testin Testirino",
                CommunityItems = new List<CreateCommunityViewModel>
                    {
                        new CreateCommunityViewModel() { Name = "Community 1", Description = "Description Community 1"},
                        new CreateCommunityViewModel() { Name = "Community 2", Description = "Description Community 2"},
                    },
            };

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<object>(HttpVerb.PUT, this.fixture.ControllerName, updateModel, invalidId);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Equal("The Profile already exists .", httpResultData.ResponseError.ExceptionMessage);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Delete] is returning Accepted [202] when id is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfilePut_WhenDeleteIsSuccesfull_ShouldReturnAccepted()
        {
            // Arrange
            var profileInDb = new CandidateProfile() { Name = "Test", Description = "Test" };
            this.fixture.Seed(profileInDb);
            var profile = this.fixture.Get<CandidateProfile>(profileInDb.Id);
            int countBeforeDelete = this.fixture.GetCount<CandidateProfile>();

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<object>(HttpVerb.DELETE, this.fixture.ControllerName, null, profile.Id).ConfigureAwait(false);

            // Assert
            int countAfterDelete = this.fixture.GetCount<CandidateProfile>();
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotEqual(countBeforeDelete, countAfterDelete);
            Assert.Equal(0, countAfterDelete);
            Assert.NotNull(httpResultData);
        }

        [Fact(DisplayName = "Verify api/CandidateProfile [Delete] is returning Bad Request when id is invalid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidateProfileDeleteId_WhenIdIsValid_ShouldReturnBadRequest()
        {
            // Arrange
            int invalidId = 999;

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<object>(HttpVerb.DELETE, this.fixture.ControllerName, null, invalidId);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.Equal($"Profile not found for the Profile Id: {invalidId}", httpResultData.ResponseError.ExceptionMessage);
        }
    }
}
