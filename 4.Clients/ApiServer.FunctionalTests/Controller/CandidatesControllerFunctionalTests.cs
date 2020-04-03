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
        [Trait("Category", "API-Tasks")]
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
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Equal(list.Count, httpResultData.ResponseEntity.Count);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get] is returning Accepted [202] and an empty collection when does not find entities")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidatesGet_WhenThereAreNoEntities_ShouldReturnAccepted202AndEmptyCollection()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateViewModel>>(HttpVerb.GET, ControllerName);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.Empty(httpResultData.ResponseEntity);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{FilterCandidateViewModel}] is returning right entity when a valid filter is provided and there is a match")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidatesFilter_WhenThereIsValidFilterAndValidData_ShouldReturnAcceptedAndEntity()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            Context.SeedDatabaseWith(CandidatesControllerHelper.GetCandidateList());
            var model = CandidatesControllerHelper.GetFilterModel(ModelType.Valid);

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateViewModel>>(HttpVerb.POST, $"{ControllerName}/filter", model);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.True(httpResultData.ResponseEntity.Count == 1);
            Assert.Equal(CandidatesControllerHelper.validFilterData.Candidate.Name, httpResultData.ResponseEntity.Single().Name);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{FilterCandidateViewModel}] is returning Accepted [202] and a empty collection when there is no match   ")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidatesFilter_WhenThereIsValidFilterAndInvalidData_ShouldReturnAcceptedAndEntity()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            Context.SeedDatabaseWith(CandidatesControllerHelper.GetCandidateList());
            var model = CandidatesControllerHelper.GetFilterModel(ModelType.Invalid);

            //Act
            var httpResultData = await HttpCallAsync<List<ReadedCandidateViewModel>>(HttpVerb.POST, $"{ControllerName}/filter", model);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.NotNull(httpResultData);
            Assert.True(httpResultData.ResponseEntity.Count == 0);
            Assert.Empty(httpResultData.ResponseEntity);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{id}] is returning Accepted [202] entity when Id is valid")]
        [Trait("Category", "API-Tasks")]
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
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.NotNull(httpResultData.ResponseEntity);
            Assert.Equal(candidate.Id, httpResultData.ResponseEntity.Id);
        }

        [Fact(DisplayName = "Verify api/Candidates [Get/{id}] is returning Not Found [404] when id is not valid")]
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidatesGetId_WhenEntityIsNotFound_ShouldReturnNotFound404()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var candidate = new Candidate() { Id = 1, Name = "Testing" };

            //Act
            var httpResultData = await HttpCallAsync<ReadedCandidateViewModel>(HttpVerb.GET, $"{ControllerName}/{candidate.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResultData.Response.StatusCode);
            Assert.Null(httpResultData.ResponseEntity);
            Assert.Equal("Not Found", httpResultData.Response.ReasonPhrase);
        }

        [Fact(DisplayName = "Verify api/Candidates [Exists/{id}] is returning Accepted [202] entity when Id is valid")]
        [Trait("Category", "API-Tasks")]
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
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.NotNull(httpResultData.ResponseEntity);
            Assert.Equal(candidate.Id, httpResultData.ResponseEntity.Id);
        }

        [Fact(DisplayName = "Verify api/Candidates [Exists/{id}] is returning Accepted[202](?) when id is not valid")]
        [Trait("Category", "API-Tasks")]
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
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidatesGetApp_WhenEntitiesAreFound_ShouldReturnAccepted200AndEntities()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            Context.SeedDatabaseWith(CandidatesControllerHelper.GetCandidateList());
            int entitiesCount = CandidatesControllerHelper.GetCandidateList().Count;

            //Act
            var httpResultData = await HttpCallAsync <List<ReadedCandidateAppViewModel>>(HttpVerb.GET, $"{ControllerName}/GetApp");

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
            Assert.Equal(entitiesCount, httpResultData.ResponseEntity.Count);
        }

        [Fact(DisplayName = "Verify api/Candidates [GetApp] is returning Accepted[202] and a empty collection when ther are not entities")]
        [Trait("Category", "API-Tasks")]
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
        [Trait("Category", "API-Tasks")]
        public async System.Threading.Tasks.Task GivenCandidatesPost_WhenCreationIsSuccesfull_ShouldReturnCreated201()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var recruiter = new Consultant() { Name = "Testing" };
            var community = new Community() { Name = "Net", Profile = new CandidateProfile() { Name = "Testing profile" }};

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
            Assert.NotNull(httpResultData);
            Assert.NotEmpty(httpResultData.ResponseString);
            Assert.NotNull(httpResultData.ResponseEntity);
            Assert.True(httpResultData.ResponseEntity.Id > 0);
        }

        //
        
        //[Theory(DisplayName = "Verify api/Candidates [Post] is returning Bad Request [400] when model is not valid")]
        //[InlineData("Name")]
        //[InlineData("Description")]
        //[Trait("Category", "API-Tasks")]
        //public async System.Threading.Tasks.Task GivenCandidatesPost_WhenCreationIsNotSuccesfullBecauseValidationError_ShouldReturnBadRequest400(string propertyName)
        //{
        //    //Arrange
        //    Context.SetupDatabaseForTesting();
        //    var model = DataFactory.CreateInstance<CreateCandidateViewModel>()
        //        .WithPropertyValue(propertyName, default);

        //    //Act
        //    var httpResultData = await HttpCallAsync<CreatedCandidateViewModel>(HttpVerb.POST, ControllerName, model);

        //    //Assert
        //    Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
        //    Assert.NotNull(httpResultData);
        //    Assert.NotEmpty(httpResultData.ResponseString);
        //}

        //[Fact(DisplayName = "Verify api/Candidates [Post] is returning Internal Server error [500] when model is not valid")]
        //[Trait("Category", "API-Tasks")]
        //public async System.Threading.Tasks.Task GivenCandidatesPost_WhenCreationIsNotSuccesfullBecauseExistenceError_ShouldReturnInternalServerError500()
        //{
        //    //Arrange
        //    Context.SetupDatabaseForTesting();
        //    var model = new Candidate() { Name = "Testing" };
        //    Context.SeedDatabaseWith(model);

        //    //Act
        //    var httpResultData = await HttpCallAsync<CreatedCandidateViewModel>(HttpVerb.POST, ControllerName, model);

        //    //Assert
        //    Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
        //    Assert.NotNull(httpResultData);
        //    Assert.NotEmpty(httpResultData.ResponseString);
        //    Assert.Equal("The Profile already exists .", httpResultData.ResponseError.Message);
        //}

        //[Fact(DisplayName = "Verify api/Candidates [Put] is returning Accepted [202] when data is valid")]
        //[Trait("Category", "API-Tasks")]
        //public async System.Threading.Tasks.Task GivenCandidatesPut_WhenUpdateIsSuccesfull_ShouldReturnAccepted202()
        //{
        //    //Arrange
        //    Context.SetupDatabaseForTesting();
        //    var candidateProfile = new Candidate { Name = "Testing Testerino" };
        //    Context.SeedDatabaseWith(candidateProfile);
        //    var communitiesCountBeforeUpdate = Context.Community.Count();
        //    var updateModel = new UpdateCandidateViewModel()
        //    {
        //        Name = candidateProfile.Name
        //    };

        //    //Act
        //    var httpResultData = await HttpCallAsync<object>(HttpVerb.PUT, ControllerName, updateModel, candidateProfile.Id);

        //    //Assert
        //    var candidateAfterUpdate = Context.Profiles.AsNoTracking().First();
        //    var communitiesCountAfterUpdate = Context.Community.AsNoTracking().Count();
        //    Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
        //}

        //[Theory(DisplayName = "Verify api/Candidates [Put] is returning Bad Request [400] when model is not valid")]
        //[InlineData("Name")]
        //[InlineData("Description")]
        //[Trait("Category", "API-Tasks")]
        //public async System.Threading.Tasks.Task GivenCandidatesPut_WhenUpdateIsNotSuccesfull_ShouldReturnInternalServerError500(string propertyName)
        //{
        //    //Arrange
        //    Context.SetupDatabaseForTesting();
        //    var candidateProfile = new Candidate { Name = "Testing Testerino" };
        //    Context.SeedDatabaseWith(candidateProfile);
        //    var updateModel = DataFactory.CreateInstance<UpdateCandidateViewModel>()
        //        .WithPropertyValue(propertyName, default);

        //    //Act
        //    var httpResultData = await HttpCallAsync<object>(HttpVerb.PUT, ControllerName, updateModel, candidateProfile.Id);

        //    //Assert
        //    Assert.Equal(HttpStatusCode.BadRequest, httpResultData.Response.StatusCode);
        //    Assert.NotNull(httpResultData);
        //}

        //[Fact(DisplayName = "Verify api/Candidates [Put] is returning Internal Server Error [500] when candidate profile already exists in database")]
        //[Trait("Category", "API-Tasks")]
        //public async System.Threading.Tasks.Task GivenCandidatesPut_WhenUpdateIsSuccesfull_ShouldReturnA3ccepted202()
        //{
        //    //Arrange
        //    Context.SetupDatabaseForTesting();
        //    var candidateProfile = new Candidate { Name = "Testing Testerino" };
        //    Context.SeedDatabaseWith(candidateProfile);
        //    int invalidId = 999;

        //    var updateModel = new UpdateCandidateViewModel()
        //    {
        //        Name = candidateProfile.Name
        //    };

        //    //Act
        //    var httpResultData = await HttpCallAsync<object>(HttpVerb.PUT, ControllerName, updateModel, invalidId);

        //    //Assert
        //    Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
        //    Assert.NotNull(httpResultData);
        //    Assert.NotEmpty(httpResultData.ResponseString);
        //    Assert.Equal("The Profile already exists .", httpResultData.ResponseError.Message);
        //}

        //[Fact(DisplayName = "Verify api/Candidates [Delete] is returning Accepted [202] when id is valid")]
        //[Trait("Category", "API-Tasks")]
        //public async System.Threading.Tasks.Task GivenCandidatesPut_WhenDeleteIsSuccesfull_ShouldReturnAccepted()
        //{
        //    //Arrange
        //    Context.SetupDatabaseForTesting();
        //    var candidateProfile = new Candidate { Name = "Testing Testerino" };
        //    Context.SeedDatabaseWith(candidateProfile);
        //    int countBeforeDelete = Context.Profiles.AsNoTracking().Count();

        //    //Act
        //    var httpResultData = await HttpCallAsync<object>(HttpVerb.DELETE, ControllerName, null, candidateProfile.Id);

        //    //Assert
        //    int countAfterDelete = Context.Profiles.AsNoTracking().Count();
        //    Assert.Equal(HttpStatusCode.Accepted, httpResultData.Response.StatusCode);
        //    Assert.NotEqual(countBeforeDelete, countAfterDelete);
        //    Assert.Equal(0, countAfterDelete);
        //    Assert.NotNull(httpResultData);
        //}

        //[Fact(DisplayName = "Verify api/Candidates [Delete] is returning Invalid Server [500] when id is invalid")]
        //[Trait("Category", "API-Tasks")]
        //public async System.Threading.Tasks.Task GivenCandidatesDeleteId_WhenIdIsValid_ShouldReturnAccepted202()
        //{
        //    //Arrange
        //    Context.SetupDatabaseForTesting();
        //    int invalidId = 999;

        //    //Act
        //    var httpResultData = await HttpCallAsync<object>(HttpVerb.DELETE, ControllerName, null, invalidId);

        //    //Assert
        //    Assert.Equal(HttpStatusCode.InternalServerError, httpResultData.Response.StatusCode);
        //    Assert.Equal($"Profile not found for the Profile Id: {invalidId}", httpResultData.ResponseError.Message);
        //}
        
        private enum ModelType
        {
            Valid, //will match an entity
            Invalid //will not match any entity
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

            internal static List<Candidate> GetCandidateList()
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

            internal static FilterCandidateViewModel GetFilterModel(ModelType modelType)
            {
                var validFilterCandidateSkillViewModel = new List<FilterCandidateSkillViewModel>() {
                    new FilterCandidateSkillViewModel() { SkillId = validFilterData.Skill.Id, MinRate = 5, MaxRate = 10 }
                };

                var invalidFilterCandidateSkillViewModel = new List<FilterCandidateSkillViewModel>() {
                    new FilterCandidateSkillViewModel() { SkillId = 999, MinRate = 999, MaxRate = 999 }
                };

                var validModel = new FilterCandidateViewModel() { Community = validFilterData.Community.Id, PreferredOffice = validFilterData.Office.Id, SelectedSkills = validFilterCandidateSkillViewModel };
                var invalidModel = new FilterCandidateViewModel() { Community = validFilterData.Community.Id, PreferredOffice = validFilterData.Office.Id, SelectedSkills = invalidFilterCandidateSkillViewModel };

                return modelType == ModelType.Valid ? validModel : invalidModel;
            }
        }
    }
}