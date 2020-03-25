using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.CandidateProfile;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using Persistance.EF.Extensions;
using System;
using System.Linq;
using Xunit;
using Core;
using Domain.Services.Impl.IntegrationTests.Core;

namespace Domain.Services.Impl.IntegrationTests.Services
{
    public class CandidateProfileServicesIntegrationTest : BaseServiceIntegrationTest
    {
        readonly ICandidateProfileService _candidateProfileService;
        readonly IRepository<CandidateProfile> _candidateProfileRepository;
        CandidateProfile _candidateProfile;
        CreateCandidateProfileContract _createCandidateProfileContract;

        public CandidateProfileServicesIntegrationTest(ServiceFixture serviceFixture) : base(serviceFixture)
        {
            _candidateProfileRepository = Services.GetService(typeof(IRepository<CandidateProfile>)) as IRepository<CandidateProfile>;
            _candidateProfileService = Services.GetService(typeof(ICandidateProfileService)) as ICandidateProfileService;
            _candidateProfile = DataFactory.CreateInstance<CandidateProfile>();
            _createCandidateProfileContract = DataFactory.CreateInstance<CreateCandidateProfileContract>();
        }

        [Fact(DisplayName = "Verify that given a valid model, should add to the database")]
        public void GivenValidCreateCandidateProfileContract_ShouldCreateItWithoutProblems()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            int recordCountBeforeCreate = GetProfilesCount();
            var validCreateCandidateProfileContract = _createCandidateProfileContract;

            //Act
            CreatedCandidateProfileContract result = _candidateProfileService.Create(validCreateCandidateProfileContract);
            int recordCountAfterCreate = GetProfilesCount();

            //Assert
            Assert.NotEqual(recordCountBeforeCreate, recordCountAfterCreate);
            Assert.Equal(1, recordCountAfterCreate);
            Assert.NotNull(result);
        }

        [Theory(DisplayName = "Verify that given a invalid model, should throw an exception")]
        [InlineData("Name")]
        [InlineData("Description")]
        public void GivenInvalidCreateCandidateProfileContract_ShouldThrowException(string propertyName)
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var invalidCreateCandidateProfileContract = _createCandidateProfileContract.WithPropertyValue(propertyName, null);

            //Act
            Exception ex = Assert.Throws<CreateContractInvalidException>(() => _candidateProfileService.Create(invalidCreateCandidateProfileContract));

            //Assert
            Assert.IsType<CreateContractInvalidException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"'{propertyName}' must not be empty.", ex.Message);
        }

        [Fact(DisplayName = "Verify that given a model that already exists on the database, throws an exception")]
        public void GivenAlreadyCreatedCreateCandidateProfileContract_ShouldValidateExistanceAndThrowException()
        {
            Context.SetupDatabaseForTesting();
            Context.Profiles.Add(new CandidateProfile { Name = "Testing Testerino" });
            Context.SaveChanges();
            var invalidCreateCandidateProfileContract = _createCandidateProfileContract.WithPropertyValue("Name", GetCandidateProfileFromDatabase().Name);

            Exception ex = Assert.Throws<InvalidCandidateProfileException>(() => _candidateProfileService.Create(invalidCreateCandidateProfileContract));

            Assert.IsType<InvalidCandidateProfileException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"The Profile already exists .", ex.Message);
        }


        [Fact]
        public void GivenCandidateProfileServiceDelete_WhenIdParameterIsValid_ShouldDeleteEntityInDatabase()
        {
            Context.SetupDatabaseForTesting();
            Context.SeedDatabaseWith(_candidateProfile);
            int candidateProfileCountBeforeDelete = GetProfilesCount(); 

            _candidateProfileService.Delete(_candidateProfile.Id);
            int candidateProfileCountAfterDelete = GetProfilesCount();

            Assert.Equal(1, candidateProfileCountBeforeDelete);
            Assert.Equal(0, candidateProfileCountAfterDelete);
            Assert.NotEqual(candidateProfileCountAfterDelete, candidateProfileCountBeforeDelete);
        }

        [Fact]
        public void GivenCandidateProfileServiceDelete_WhenIdParameterIsInvalid_ShouldThrowException()
        {
            Context.SetupDatabaseForTesting();

            Exception ex = Assert.Throws<DeleteCandidateProfileNotFoundException>(() => _candidateProfileService.Delete(_candidateProfile.Id));

            Assert.IsType<DeleteCandidateProfileNotFoundException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"Profile not found for the Profile Id: { _candidateProfile.Id }", ex.Message);
        }

        [Fact]
        public void GivenCandidateProfileUpdate_WhenModelIsValid_ShouldUpdateModel()
        {
            Context.SetupDatabaseForTesting();
            string newDescription = "An entirely new description";
            var candidateProfile = new CandidateProfile { Name = "Testy Testirino", Description = "This description should change on update" };
            Context.Profiles.Add(candidateProfile);
            Context.SaveChanges();
            Context.Entry(candidateProfile).State = EntityState.Detached; //changes state from unchanged to detached to tracking from context

            var updateCandidateProfile = new UpdateCandidateProfileContract { Id = candidateProfile.Id, Name = candidateProfile.Name, Description = newDescription };

            _candidateProfileService.Update(updateCandidateProfile);
            var candidateProfileUpdated = Context.Profiles.Where(profile => profile.Id == candidateProfile.Id).First();
            Assert.Equal(newDescription, candidateProfileUpdated.Description);
        }

        private int GetProfilesCount()
        {
            return _candidateProfileRepository.Query().Count();
        }

        private CandidateProfile GetCandidateProfileFromDatabase()
        {
            return _candidateProfileRepository
                .Query()
                .AsNoTracking()
                .FirstOrDefault();
        }
    }
}
