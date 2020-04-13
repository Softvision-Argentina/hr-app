using System.Collections.Generic;
using Core;
using Xunit;
using ApiServer.Contracts.Candidates;
using Domain.Model;
using ApiServer.FunctionalTests.Core;
using System.Net;
using Persistance.EF.Extensions;
using System.Linq;
using ApiServer.Contracts.Community;
using Microsoft.EntityFrameworkCore;
using ApiServer.Contracts.CandidateSkill;
using ApiServer.Contracts.Consultant;
using ApiServer.Contracts.CandidateProfile;

namespace ApiServer.FunctionalTests.Controller
{
    public class CandidatesControllerFunctionalTest : BaseApiTest
    {
        readonly ApiFixture _fixture;
        public CandidatesControllerFunctionalTest(ApiFixture fixture) : base(fixture)
        {
            _fixture = fixture;
            ControllerName = "Candidates";
        }

        [Fact(DisplayName = "Verify api/Candidates [Get] is returning Accepted [202] when does find entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGet_WhenEntitiesAreFound_ShouldReturnAndAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var list = new List<Candidate>()
            {
                new Candidate() { Name = "Testing" },
                new Candidate() { Name = "Testerino" }
            };

            Context.SeedDatabaseWith(list);

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateViewModel>>(HttpVerb.GET, $"{ControllerName}/");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(list.Count, httpResultData.ResponseEntity.Count);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get] is returning Accepted [202] and an empty collection when does not find entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGet_WhenThereAreNoEntities_ShouldReturnAccepted202AndEmptyCollection()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateViewModel>>(HttpVerb.GET, ControllerName);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Empty(httpResultData.ResponseEntity);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{FilterCandidateViewModel}] is returning right entity when a valid filter is provided and there is a match")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesFilter_WhenThereIsValidFilterAndValidData_ShouldReturnEntitiesAndAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            Context.SeedDatabaseWith(CandidatesControllerHelper.GetCandidateList());
            var model = CandidatesControllerHelper.GetFilterCandidateViewModel(FilterType.MatchEntity);

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateViewModel>>(HttpVerb.POST, $"{ControllerName}/filter", model);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.True(httpResultData.ResponseEntity.Count == 1);
            Assert.Equal(CandidatesControllerHelper.validFilterData.Candidate.Name, httpResultData.ResponseEntity.Single().Name);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{FilterCandidateViewModel}] is returning Accepted [202] and a empty collection when there is no match")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesFilter_WhenThereIsNoMatchForFilter_ShouldReturnAcceptedAndAnEmptyCollectionOfEntities()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            Context.SeedDatabaseWith(CandidatesControllerHelper.GetCandidateList());
            var model = CandidatesControllerHelper.GetFilterCandidateViewModel(FilterType.WontMatchEntity);

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateViewModel>>(HttpVerb.POST, $"{ControllerName}/filter", model);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.True(httpResultData.ResponseEntity.Count == 0);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{id}] is returning Accepted [202] and entity when Id is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetId_WhenEntityIsFound_ShouldReturnAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidate = new Candidate() { Name = "Testing" };
            Context.SeedDatabaseWith(candidate);

            //Act
            var httpResultData = await HttpCallAsync<ReadedCandidateViewModel>(HttpVerb.GET, $"{ControllerName}/{candidate.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(candidate.Id, httpResultData.ResponseEntity.Id);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{id}] is returning Not Found [404] when id is not valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetId_WhenEntityIsNotFound_ShouldReturnNotFound404()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidate = new Candidate() { Id = 1, Name = "Testing" };

            //Act
            var httpResultData = await HttpCallAsync<ReadedCandidateViewModel>(HttpVerb.GET, $"{ControllerName}/{candidate.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResultData.Response.StatusCode);
            Assert.Equal("Not Found", httpResultData.Response.ReasonPhrase);
        }

        [Fact(DisplayName = "Verify api/Candidates [Exists/{id}] is returning Accepted [202] entity when Id is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesExistsId_WhenEntityIsFound_ShouldReturnAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidate = new Candidate() { Name = "Testing" };
            Context.SeedDatabaseWith(candidate);

            //Act
            var httpResultData = await HttpCallAsync<ReadedCandidateViewModel>(HttpVerb.GET, $"{ControllerName}/Exists/{candidate.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(candidate.Id, httpResultData.ResponseEntity.Id);
        }

        [Fact(DisplayName = "Verify api/Candidates [Exists/{id}] is returning Accepted[202](?) when id is not valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesExiststId_WhenEntityIsNotFound_ShouldReturnNotFound404()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidate = new Candidate() { Id = 1, Name = "Testing" };

            //Act
            var httpResultData = await HttpCallAsync<ReadedCandidateViewModel>(HttpVerb.GET, $"{ControllerName}/Exists/{candidate.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Empty(httpResultData.ResponseString);
        }

        [Fact(DisplayName = "Verify api/Candidates [GetApp] is returning Accepted[202] when there are entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetApp_WhenEntitiesAreFound_ShouldReturnAccepted200AndEntities()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            Context.SeedDatabaseWith(CandidatesControllerHelper.GetCandidateList());
            int entitiesCount = CandidatesControllerHelper.GetCandidateList().Count;

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateAppViewModel>>(HttpVerb.GET, $"{ControllerName}/GetApp");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(entitiesCount, httpResultData.ResponseEntity.Count);
        }

        [Fact(DisplayName = "Verify api/Candidates [GetApp] is returning Accepted[202] and a empty collection when ther are not entities")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetApp_WhenEntiesAreNotFound_ShouldReturnAcceptedAndEmptyCollection()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateAppViewModel>>(HttpVerb.GET, $"{ControllerName}/GetApp");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Empty(httpResultData.ResponseEntity);
        }

        [Fact(DisplayName = "Verify api/Candidates [Post] is returning Created [201] when data is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesPost_WhenCreationIsSuccesfull_ShouldReturnCreated201()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var recruiter = new Consultant() { Name = "Testing" };
            var community = new Community() { Name = "Net", Profile = new CandidateProfile() { Name = "Testing profile" } };

            Context.SeedDatabaseWith(recruiter);
            Context.SeedDatabaseWith(community);

            var model = new CreateCandidateViewModel()
            {
                DNI = 36501240,
                Name = "Rodrigo",
                LastName = "Ramirez",
                Recruiter = new ReadedConsultantViewModel() { Id = recruiter.Id },
                Community = new ReadedCommunityViewModel() { Id = community.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = community.Profile.Id },
                LinkedInProfile = "/linkedin"
            };

            //Act
            var httpResultData = await HttpCallAsync<CreatedCandidateViewModel>(HttpVerb.POST, ControllerName, model);

            //Assert
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
            //Arrange
            Context.SetupDatabaseForTesting();
            var recruiter = new Consultant() { Name = "Testing" };
            var community = new Community() { Name = "Net", Profile = new CandidateProfile() { Name = "Testing profile" } };
            Context.SeedDatabaseWith(recruiter);
            Context.SeedDatabaseWith(community);

            var model = new CreateCandidateViewModel()
            {
                DNI = 36501240,
                Name = "Rodrigo",
                LastName = "Ramirez",
                Recruiter = new ReadedConsultantViewModel() { Id = recruiter.Id },
                Community = new ReadedCommunityViewModel() { Id = community.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = community.Profile.Id },
                LinkedInProfile = "/linkedin"
            };

            model.WithPropertyValue(parameterName, default);

            //Act
            var httpResultData = await HttpCallAsync<CreatedCandidateViewModel>(HttpVerb.POST, ControllerName, model);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
            Assert.True(httpResultData.ResponseEntity.Id == 0);
        }

        [Fact(DisplayName = "Verify api/Candidates [Post] is returning Internal Server Error [500] when email is already in database")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesPost_WhenExistanceValidationFailsOnEmail_ShouldReturnInternalServerError500()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var recruiter = new Consultant() { Name = "Testing" };
            var community = new Community() { Name = "Net", Profile = new CandidateProfile() { Name = "Testing profile" } };
            var candidate = new Candidate() { Name = "Testirino", EmailAddress = "testirino99@gmail.com" };

            Context.SeedDatabaseWith(recruiter);
            Context.SeedDatabaseWith(community);
            Context.SeedDatabaseWith(candidate);

            var model = new CreateCandidateViewModel()
            {
                DNI = 36501240,
                Name = "Rodrigo",
                LastName = "Ramirez",
                Recruiter = new ReadedConsultantViewModel() { Id = recruiter.Id },
                Community = new ReadedCommunityViewModel() { Id = community.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = community.Profile.Id },
                EmailAddress = candidate.EmailAddress,
                LinkedInProfile = "/linkedin"
            };

            //Act
            var httpResultData = await HttpCallAsync<CreatedCandidateViewModel>(HttpVerb.POST, ControllerName, model);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
            Assert.Equal("The Email already exists .", httpResultData.ResponseError.Message);
            Assert.Equal(400, httpResultData.ResponseError.ErrorCode);
        }

        [Fact(DisplayName = "Verify api/Candidates [Post] is returning Internal Server Error [500] when Linkedin profile is already in database")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesPost_WhenExistanceValidationFailsOnLinkedinProfile_ShouldReturnInternalServerError500()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var recruiter = new Consultant() { Name = "Testing" };
            var community = new Community() { Name = "Net", Profile = new CandidateProfile() { Name = "Testing profile" } };
            var candidate = new Candidate() { Name = "Testirino", LinkedInProfile = "/Testirino99" };

            Context.SeedDatabaseWith(recruiter);
            Context.SeedDatabaseWith(community);
            Context.SeedDatabaseWith(candidate);

            var model = new CreateCandidateViewModel()
            {
                DNI = 36501240,
                Name = "Rodrigo",
                LastName = "Ramirez",
                Recruiter = new ReadedConsultantViewModel() { Id = recruiter.Id },
                Community = new ReadedCommunityViewModel() { Id = community.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = community.Profile.Id },
                LinkedInProfile = candidate.LinkedInProfile
            };

            //Act
            var httpResultData = await HttpCallAsync<CreatedCandidateViewModel>(HttpVerb.POST, ControllerName, model);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
            Assert.Equal("The LinkedIn Profile already exists in our database.", httpResultData.ResponseError.Message);
            Assert.Equal(400, httpResultData.ResponseError.ErrorCode);
        }

        [Fact(DisplayName = "Verify api/Candidates [Put] is returning Accepted [202] when update model is valid")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesPut_WhenModelIsValid_ShouldUpdateEntityAndReturnAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var recruiter = new Consultant() { Name = "new recruiter" };
            var community = new Community() { Name = "new community", Profile = new CandidateProfile() { Name = "new candidate profile name" } };
            var office = new Office() { Name = "new office preference" };

            Context.SeedDatabaseWith(recruiter);
            Context.SeedDatabaseWith(community);
            Context.SeedDatabaseWith(office);

            var candidate = new Candidate()
            {
                Name = "Testing with TYPO",
                LastName = "Testerino with TYPO",
                LinkedInProfile = "/TestingTestirino99 with TYPO",
                PreferredOffice = new Office() { Name = "Outdated Office" },
                Recruiter = new Consultant() { Name = "Outdated recruiter" },
                Community = new Community() { Name = "Outdated Community", Profile = new CandidateProfile() { Name = "Outdated candidate profile name" } }
            };

            Context.SeedDatabaseWith(candidate);

            var model = new UpdateCandidateViewModel()
            {
                Name = "Testing",
                LastName = "Testerino",
                LinkedInProfile = "/TestingTestirino99",
                Recruiter = new ReadedConsultantViewModel() { Id = recruiter.Id },
                Community = new ReadedCommunityViewModel() { Id = community.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = community.Profile.Id },
                PreferredOfficeId = office.Id
            };

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.PUT, $"{ControllerName}", model, candidate.Id);
            var candidateFromDatabase = Context.Candidates
                .AsNoTracking()
                .Include(_ => _.Recruiter)
                .Include(_ => _.Community)
                .Include(_ => _.Profile)
                .Include(_ => _.PreferredOffice)
                .Single();

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(model.Name, candidateFromDatabase.Name);
            Assert.Equal(model.LastName, candidateFromDatabase.LastName);
            Assert.Equal(model.LinkedInProfile, candidateFromDatabase.LinkedInProfile);
            Assert.Equal(model.Recruiter.Id, candidateFromDatabase.Recruiter.Id);
            Assert.Equal(model.Community.Id, candidateFromDatabase.Community.Id);
            Assert.Equal(model.Profile.Id, candidateFromDatabase.Profile.Id);
            Assert.Equal(model.PreferredOfficeId, candidateFromDatabase.PreferredOffice.Id);
        }

        [Theory(DisplayName = "Verify api/Candidates [Put] is returning Accepted [202] when update model is valid")]
        [Trait("Category", "Functional-Test")]
        [InlineData("Name")]
        [InlineData("LastName")]
        public async System.Threading.Tasks.Task GivenCandidatesPut_WhenModelIsInValid_ShouldNotUpdateEntitiesAndReturnBadRequest400(string propertyName)
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var recruiter = new Consultant() { Name = "new recruiter" };
            var community = new Community() { Name = "new community", Profile = new CandidateProfile() { Name = "new candidate profile name" } };
            var office = new Office() { Name = "new office preference" };

            var candidate = new Candidate()
            {
                Name = "Testing with TYPO",
                LastName = "Testerino with TYPO",
                LinkedInProfile = "/TestingTestirino99 with TYPO",
                PreferredOffice = new Office() { Name = "Outdated Office" },
                Recruiter = new Consultant() { Name = "Outdated recruiter" },
                Community = new Community() { Name = "Outdated Community", Profile = new CandidateProfile() { Name = "Outdated candidate profile name" } }
            };

            Context.SeedDatabaseWith(candidate);

            var model = new UpdateCandidateViewModel()
            {
                Name = "this property will be invalid",
                LastName = "This property will be invalid",
                LinkedInProfile = "/TestingTestirino99",
                Recruiter = new ReadedConsultantViewModel() { Id = recruiter.Id },
                Community = new ReadedCommunityViewModel() { Id = community.Id },
                Profile = new ReadedCandidateProfileViewModel() { Id = community.Profile.Id },
                PreferredOfficeId = office.Id
            };

            model.WithPropertyValue(propertyName, default);

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.PUT, $"{ControllerName}", model, candidate.Id);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
        }

        [Fact(DisplayName = "Verify api/Candidates [Delete/{id}] is returning Accepted [202] and deletes the entity when is found")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesDeleteId_WhenEntityIsFound_ShouldDeleteEntityAndReturnAccepted202()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidate = new Candidate() { Name = "Testing" };
            Context.SeedDatabaseWith(candidate);
            int entityCountBeforeDelete = Context.Candidates.AsNoTracking().Count();

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.DELETE, $"{ControllerName}", null, candidate.Id);
            int entityCountAfterDelete = Context.Candidates.AsNoTracking().Count();

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(1, entityCountBeforeDelete);
            Assert.Equal(0, entityCountAfterDelete);
            Assert.NotEqual(entityCountBeforeDelete, entityCountAfterDelete);
        }
        [Fact(DisplayName = "Verify api/Candidates [Delete/{id}] is returning Internal Server Error [500] when id is not found")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetId_WhenEntityIsFound_ShouldReturnAccepted2022()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            int invalidEntityId = 999;

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.DELETE, $"{ControllerName}", null, invalidEntityId);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
            Assert.Equal($"Candidate not found for the CandidateId: {invalidEntityId}", httpResultData.ResponseError.Message);
        }

        [Fact(DisplayName = "Verify that pings returns ok")]
        [Trait("Category", "Functional-Test")]
        public async System.Threading.Tasks.Task GivenCandidatesGetId_WhenEntityIsFound_ShouldRetssurnAccepted2022()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            //Act
            var httpResultData = await HttpCallAsync<object>(HttpVerb.GET, $"{ControllerName}/Ping");

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResultData.Response.StatusCode);
        }

        private enum FilterType
        {
            MatchEntity, //will match an entity
            WontMatchEntity //will not match any entity
        }

        private static class CandidatesControllerHelper
        {
            public class FilterData
            {
                public Candidate Candidate { get; set; }
                public Community Community { get; set; }
                public Office Office { get; set; }
                public Skill Skill { get; set; }
            }

            public static FilterData validFilterData;

            public static List<Candidate> GetCandidateList()
            {
                var netCommunity = new Community() { Name = "Net", Profile = new CandidateProfile() { Name = "candidate profile 1" } };
                var devopsCommunity = new Community() { Name = "Net", Profile = new CandidateProfile() { Name = "candidate profile 2" } };
                var almagroOffice = new Office() { Name = "Almagro" };
                var vicenteLopezOffice = new Office() { Name = "Vicente Lopez" };
                var netSkill = new Skill() { Name = "Entity Framework" };
                var devopsSkill = new Skill() { Name = "Jenkins" };

                var expectedCandidate = new Candidate()
                {
                    Name = "this will meet search criteria",
                    PreferredOffice = almagroOffice,
                    Community = netCommunity,
                    CandidateSkills = new List<CandidateSkill>()
                    {
                        new CandidateSkill(){ Skill = netSkill, Rate = 9 }
                    }
                };

                var candidateList = new List<Candidate>()
                {
                    new Candidate() //skill rate is low
                    {
                        Name = "This candidate will not meet search criteria",
                        PreferredOffice = almagroOffice,
                        Community = netCommunity,
                        CandidateSkills = new List<CandidateSkill>()
                        {
                            new CandidateSkill(){ Skill = netSkill, Rate = 1 }
                        }
                    },

                    new Candidate() //not the community asked
                    {
                        Name = "This candidate will not meet search criteria either",
                        PreferredOffice = vicenteLopezOffice,
                        Community = devopsCommunity,
                        Profile = new CandidateProfile(),
                        CandidateSkills = new List<CandidateSkill>()
                        {
                            new CandidateSkill(){ Skill = devopsSkill }
                        }
                    },

                    expectedCandidate
                 };

                validFilterData = new FilterData() { Candidate = expectedCandidate, Community = netCommunity, Office = almagroOffice, Skill = netSkill };

                return candidateList;
            }

            public static FilterCandidateViewModel GetFilterCandidateViewModel(FilterType filterType)
            {
                var validFilterCandidateSkillViewModel = new List<FilterCandidateSkillViewModel>() {
                    new FilterCandidateSkillViewModel() { SkillId = validFilterData.Skill.Id, MinRate = 5, MaxRate = 10 }
                };

                var invalidFilterCandidateSkillViewModel = new List<FilterCandidateSkillViewModel>() {
                    new FilterCandidateSkillViewModel() { SkillId = 999, MinRate = 999, MaxRate = 999 }
                };

                var validModel = new FilterCandidateViewModel() { Community = validFilterData.Community.Id, PreferredOffice = validFilterData.Office.Id, SelectedSkills = validFilterCandidateSkillViewModel };
                var invalidModel = new FilterCandidateViewModel() { Community = validFilterData.Community.Id, PreferredOffice = validFilterData.Office.Id, SelectedSkills = invalidFilterCandidateSkillViewModel };

                return filterType == FilterType.MatchEntity ? validModel : invalidModel;
            }
        }
    }
}