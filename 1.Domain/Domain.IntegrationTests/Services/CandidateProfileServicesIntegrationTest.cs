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

        public CandidateProfileServicesIntegrationTest(ServiceFixture serviceFixture) : base(serviceFixture)
        {
            _candidateProfileRepository = Services.GetService(typeof(IRepository<CandidateProfile>)) as IRepository<CandidateProfile>;
            _candidateProfileService = Services.GetService(typeof(ICandidateProfileService)) as ICandidateProfileService;
        }

        [Fact(DisplayName = "Verify that given a valid model, should add to the database")]
        public void GivenCandidateProfileServiceCreate_WhenSendingValidData_ShouldAppendModelToDatabase()
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var recordCountBeforeCreate = GetProfilesCount();
            var validCreateCandidateProfileContract = DataFactory.CreateInstance<CreateCandidateProfileContract>();

            //Act
            CreatedCandidateProfileContract result = _candidateProfileService.Create(validCreateCandidateProfileContract);
            var recordCountAfterCreate = GetProfilesCount();

            //Assert
            Assert.NotEqual(recordCountBeforeCreate, recordCountAfterCreate);
            Assert.Equal(1, recordCountAfterCreate);
            Assert.NotNull(result);
        }

        [Theory(DisplayName = "Verify that given a invalid model, should throw an exception")]
        [InlineData("Name")]
        [InlineData("Description")]
        public void GivenCandidateProfileServiceCreate_WhenSendingInvalidata_ShouldThrowException(string propertyName)
        {
            //Arrange
            Context.SetupDatabaseForTesting();
            var invalidCreateCandidateProfileContract = DataFactory.CreateInstance<CreateCandidateProfileContract>()
                .WithPropertyValue(propertyName, null);

            //Act
            var ex = Assert.Throws<CreateContractInvalidException>(() => _candidateProfileService.Create(invalidCreateCandidateProfileContract));

            //Assert
            Assert.IsType<CreateContractInvalidException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"'{propertyName}' must not be empty.", ex.Message);
        }

        [Fact(DisplayName = "Verify that given a model that already exists on the database, throws an exception")]
        public void GivenCandidateProfileServiceCreate_WhenHavingSameDataInDatabase_ShouldValidateExistanceAndThrowException()
        {
            Context.SetupDatabaseForTesting();
            Context.SeedDatabaseWith(new CandidateProfile { Name = "Testing Testerino" });
            var invalidCreateCandidateProfileContract = DataFactory.CreateInstance<CreateCandidateProfileContract>()
                .WithPropertyValue("Name", GetCandidateProfileFromDatabase().Name);

            var ex = Assert.Throws<InvalidCandidateProfileException>(() => _candidateProfileService.Create(invalidCreateCandidateProfileContract));

            Assert.IsType<InvalidCandidateProfileException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"The Profile already exists .", ex.Message);
        }

        [Fact(DisplayName = "Verify that given a valid Id, should delete the entity in database")]
        public void GivenCandidateProfileServiceDelete_WhenIdParameterIsValid_ShouldDeleteEntityInDatabase()
        {
            Context.SetupDatabaseForTesting();
            var candidateProfile = DataFactory.CreateInstance<CandidateProfile>();
            Context.SeedDatabaseWith(candidateProfile);
            var candidateProfileCountBeforeDelete = GetProfilesCount(); 

            _candidateProfileService.Delete(candidateProfile.Id);
            var candidateProfileCountAfterDelete = GetProfilesCount();

            Assert.Equal(1, candidateProfileCountBeforeDelete);
            Assert.Equal(0, candidateProfileCountAfterDelete);
        }

        [Fact(DisplayName = "Verify that given a invalid Id, delete method should throw a exception")]
        public void GivenCandidateProfileServiceDelete_WhenIdParameterIsInvalid_ShouldThrowException()
        {
            Context.SetupDatabaseForTesting();
            var candidateProfile = DataFactory.CreateInstance<CandidateProfile>();

            var ex = Assert.Throws<DeleteCandidateProfileNotFoundException>(() => _candidateProfileService.Delete(candidateProfile.Id));

            Assert.IsType<DeleteCandidateProfileNotFoundException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"Profile not found for the Profile Id: { candidateProfile.Id }", ex.Message);
        }

        [Fact(DisplayName = "Verify that given a valid data, should update entity in database")]
        public void GivenCandidateProfileUpdate_WhenModelIsValid_ShouldUpdateModel()
        {
            Context.SetupDatabaseForTesting();
            string newDescription = "An entirely new description";
            var candidateProfile = new CandidateProfile { Name = "Testy Testirino", Description = "This description should change on update" };
            Context.SeedDatabaseWith(candidateProfile);
            Context.Entry(candidateProfile).State = EntityState.Detached;
            var updateCandidateProfile = new UpdateCandidateProfileContract { Id = candidateProfile.Id, Name = candidateProfile.Name, Description = newDescription };

            _candidateProfileService.Update(updateCandidateProfile);

            var candidateProfileUpdated = Context.Profiles.Where(profile => profile.Id == candidateProfile.Id).First();
            Assert.Equal(newDescription, candidateProfileUpdated.Description);
        }

        [Theory(DisplayName = "Verify that given invalid properties values, should throw exceptions")]
        [InlineData("Id")]
        [InlineData("Name")]
        [InlineData("Description")]
        public void GivenCandidateProfileUpdate_WhenModelIsNotValid_ShouldThrowsException(string property)
        {
            Context.SetupDatabaseForTesting();
            var candidateProfile = DataFactory.CreateInstance<UpdateCandidateProfileContract>()
                .WithPropertyValue(property, default);

            var ex = Assert.Throws<CreateContractInvalidException>(() => _candidateProfileService.Update(candidateProfile));

            Assert.IsType<CreateContractInvalidException>(ex);
            Assert.NotNull(ex);
        }

        [Fact(DisplayName = "Verify that given entities that are already saved in the database, should throw exception to avoid duplicated entries")]
        public void GivenCandidateProfileUpdate_WhenModelisAlreadyInDatabase_ShouldThrowException()
        {
            Context.SetupDatabaseForTesting();
            var candidateProfile = new CandidateProfile { Name = "Testy Testirino", Description = "This description should not change on update" };
            Context.SeedDatabaseWith(candidateProfile);
            Context.Entry(candidateProfile).State = EntityState.Detached;
            var updateCandidateProfile = new UpdateCandidateProfileContract { Id = 1, Name = candidateProfile.Name, Description = "Description" };
            
            var ex = Assert.Throws<InvalidCandidateProfileException>(() => _candidateProfileService.Update(updateCandidateProfile));

            Assert.Equal("The Profile already exists .", ex.Message);
            Assert.IsType<InvalidCandidateProfileException>(ex);
            Assert.NotNull(ex);
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
