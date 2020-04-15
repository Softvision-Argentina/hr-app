using AutoMapper;
using Core.Persistance;
using Domain.Services.Impl.IntegrationTests.Core;
using Domain.Model;
using Domain.Model.Exceptions.Candidate;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.User;
using Domain.Services.Interfaces.Services;
using System.Linq;
using Xunit;
using Persistance.EF.Extensions;

namespace Domain.Services.Impl.IntegrationTests.Services
{
    public class CandidateServiceIntegrationTest : BaseServiceIntegrationTest
    {
        private readonly ICandidateService _candidateService;
        private readonly IUserService _userService;
        private readonly ICommunityService _communityService;
        private readonly ICandidateProfileService _candidateProfileService;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IMapper _mapper;
        public CandidateServiceIntegrationTest(ServiceFixture serviceFixture) : base(serviceFixture)
        {
            _candidateRepository = Services.GetService(typeof(IRepository<Candidate>)) as IRepository<Candidate>;
            _candidateService = Services.GetService(typeof(ICandidateService)) as ICandidateService;
            _userService = Services.GetService(typeof(IUserService)) as IUserService;
            _communityService = Services.GetService(typeof(ICommunityService)) as ICommunityService;
            _candidateProfileService = Services.GetService(typeof(ICandidateProfileService)) as ICandidateProfileService;
            _mapper = Services.GetService(typeof(IMapper)) as IMapper;
        }

        [Fact(DisplayName = "Verify that given a valid model, should add to the database")]
        public void GivenValidCreateCandidateContract_ShouldCreateItWithoutProblems()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var user = new User();
            Context.SeedDatabaseWith(user);
            var profile = new CandidateProfile();
            Context.SeedDatabaseWith(profile);
            var community = new Community();
            community.ProfileId = profile.Id;
            Context.SeedDatabaseWith(community);
            
            var userContract = DataFactory.CreateInstance<ReadedUserContract>();
            var communityContract = DataFactory.CreateInstance<ReadedCommunityContract>();
            var profileContract = DataFactory.CreateInstance<ReadedCandidateProfileContract>();

            userContract.Id = user.Id;
            communityContract.Id = community.Id;
            profileContract.Id = profile.Id;

            var candidateContract = DataFactory.CreateInstance<CreateCandidateContract>();
            candidateContract.User = userContract;
            candidateContract.Community = communityContract;
            candidateContract.Profile = profileContract;

            //Act
            var result = _candidateService.Create(candidateContract);

            //Assert
            Assert.NotNull(result);
        }

        [Theory(DisplayName = "Verify that given a invalid model, should throw an exception")]
        [InlineData("Community")]
        [InlineData("User")]
        [InlineData("CandidateProfile")]
        public void GivenInvalidCreateCandidateContract_ShouldThrowException(string propertyName)
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var invalidCreateCandidateContract =
                DataFactory.CreateInstance<CreateCandidateContract>()
                .WithPropertyValue(propertyName, null);

            //Act
            var ex = Assert.Throws<CreateContractInvalidException>(() => _candidateService.Create(invalidCreateCandidateContract));

            //Assert
            Assert.IsType<CreateContractInvalidException>(ex);
            Assert.NotNull(ex);
        }

        [Fact(DisplayName = "Verify that given a model that already exists on the database, throws an exception")]
        public void GivenAlreadyCreatedCreateCandidateContract_ShouldValidateExistanceAndThrowException()
        {
            //Arrange
            Context.SetupDatabaseForTesting();

            var candidate = new Candidate();
            candidate.EmailAddress = "test@test.com";
            Context.SeedDatabaseWith(candidate);

            var userContract = DataFactory.CreateInstance<ReadedUserContract>();
            var communityContract = DataFactory.CreateInstance<ReadedCommunityContract>();
            var profileContract = DataFactory.CreateInstance<ReadedCandidateProfileContract>();

            var invalidCreateCandidateContract = DataFactory.CreateInstance<CreateCandidateContract>()
                .WithPropertyValue("EmailAddress", candidate.EmailAddress)
                .WithPropertyValue("User", userContract)
                .WithPropertyValue("Profile", profileContract)
                .WithPropertyValue("Community", communityContract);

            //Act
            var ex = Assert.Throws<InvalidCandidateException>(() => _candidateService.Create(invalidCreateCandidateContract));

            //Assert
            Assert.IsType<InvalidCandidateException>(ex);
            Assert.NotNull(ex);
        }
    }
}
