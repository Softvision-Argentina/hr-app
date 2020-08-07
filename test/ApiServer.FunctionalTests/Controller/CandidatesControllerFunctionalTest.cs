// <copyright file="CandidatesControllerFunctionalTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Net;
using ApiServer.Contracts.CandidateProfile;
using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.Community;
using ApiServer.Contracts.User;
using ApiServer.FunctionalTests.Fixture;
using Core.Testing.Platform;
using Domain.Model;
using Persistance.EF.Extensions;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace ApiServer.FunctionalTests.Controller
{
    [Collection(nameof(TestType.Functional))]

    public class CandidatesControllerFunctionalTest : IClassFixture<CandidatesControllerFixture>
    {
        private readonly CandidatesControllerFixture fixture;

        public CandidatesControllerFunctionalTest(CandidatesControllerFixture fixture)
        {
            this.fixture = fixture;
            this.fixture.ContextAction((context) =>
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            });
        }

        [Fact(DisplayName = "Verify api/Candidates [Post] is returning validation exception when candidate phone number already exist on database ")]
        public async System.Threading.Tasks.Task GivenCandidatePost_WhenCandidatePhoneNumberAlreadyExistOnDatabase_ShouldReturnBadRequest()
        {
            // Arrange
            var phoneNumber = "011155122200";
            var candidateInDb = new Candidate() { Name = "Test", PhoneNumber = phoneNumber };
            var userInDb = new User() { Username = "Test" };
            var communityInDb = new Community() { Name = "community", Profile = new CandidateProfile() { Name = "community" } };

            this.fixture.Seed(candidateInDb);
            this.fixture.Seed(userInDb);
            this.fixture.Seed(communityInDb);

            var model = new CreateCandidateViewModel()
            {
                DNI = 36501240,
                Name = "Rodrigo",
                LastName = "Ramirez",
                User = new ReadedUserViewModel() { Id = userInDb.Id },
                Community = new ReadedCommunityViewModel() { Id = communityInDb.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = communityInDb.Profile.Id },
                PhoneNumber = phoneNumber, // Key
            };

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<CreatedCandidateViewModel>(HttpVerb.POST, this.fixture.ControllerName, model);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.Equal("Phone number already exists", httpResultData.ResponseError.ExceptionMessage);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get] is returning Accepted [202] when does find entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGet_WhenEntitiesAreFound_ShouldReturnAndAccepted202()
        {
            // Arrange
            var candidateInDb = new Candidate() { Name = "Test", DNI = 36501240 };
            this.fixture.Seed(candidateInDb);
            var candidatesCount = this.fixture.GetCount<Candidate>();

            // Act
            var httpResultData =
                await this.fixture.HttpCallAsync<List<ReadedCandidateViewModel>>(
                    HttpVerb.GET,
                    $"{this.fixture.ControllerName}/");

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(candidatesCount, httpResultData.ResponseEntity.Count);
            Assert.Equal(candidateInDb.Id, httpResultData.ResponseEntity.Single().Id);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get] is returning Accepted [202] and an empty collection when does not find entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGet_WhenThereAreNoEntities_ShouldReturnAccepted202AndEmptyCollection()
        {
            // Act
            var httpResultData = await this.fixture.HttpCallAsync<List<ReadedCandidateViewModel>>(HttpVerb.GET, this.fixture.ControllerName).ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Empty(httpResultData.ResponseEntity);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{FilterCandidateViewModel}] is returning right entity when a valid filter is provided and there is a match")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesFilter_WhenThereIsValidFilterAndValidData_ShouldReturnEntitiesAndAccepted202()
        {
            // Arrange
            var candidateList = this.fixture.GetCandidateList();
            this.fixture.Seed(candidateList);
            var model = this.fixture.GetFilterCandidateViewModel(CandidatesControllerFixture.FilterType.Match);

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<List<ReadedCandidateViewModel>>(HttpVerb.POST, $"{this.fixture.ControllerName}/filter", model);

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.True(httpResultData.ResponseEntity.Count == 1);
            Assert.Equal("this will meet search criteria", httpResultData.ResponseEntity.Single().Name);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{FilterCandidateViewModel}] is returning Accepted [202] and a empty collection when there is no match")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesFilter_WhenThereIsNoMatchForFilter_ShouldReturnAcceptedAndAnEmptyCollectionOfEntities()
        {
            // Arrange
            var model = this.fixture.GetFilterCandidateViewModel(CandidatesControllerFixture.FilterType.DontMatch);

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<List<ReadedCandidateViewModel>>(HttpVerb.POST, $"{this.fixture.ControllerName}/filter", model);

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.True(httpResultData.ResponseEntity.Count == 0);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{id}] is returning Accepted [202] and entity when Id is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetId_WhenEntityIsFound_ShouldReturnAccepted202()
        {
            // Arrange
            var candidateInDb = new Candidate() { Name = "Test" };
            this.fixture.Seed(candidateInDb);
            var candidate = this.fixture.Get<Candidate>(candidateInDb.Id);

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<ReadedCandidateViewModel>(HttpVerb.GET, $"{this.fixture.ControllerName}/{candidate.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(candidate.Id, httpResultData.ResponseEntity.Id);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{id}] is returning Not Found [404] when id is not valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetId_WhenEntityIsNotFound_ShouldReturnNotFound404()
        {
            // Arrange
            var invalidId = 999;

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<ReadedCandidateViewModel>(HttpVerb.GET, $"{this.fixture.ControllerName}/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResultData.Response.StatusCode);
            Assert.Equal("Not Found", httpResultData.Response.ReasonPhrase);
        }

        [Fact(DisplayName = "Verify api/Candidates [Exists/{id}] is returning Accepted [202] entity when Id is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesExistsId_WhenEntityIsFound_ShouldReturnAccepted202()
        {
            // Arrange
            var candidateInDb = new Candidate() { Name = "Test" };
            this.fixture.Seed(candidateInDb);
            var candidate = this.fixture.Get<Candidate>(candidateInDb.Id);

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<ReadedCandidateViewModel>(HttpVerb.GET, $"{this.fixture.ControllerName}/Exists/{candidate.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
        }

        [Fact(DisplayName = "Verify api/Candidates [Exists/{id}] is returning Accepted[202](?) when id is not valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesExiststId_WhenEntityIsNotFound_ShoulReturnAccepted202()
        {
            // Arrange
            var invalidId = 999;

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<ReadedCandidateViewModel>(HttpVerb.GET, $"{this.fixture.ControllerName}/Exists/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Empty(httpResultData.ResponseString);
        }

        [Fact(DisplayName = "Verify api/Candidates [GetApp] is returning Accepted[202] when there are entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetApp_WhenEntitiesAreFound_ShouldReturnAccepted200AndEntities()
        {
            // Arrange
            var candidateInDb = new Candidate() { Name = "Test" };
            this.fixture.Seed(candidateInDb);
            var entitiesCount = this.fixture.GetCount<Candidate>();

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<List<ReadedCandidateAppViewModel>>(HttpVerb.GET, $"{this.fixture.ControllerName}/GetApp");

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(entitiesCount, httpResultData.ResponseEntity.Count);
        }

        [Fact(DisplayName = "Verify api/Candidates [GetApp] is returning Accepted[202] and a empty collection when there are no entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetApp_WhenEntitiesAreNotFound_ShouldReturnAcceptedAndEmptyCollection()
        {
            // Act
            var httpResultData = await this.fixture.HttpCallAsync<List<ReadedCandidateAppViewModel>>(HttpVerb.GET, $"{this.fixture.ControllerName}/GetApp");

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Empty(httpResultData.ResponseEntity);
        }

        [Fact(DisplayName = "Verify api/Candidates [Post] is returning Created [201] when data is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesPost_WhenCreationIsSuccesfull_ShouldReturnCreated201()
        {
            // Arrange
            var candidateInDb = new Candidate() { Name = "Test", EmailAddress = "test@gmail.com" };
            var userInDb = new User() { Username = "Test" };
            var communityInDb = new Community() { Name = "community", Profile = new CandidateProfile() { Name = "community" } };

            this.fixture.Seed(candidateInDb);
            this.fixture.Seed(userInDb);
            this.fixture.Seed(communityInDb);

            var model = new CreateCandidateViewModel()
            {
                DNI = 36501240,
                Name = "Rodrigo",
                LastName = "Ramirez",
                User = new ReadedUserViewModel() { Id = userInDb.Id },
                Community = new ReadedCommunityViewModel() { Id = communityInDb.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = communityInDb.Profile.Id },
                LinkedInProfile = "/linkedin",
                PhoneNumber = "0111551277868",
            };

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<CreatedCandidateViewModel>(HttpVerb.POST, this.fixture.ControllerName, model);

            // Assert
            Assert.Equal(HttpStatusCode.Created, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData.ResponseEntity);
            Assert.True(httpResultData.ResponseEntity.Id > 0);
        }

        [Theory(DisplayName = "Verify api/Candidates [Post] is returning Bad Request [400] when validation of model fails ")]
        [Trait("Category", "Functional-Test")]
        [InlineData("Name")]
        [InlineData("LastName")]
        public async System.Threading.Tasks.Task GivenCandidatesPost_WhenValidationFails_ShouldReturnBadRequest(string parameterName)
        {
            var candidateInDb = new Candidate() { Name = "Test", EmailAddress = "test@gmail.com" };
            var userInDb = new User() { Username = "Test" };
            var communityInDb = new Community() { Name = "community", Profile = new CandidateProfile() { Name = "community" } };

            this.fixture.Seed(candidateInDb);
            this.fixture.Seed(userInDb);
            this.fixture.Seed(communityInDb);

            // Arrange
            var model = new CreateCandidateViewModel()
            {
                DNI = 36501240,
                Name = "Rodrigo",
                LastName = "Ramirez",
                User = new ReadedUserViewModel() { Id = userInDb.Id },
                Community = new ReadedCommunityViewModel() { Id = communityInDb.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = communityInDb.Profile.Id },
                LinkedInProfile = "/linkedin",
            };

            model.WithPropertyValue(parameterName, null);

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<CreatedCandidateViewModel>(HttpVerb.POST, this.fixture.ControllerName, model);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.True(httpResultData.ResponseEntity.Id == 0);
        }

        [Fact(DisplayName = "Verify api/Candidates [Post] returns bad request when email is already in database")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesPost_WhenExistanceValidationFailsOnEmail_ShouldReturnBadRequest()
        {
            var candidateInDb = new Candidate() { Name = "Test", EmailAddress = "test@gmail.com" };
            var userInDb = new User() { Username = "Test" };
            var communityInDb = new Community() { Name = "community", Profile = new CandidateProfile() { Name = "community" } };

            this.fixture.Seed(candidateInDb);
            this.fixture.Seed(userInDb);
            this.fixture.Seed(communityInDb);

            // Arrange

            var model = new CreateCandidateViewModel()
            {
                DNI = 36501240,
                Name = "Rodrigo",
                LastName = "Ramirez",
                User = new ReadedUserViewModel() { Id = userInDb.Id },
                Community = new ReadedCommunityViewModel() { Id = communityInDb.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = communityInDb.Profile.Id },
                EmailAddress = candidateInDb.EmailAddress, /* wrong data, already in database */
                LinkedInProfile = "/linkedin",
            };

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<CreatedCandidateViewModel>(HttpVerb.POST, this.fixture.ControllerName, model);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.Equal("Email address already exists", httpResultData.ResponseError.ExceptionMessage);
            Assert.Equal(400, httpResultData.ResponseError.ErrorCode);
        }

        [Fact(DisplayName = "Verify api/Candidates [Put] is returning Accepted [202] when update model is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesPut_WhenModelIsValid_ShouldUpdateEntityAndReturnAccepted202()
        {
            // Assert
            // Wrong values
            var wrongUserInDb = new User() { FirstName = "Wrong user" };
            var wrongProfileInDb = new CandidateProfile() { Name = "Wrong CandidateProfile" };
            var wrongOfficeInDb = new Office() { Name = "Wrong office" };
            var wrongCommunityInDb = new Community() { Name = "Wrong Community", Profile = wrongProfileInDb };

            var candidateToFix = new Candidate()
            {
                Name = "Testing with TYPO",
                LastName = "Testerino with TYPO",
                LinkedInProfile = "/TestingTestirino99 with TYPO",
                EmailAddress = "Email Address with TYPO",
                User = wrongUserInDb,
                Community = wrongCommunityInDb,
                Profile = wrongProfileInDb,
                PreferredOffice = wrongOfficeInDb,
            };

            this.fixture.Seed(candidateToFix);

            // Right values
            var rightUserInDb = new User() { FirstName = "Right user" };
            var rightProfileInDb = new CandidateProfile() { Name = "Right CandidateProfile" };
            var rightOfficeInDb = new Office() { Name = "Right office" };
            var rightCommunityInDb = new Community() { Name = "Right Community", Profile = rightProfileInDb };
            this.fixture.Seed(rightUserInDb);
            this.fixture.Seed(rightOfficeInDb);
            this.fixture.Seed(rightCommunityInDb);

            var model = new UpdateCandidateViewModel()
            {
                Name = "Testing",
                LastName = "Testerino",
                LinkedInProfile = "/TestingTestirino99",
                EmailAddress = "Email Address",
                User = new ReadedUserViewModel() { Id = rightUserInDb.Id },
                Community = new ReadedCommunityViewModel() { Id = rightCommunityInDb.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = rightCommunityInDb.Profile.Id },
                PreferredOfficeId = rightOfficeInDb.Id,
            };

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<object>(HttpVerb.PUT, $"{this.fixture.ControllerName}", model, candidateToFix.Id);

            var candidateFromDatabase = this.fixture.GetEager(candidateToFix.Id);

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(model.Name, candidateFromDatabase.Name);
            Assert.Equal(model.LastName, candidateFromDatabase.LastName);
            Assert.Equal(model.LinkedInProfile, candidateFromDatabase.LinkedInProfile);
            Assert.Equal(model.User.Id, candidateFromDatabase.User.Id);
            Assert.Equal(model.Community.Id, candidateFromDatabase.Community.Id);
            Assert.Equal(model.Profile.Id, candidateFromDatabase.Profile.Id);
            Assert.Equal(model.PreferredOfficeId, candidateFromDatabase.PreferredOffice.Id);
        }

        [Theory(DisplayName = "Verify api/Candidates [Put] is returning BadRequest 400 when model is not valid")]
        [Trait("Category", "Functional-Test")]
        [InlineData("Name")]
        [InlineData("LastName")]
        public async System.Threading.Tasks.Task GivenCandidatesPut_WhenModelIsInValid_ShouldNotUpdateEntitiesAndReturnBadRequest400(string propertyName)
        {
            // Arrange
            var userInDb = new User() { Username = "Test" };
            var officeInDb = new Office() { Name = "Test" };
            var communityInDb = new Community() { Name = "community", Profile = new CandidateProfile() { Name = "community" } };

            this.fixture.Seed(userInDb);
            this.fixture.Seed(officeInDb);
            this.fixture.Seed(communityInDb);

            var candidate = new Candidate()
            {
                Name = "Testing with TYPO",
                LastName = "Testerino with TYPO",
                LinkedInProfile = "/TestingTestirino99 with TYPO",
                PreferredOffice = new Office() { Name = "Outdated Office" },
                User = new User() { FirstName = "Outdated user" },
                Community = new Community() { Name = "Outdated Community", Profile = new CandidateProfile() { Name = "Outdated candidate profile name" } },
            };

            this.fixture.Seed(candidate);

            var model = new UpdateCandidateViewModel()
            {
                Name = "this property will be invalid",
                LastName = "This property will be invalid",
                LinkedInProfile = "/TestingTestirino99",
                User = new ReadedUserViewModel() { Id = userInDb.Id },
                Community = new ReadedCommunityViewModel() { Id = communityInDb.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = communityInDb.Profile.Id },
                PreferredOfficeId = officeInDb.Id,
            };

            model.WithPropertyValue(propertyName, null);

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<object>(HttpVerb.PUT, $"{this.fixture.ControllerName}", model, candidate.Id);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
        }

        [Fact(DisplayName = "Verify api/Candidates [Delete/{id}] is returning Accepted [202] and deletes the entity when is found")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesDeleteId_WhenEntityIsFound_ShouldDeleteEntityAndReturnAccepted202()
        {
            // Arrange
            var candidate = new Candidate() { Name = "Testing" };
            this.fixture.Seed(candidate);
            var entityCountBeforeDelete = this.fixture.GetCount<Candidate>();

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<object>(HttpVerb.DELETE, $"{this.fixture.ControllerName}", null, candidate.Id);
            var entityCountAfterDelete = this.fixture.GetCount<Candidate>();

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(1, entityCountBeforeDelete);
            Assert.Equal(0, entityCountAfterDelete);
            Assert.NotEqual(entityCountBeforeDelete, entityCountAfterDelete);
        }

        [Fact(DisplayName = "Verify api/Candidates [Delete/{id}] is returning bad request when id is not found")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesDelete_WhenEntityIsFound_ShouldReturnBadRequest()
        {
            // Arrange
            var invalidEntityId = 999;

            // Act
            var httpResultData = await this.fixture.HttpCallAsync<object>(HttpVerb.DELETE, $"{this.fixture.ControllerName}", null, invalidEntityId);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.Equal($"Candidate not found for the CandidateId: {invalidEntityId}", httpResultData.ResponseError.ExceptionMessage);
        }

        [Fact(DisplayName = "Verify that pings returns ok")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetId_WhenEntityIsFound_ShouldRetssurnAccepted2022()
        {
            // Act
            var httpResultData = await this.fixture.HttpCallAsync<object>(HttpVerb.GET, $"{this.fixture.ControllerName}/Ping");

            // Assert
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
        }
    }
}
