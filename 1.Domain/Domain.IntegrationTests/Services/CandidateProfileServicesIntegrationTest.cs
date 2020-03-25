using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.CandidateProfile;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Interfaces.Services;
using System;
using System.Linq;
using Xunit;
using Domain.Services.Impl.IntegrationTests.Dummy;
using Core;

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
        public void GivenValidCreateCandidateProfileContract_ShouldCreateItWithoutProblems()
        {
            //Arrange
            int recordsCount = GetProfilesCount();
            var validCreateCandidateProfileContract = DataFactory.CreateInstance<CreateCandidateProfileContract>();

            //Act
            CreatedCandidateProfileContract result = _candidateProfileService.Create(validCreateCandidateProfileContract);

            //Assert
            Assert.Equal(recordsCount + 1, GetProfilesCount());
            Assert.NotNull(result);
        }

        [Theory(DisplayName = "Verify that given a invalid model, should throw an exception")]
        [InlineData("Name")]
        [InlineData("Description")]
        public void GivenInvalidCreateCandidateProfileContract_ShouldThrowException(string propertyName)
        {
            //Arrange
            var invalidCreateCandidateProfileContract = 
                DataFactory.CreateInstance<CreateCandidateProfileContract>()
                .WithPropertyValue(propertyName, null);

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
            var invalidCreateCandidateProfileContract =
                DataFactory.CreateInstance<CreateCandidateProfileContract>()
                .WithPropertyValue("Name", GetCandidateProfileFromDatabase().Name);

            Exception ex = Assert.Throws<InvalidCandidateProfileException>(() => _candidateProfileService.Create(invalidCreateCandidateProfileContract));

            Assert.IsType<InvalidCandidateProfileException>(ex);
            Assert.NotNull(ex);
            Assert.Equal($"The Profile already exists .", ex.Message);
        }

        private int GetProfilesCount()
        {
            return _candidateProfileRepository.Query().Count();
        }

        private CandidateProfile GetCandidateProfileFromDatabase()
        {
            return _candidateProfileRepository.Query().FirstOrDefault();
        }
    }
}
