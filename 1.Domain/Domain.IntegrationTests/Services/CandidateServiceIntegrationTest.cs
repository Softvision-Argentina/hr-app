using AutoMapper;
using Core.Persistance;
using Domain.Services.Impl.IntegrationTests.Core;
using Domain.Model;
using Domain.Model.Exceptions.Candidate;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.Consultant;
using Domain.Services.Interfaces.Services;
using System.Linq;
using Xunit;
using Persistance.EF.Extensions;

namespace Domain.Services.Impl.IntegrationTests.Services
{
    public class CandidateServiceIntegrationTest : BaseServiceIntegrationTest
    {
        private readonly ICandidateService _candidateService;
        private readonly IConsultantService _consultantService;
        private readonly ICommunityService _communityService;
        private readonly ICandidateProfileService _candidateProfileService;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IMapper _mapper;
        public CandidateServiceIntegrationTest(ServiceFixture serviceFixture) : base(serviceFixture)
        {
            _candidateRepository = Services.GetService(typeof(IRepository<Candidate>)) as IRepository<Candidate>;
            _candidateService = Services.GetService(typeof(ICandidateService)) as ICandidateService;
            _consultantService = Services.GetService(typeof(IConsultantService)) as IConsultantService;
            _communityService = Services.GetService(typeof(ICommunityService)) as ICommunityService;
            _candidateProfileService = Services.GetService(typeof(ICandidateProfileService)) as ICandidateProfileService;
            _mapper = Services.GetService(typeof(IMapper)) as IMapper;
        }

        [Fact(DisplayName = "Verify that given a valid model, should add to the database")]
        public void GivenValidCreateCandidateContract_ShouldCreateItWithoutProblems()
        {
            //Arrange
            int recordsCount = GetCandidatesCount();
            var validCreateCandidateContract = DataFactory.CreateInstance<CreateCandidateContract>();

            var validConsultant = DataFactory.CreateInstance<Consultant>();
            var createConsultantContract = _mapper.Map<CreateConsultantContract>(validConsultant);
            CreatedConsultantContract createdConsultantContract = _consultantService.Create(createConsultantContract);
            var readedConsultantContract = _consultantService.Read(createdConsultantContract.Id);

            var validCandidateProfile = DataFactory.CreateInstance<CandidateProfile>();
            var createCandidateProfileContract = _mapper.Map<CreateCandidateProfileContract>(validCandidateProfile);
            CreatedCandidateProfileContract createdCandidateProfileContract = _candidateProfileService.Create(createCandidateProfileContract);
            var readedCandidateProfileContract = _candidateProfileService.Read(createdCandidateProfileContract.Id);

            var validCommunity = DataFactory.CreateInstance<Community>();
            var createCommunityContract = _mapper.Map<CreateCommunityContract>(validCommunity);
            createCommunityContract.ProfileId = readedCandidateProfileContract.Id;
            CreatedCommunityContract createdCommunityContract = _communityService.Create(createCommunityContract);
            var readedCommunityContract = _communityService.Read(createdCommunityContract.Id);

            

            validCreateCandidateContract.Recruiter = readedConsultantContract;
            validCreateCandidateContract.Community = readedCommunityContract;
            validCreateCandidateContract.Profile = readedCandidateProfileContract;

            //Act
            CreatedCandidateContract result = _candidateService.Create(validCreateCandidateContract);

            //Assert
            Assert.Equal(recordsCount + 1, GetCandidatesCount());
            Assert.NotNull(result);
        }

        [Theory(DisplayName = "Verify that given a invalid model, should throw an exception")]
        [InlineData("Community")]
        [InlineData("Recruiter")]
        [InlineData("CandidateProfile")]
        public void GivenInvalidCreateCandidateContract_ShouldThrowException(string propertyName)
        {
            //Arrange
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
            var invalidCreateCandidateContract =
                DataFactory.CreateInstance<CreateCandidateContract>()
                .WithPropertyValue("EmailAddress", GetCandidateFromDatabase().EmailAddress);

            var validConsultant = DataFactory.CreateInstance<Consultant>();
            var createConsultantContract = _mapper.Map<CreateConsultantContract>(validConsultant);
            CreatedConsultantContract createdConsultantContract = _consultantService.Create(createConsultantContract);
            var readedConsultantContract = _consultantService.Read(createdConsultantContract.Id);

            var validCandidateProfile = DataFactory.CreateInstance<CandidateProfile>();
            var createCandidateProfileContract = _mapper.Map<CreateCandidateProfileContract>(validCandidateProfile);
            CreatedCandidateProfileContract createdCandidateProfileContract = _candidateProfileService.Create(createCandidateProfileContract);
            var readedCandidateProfileContract = _candidateProfileService.Read(createdCandidateProfileContract.Id);

            var validCommunity = DataFactory.CreateInstance<Community>();
            var createCommunityContract = _mapper.Map<CreateCommunityContract>(validCommunity);
            createCommunityContract.ProfileId = readedCandidateProfileContract.Id;
            CreatedCommunityContract createdCommunityContract = _communityService.Create(createCommunityContract);
            var readedCommunityContract = _communityService.Read(createdCommunityContract.Id);

            invalidCreateCandidateContract.Recruiter = readedConsultantContract;
            invalidCreateCandidateContract.Community = readedCommunityContract;
            invalidCreateCandidateContract.Profile = readedCandidateProfileContract;

            var ex = Assert.Throws<InvalidCandidateException>(() => _candidateService.Create(invalidCreateCandidateContract));

            Assert.IsType<InvalidCandidateException>(ex);
            Assert.NotNull(ex);
        }

        private int GetCandidatesCount()
        {
            return _candidateRepository.Query().Count();
        }

        private Candidate GetCandidateFromDatabase()
        {
            return _candidateRepository.Query().FirstOrDefault();
        }
    }
}
