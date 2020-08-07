// <copyright file="CandidateProfileServicesIntegrationTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

using Core.Testing.Platform;
using Domain.Model;
using Domain.Model.Exceptions.CandidateProfile;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Impl.IntegrationTests.Fixtures;
using Domain.Services.Interfaces.Services;
using Persistance.EF.Extensions;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Domain.Services.Impl.IntegrationTests.Services
{
    [Collection(nameof(TestType.Integration))]
    public class CandidateProfileServicesIntegrationTest : IClassFixture<CandidateProfileFixture>
    {
        private readonly CandidateProfileFixture fixture;

        public CandidateProfileServicesIntegrationTest(CandidateProfileFixture fixture)
        {
            this.fixture = fixture;
            this.fixture.ContextAction((context) =>
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            });
        }

        [Fact(DisplayName = "Verify that given a valid model, should add to the database")]
        public void GivenCandidateProfileServiceCreate_WhenSendingValidData_ShouldAppendModelToDatabase()
        {
            // Arrange
            var recordCountBeforeCreate = this.fixture.GetCount<CandidateProfile>();
            var validCreateCandidateProfileContract = DataFactory.CreateInstance<CreateCandidateProfileContract>();

            CreatedCandidateProfileContract result = null;

            this.fixture.UseService<ICandidateProfileService>((service) =>
            {
                result = service.Create(validCreateCandidateProfileContract);
            });

            var recordCountAfterCreate = this.fixture.GetCount<CandidateProfile>();

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(recordCountBeforeCreate, recordCountAfterCreate);
            Assert.Equal(recordCountBeforeCreate + 1, recordCountAfterCreate);
            Assert.True(result.Id > 0);
        }

        [Theory(DisplayName = "Verify that given a invalid model, should throw an exception")]
        [InlineData("Name")]
        [InlineData("Description")]
        public void GivenCandidateProfileServiceCreate_WhenSendingInvalidData_ShouldThrowException(string propertyName)
        {
            // Arrange
            var invalidCreateCandidateProfileContract = DataFactory.CreateInstance<CreateCandidateProfileContract>()
                .WithPropertyValue(propertyName, null);

            // Act
            var ex = Assert.Throws<CreateContractInvalidException>(() =>
            {
                this.fixture.UseService<ICandidateProfileService>((service) =>
                {
                    service.Create(invalidCreateCandidateProfileContract);
                });
            });

            // Assert
            Assert.IsType<CreateContractInvalidException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"'{propertyName}' must not be empty.", ex.Message);
        }

        [Fact(DisplayName = "Verify that given a model that already exists on the database, throws an exception")]
        public void GivenCandidateProfileServiceCreate_WhenHavingSameDataInDatabase_ShouldValidateExistanceAndThrowException()
        {
            // Arrange
            var candidateProfile = new CandidateProfile() { Name = "Testing" };
            this.fixture.Seed(candidateProfile);
            var invalidCreateCandidateProfileContract = DataFactory.CreateInstance<CreateCandidateProfileContract>()
                .WithPropertyValue("Name", this.fixture.Get<CandidateProfile>(candidateProfile.Id).Name);

            // Act
            var ex = Assert.Throws<InvalidCandidateProfileException>(() =>
            {
                this.fixture.UseService<ICandidateProfileService>((service) =>
                {
                    service.Create(invalidCreateCandidateProfileContract);
                });
            });

            // Assert
            Assert.IsType<InvalidCandidateProfileException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"The Profile already exists .", ex.Message);
        }

        [Fact(DisplayName = "Verify that given a valid Id, should delete the entity in database")]
        public void GivenCandidateProfileServiceDelete_WhenIdParameterIsValid_ShouldDeleteEntityInDatabase()
        {
            // Arrange
            var candidateProfile = new CandidateProfile() { Name = "Testing" };
            this.fixture.Seed(candidateProfile);
            var candidateProfileCountBeforeDelete = this.fixture.GetCount<CandidateProfile>();
            var candidateExpectedAfterDelete = candidateProfileCountBeforeDelete - 1;

            // Act
            this.fixture.UseService<ICandidateProfileService>((service) =>
            {
                service.Delete(candidateProfile.Id);
            });

            var candidateProfileCountAfterDelete = this.fixture.GetCount<CandidateProfile>();

            // Assert
            Assert.Equal(candidateExpectedAfterDelete, candidateProfileCountAfterDelete);
            Assert.NotEqual(candidateProfileCountBeforeDelete, candidateProfileCountAfterDelete);
        }

        [Fact(DisplayName = "Verify that given a invalid Id, delete method should throw a exception")]
        public void GivenCandidateProfileServiceDelete_WhenIdParameterIsInvalid_ShouldThrowException()
        {
            // Arrange
            var candidateProfile = DataFactory.CreateInstance<CandidateProfile>();

            // Act
            var ex = Assert.Throws<DeleteCandidateProfileNotFoundException>(() =>
            {
                this.fixture.UseService<ICandidateProfileService>((service) =>
                {
                    service.Delete(candidateProfile.Id);
                });
            });

            // Assert
            Assert.IsType<DeleteCandidateProfileNotFoundException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"Profile not found for the Profile Id: {candidateProfile.Id }", ex.Message);
        }

        [Fact(DisplayName = "Verify that given a valid data, should update entity in database")]
        public void GivenCandidateProfileUpdate_WhenModelIsValid_ShouldUpdateModel()
        {
            // Arrange
            var candidateProfile = new CandidateProfile() { Name = "Testing" };
            this.fixture.Seed(candidateProfile);
            var newDescription = "An entirely new description";
            var updateCandidateProfile = new UpdateCandidateProfileContract
            {
                Id = candidateProfile.Id,
                Name = candidateProfile.Name,
                Description = newDescription,
            };

            // Act
            this.fixture.UseService<ICandidateProfileService>((service) =>
            {
                service.Update(updateCandidateProfile);
            });

            // Assert
            var candidateProfileUpdated = this.fixture.Get<CandidateProfile>(candidateProfile.Id);
            Assert.Equal(newDescription, candidateProfileUpdated.Description);
        }

        [Theory(DisplayName = "Verify that given invalid properties values, should throw exceptions")]
        [InlineData("Id")]
        [InlineData("Name")]
        [InlineData("Description")]
        public void GivenCandidateProfileUpdate_WhenModelIsNotValid_ShouldThrowsException(string property)
        {
            // Arrange
            var candidateProfile = DataFactory.CreateInstance<UpdateCandidateProfileContract>()
                .WithPropertyValue(property, default(string));

            // Act
            var ex = Assert.Throws<CreateContractInvalidException>(() =>
            {
                this.fixture.UseService<ICandidateProfileService>((service) =>
                {
                    service.Update(candidateProfile);
                });
            });

            // Assert
            Assert.IsType<CreateContractInvalidException>(ex);
            Assert.NotNull(ex);
        }

        [Fact(DisplayName = "Verify that given entities that are already saved in the database, should throw exception to avoid duplicated entries")]
        public void GivenCandidateProfileUpdate_WhenModelIsAlreadyInDatabase_ShouldThrowException()
        {
            // Arrange
            var profileInDb = new CandidateProfile() { Name = "Testing" };
            this.fixture.Seed(profileInDb);

            var candidateProfileFromDatabase = this.fixture.Get<CandidateProfile>(profileInDb.Id);
            var updateCandidateProfile = new UpdateCandidateProfileContract { Id = 999, Name = candidateProfileFromDatabase.Name, Description = "Description" };

            // Act
            var ex = Assert.Throws<InvalidCandidateProfileException>(() =>
            {
                this.fixture.UseService<ICandidateProfileService>((service) =>
                {
                    service.Update(updateCandidateProfile);
                });
            });

            // Assert
            Assert.Equal("The Profile already exists .", ex.Message);
            Assert.IsType<InvalidCandidateProfileException>(ex);
            Assert.NotNull(ex);
        }
    }
}
