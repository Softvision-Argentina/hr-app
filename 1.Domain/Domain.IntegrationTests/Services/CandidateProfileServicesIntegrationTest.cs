using Domain.Model;
using Domain.Model.Exceptions.CandidateProfile;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Persistance.EF.Extensions;
using System.Linq;
using Core.Persistance;
using Core.Persistance.Testing;
using Domain.Services.Impl.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Hosting.Internal;
using Xunit;
using Xunit.Abstractions;

namespace Domain.Services.Impl.IntegrationTests.Services
{
    [Collection(nameof(EnvironmentType.Integration))]
    public class CandidateProfileServicesIntegrationTest : IClassFixture<CandidateProfileFixture>
    {
        private readonly CandidateProfileFixture _fixture;
        private readonly ICandidateProfileService _candidateProfileService;
        public CandidateProfileServicesIntegrationTest(CandidateProfileFixture fixture)
        {
            _fixture = fixture;
            _candidateProfileService =
                (ICandidateProfileService)fixture.Services.GetService(typeof(ICandidateProfileService));
        }

        
        [Fact(DisplayName = "Verify that given a valid model, should add to the database")]
        public void GivenCandidateProfileServiceCreate_WhenSendingValidData_ShouldAppendModelToDatabase()
        {
            //Arrange
            var recordCountBeforeCreate = _fixture.GetCount<CandidateProfile>();
            var validCreateCandidateProfileContract = DataFactory.CreateInstance<CreateCandidateProfileContract>();

            //Act
            var result = _candidateProfileService.Create(validCreateCandidateProfileContract);
            var recordCountAfterCreate = _fixture.GetCount<CandidateProfile>();

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(recordCountBeforeCreate, recordCountAfterCreate);
            Assert.Equal(recordCountBeforeCreate + 1, recordCountAfterCreate);

            //Clean
            _fixture.Delete<CandidateProfile>();
        }

        [Theory(DisplayName = "Verify that given a invalid model, should throw an exception")]
        [InlineData("Name")]
        [InlineData("Description")]
        public void GivenCandidateProfileServiceCreate_WhenSendingInvalidData_ShouldThrowException(string propertyName)
        {
            //Arrange
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
            //Arrange
            var candidateProfile = new CandidateProfile() {Name = "Testing"};
            _fixture.Seed(candidateProfile);
            var invalidCreateCandidateProfileContract = DataFactory.CreateInstance<CreateCandidateProfileContract>()
                .WithPropertyValue("Name", _fixture.Get<CandidateProfile>(candidateProfile.Id).Name);

            //Act
            var ex = Assert.Throws<InvalidCandidateProfileException>(() => _candidateProfileService.Create(invalidCreateCandidateProfileContract));

            //Assert
            Assert.IsType<InvalidCandidateProfileException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"The Profile already exists .", ex.Message);

            //Clean
            _fixture.Delete<CandidateProfile>();
        }

        [Fact(DisplayName = "Verify that given a valid Id, should delete the entity in database")]
        public void GivenCandidateProfileServiceDelete_WhenIdParameterIsValid_ShouldDeleteEntityInDatabase()
        {
            //Arrange
            var candidateProfile = new CandidateProfile() { Name = "Testing" };
            _fixture.Seed(candidateProfile);
            var candidateProfileCountBeforeDelete = _fixture.GetCount<CandidateProfile>();

            //Act
            _candidateProfileService.Delete(candidateProfile.Id);
            var candidateProfileCountAfterDelete = _fixture.GetCount<CandidateProfile>();

            //Assert
            Assert.Equal(candidateProfileCountBeforeDelete - 1, candidateProfileCountAfterDelete);
            Assert.NotEqual(candidateProfileCountBeforeDelete, candidateProfileCountAfterDelete);

            //Clean
            _fixture.Delete<CandidateProfile>();
        }

        [Fact(DisplayName = "Verify that given a invalid Id, delete method should throw a exception")]
        public void GivenCandidateProfileServiceDelete_WhenIdParameterIsInvalid_ShouldThrowException()
        {
            //Arrange
            var candidateProfile = DataFactory.CreateInstance<CandidateProfile>();

            //Act
            var ex = Assert.Throws<DeleteCandidateProfileNotFoundException>(() => _candidateProfileService.Delete(candidateProfile.Id));

            //Assert
            Assert.IsType<DeleteCandidateProfileNotFoundException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"Profile not found for the Profile Id: { candidateProfile.Id }", ex.Message);
        }

        [Fact(DisplayName = "Verify that given a valid data, should update entity in database")]
        public void GivenCandidateProfileUpdate_WhenModelIsValid_ShouldUpdateModel()
        {
            //Arrange
            var candidateProfile = new CandidateProfile() { Name = "Testing" };
            _fixture.Seed(candidateProfile);
            var newDescription = "An entirely new description";
            var updateCandidateProfile = new UpdateCandidateProfileContract
            {
                Id = candidateProfile.Id,
                Name = candidateProfile.Name,
                Description = newDescription
            };

            //Act
            _candidateProfileService.Update(updateCandidateProfile);

            //Assert
            var candidateProfileUpdated = _fixture.Context.Profiles.AsNoTracking().First(profile => profile.Id == candidateProfile.Id);
            Assert.Equal(newDescription, candidateProfileUpdated.Description);

            //Clean
            _fixture.Delete<CandidateProfile>();
        }

        [Theory(DisplayName = "Verify that given invalid properties values, should throw exceptions")]
        [InlineData("Id")]
        [InlineData("Name")]
        [InlineData("Description")]
        public void GivenCandidateProfileUpdate_WhenModelIsNotValid_ShouldThrowsException(string property)
        {
            //Arrange
            var candidateProfile = DataFactory.CreateInstance<UpdateCandidateProfileContract>()
                .WithPropertyValue(property, default);

            //Act
            var ex = Assert.Throws<CreateContractInvalidException>(() => _candidateProfileService.Update(candidateProfile));

            //Assert
            Assert.IsType<CreateContractInvalidException>(ex);
            Assert.NotNull(ex);
        }

        [Fact(DisplayName = "Verify that given entities that are already saved in the database, should throw exception to avoid duplicated entries")]
        public void GivenCandidateProfileUpdate_WhenModelIsAlreadyInDatabase_ShouldThrowException()
        {
            //Arrange
            var profileInDb = new CandidateProfile() { Name = "Testing" };
            _fixture.Seed(profileInDb);

            var candidateProfileFromDatabase = _fixture.Get<CandidateProfile>(profileInDb.Id);
            var updateCandidateProfile = new UpdateCandidateProfileContract { Id = 999, Name = candidateProfileFromDatabase.Name, Description = "Description" };

            //Act
            var ex = Assert.Throws<InvalidCandidateProfileException>(() => _candidateProfileService.Update(updateCandidateProfile));

            //Assert
            Assert.Equal("The Profile already exists .", ex.Message);
            Assert.IsType<InvalidCandidateProfileException>(ex);
            Assert.NotNull(ex);
                
            //Clean
            _fixture.Delete<CandidateProfile>();
        }
    }
}
