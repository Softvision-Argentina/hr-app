using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.User;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.Candidate;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class CandidateServiceTest : BaseDomainTest
    {
        private readonly CandidateService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Process>> _mockRepoProcess;
        private readonly Mock<IRepository<Candidate>> _mockRepoCandidate;        
        private readonly Mock<IRepository<User>> _mockRepoUser;
        private readonly Mock<IRepository<Office>> _mockRepoOffice;
        private readonly Mock<IRepository<Community>> _mockRepoCommunity;
        private readonly Mock<IRepository<CandidateProfile>> _mockRepoCandidateP;
        private readonly Mock<ILog<CandidateService>> _mockLogCandidateService;
        private readonly Mock<UpdateCandidateContractValidator> _mockUpdateCandidateContractValidator;
        private readonly Mock<CreateCandidateContractValidator> _mockCreateCandidateContractValidator;        

        public CandidateServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepoProcess = new Mock<IRepository<Process>>();
            _mockRepoCandidate = new Mock<IRepository<Candidate>>();            
            _mockRepoUser = new Mock<IRepository<User>>();
            _mockRepoOffice = new Mock<IRepository<Office>>();
            _mockRepoCommunity = new Mock<IRepository<Community>>();
            _mockRepoCandidateP = new Mock<IRepository<CandidateProfile>>();
            _mockLogCandidateService = new Mock<ILog<CandidateService>>();
            _mockUpdateCandidateContractValidator = new Mock<UpdateCandidateContractValidator>();
            _mockCreateCandidateContractValidator = new Mock<CreateCandidateContractValidator>();            
            _service = new CandidateService(
                _mockMapper.Object,
                _mockRepoCandidate.Object,
                _mockRepoCommunity.Object,
                _mockRepoCandidateP.Object,                                               
                _mockRepoUser.Object,
                _mockRepoOffice.Object,
                _mockRepoProcess.Object,
                MockUnitOfWork.Object,
                _mockLogCandidateService.Object,
                _mockUpdateCandidateContractValidator.Object,
                _mockCreateCandidateContractValidator.Object                
            );
        }

        [Fact(DisplayName = "Verify that create CandidateService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateCandidateService()
        {
            var contract = new CreateCandidateContract {
                User = new ReadedUserContract { Id = 1 },
                Community = new ReadedCommunityContract { Id = 1 },
                Profile = new ReadedCandidateProfileContract { Id = 1 } 
            };
            var expectedCandidate = new CreatedCandidateContract();
            var userList = new List<User> { new User { Id = 1 } }.AsQueryable();
            var communityList = new List<Community> { new Community { Id = 1 } }.AsQueryable();
            var profileList = new List<CandidateProfile> { new CandidateProfile { Id = 1 } }.AsQueryable();
            _mockCreateCandidateContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Candidate>(It.IsAny<CreateCandidateContract>())).Returns(new Candidate());
            _mockRepoCandidate.Setup(repoCom => repoCom.Create(It.IsAny<Candidate>())).Returns(new Candidate());
            _mockMapper.Setup(mm => mm.Map<CreatedCandidateContract>(It.IsAny<Candidate>())).Returns(expectedCandidate);
            _mockRepoUser.Setup(x => x.Query()).Returns(userList);
            _mockRepoCommunity.Setup(x => x.Query()).Returns(communityList);
            _mockRepoCandidateP.Setup(x => x.Query()).Returns(profileList);

            var createdCandidate = _service.Create(contract);

            Assert.NotNull(createdCandidate);
            Assert.Equal(expectedCandidate, createdCandidate);
            _mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateCandidateContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Candidate>(It.IsAny<CreateCandidateContract>()), Times.Once);
            _mockRepoCandidate.Verify(mrt => mrt.Create(It.IsAny<Candidate>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedCandidateContract>(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateCandidateContract();
            var expectedCandidate = new CreatedCandidateContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateCandidateContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedCandidateContract>(It.IsAny<Candidate>())).Returns(expectedCandidate);

            var exception = Assert.Throws<Model.Exceptions.Candidate.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateCandidateContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Candidate>(It.IsAny<CreateCandidateContract>()), Times.Never);
            _mockRepoCandidate.Verify(mrt => mrt.Create(It.IsAny<Candidate>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedCandidateContract>(It.IsAny<Candidate>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete CandidateService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteCandidateService()
        {
            var communities = new List<Candidate>() { new Candidate() { Id = 1 } }.AsQueryable();
            _mockRepoCandidate.Setup(mrt => mrt.Query()).Returns(communities);

            _service.Delete(1);

            _mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepoCandidate.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepoCandidate.Verify(mrt => mrt.Delete(It.IsAny<Candidate>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteCandidateNotFoundException()
        {
            var expectedErrorMEssage = $"Candidate not found for the CandidateId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Candidate.DeleteCandidateNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepoCandidate.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepoCandidate.Verify(mrt => mrt.Delete(It.IsAny<Candidate>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update CandidateService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {

            var contract = new UpdateCandidateContract
            {
                User = new ReadedUserContract { Id = 1 },
                Community = new ReadedCommunityContract { Id = 1 },
                Profile = new ReadedCandidateProfileContract { Id = 1 },
                PreferredOfficeId = 1
            };

            var userList = new List<User> { new User { Id = 1 } }.AsQueryable();
            var communityList = new List<Community> { new Community { Id = 1 } }.AsQueryable();
            var profileList = new List<CandidateProfile> { new CandidateProfile { Id = 1 } }.AsQueryable();
            var officeList = new List<Office> { new Office { Id = 1 } }.AsQueryable();
            _mockUpdateCandidateContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Candidate>(It.IsAny<UpdateCandidateContract>())).Returns(new Candidate());
            _mockRepoUser.Setup(x => x.Query()).Returns(userList);
            _mockRepoCommunity.Setup(x => x.Query()).Returns(communityList);
            _mockRepoCandidateP.Setup(x => x.Query()).Returns(profileList);
            _mockRepoOffice.SetupSequence(x => x.Query()).Returns(officeList);

            _service.Update(contract);

            _mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateCandidateContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Candidate>(It.IsAny<UpdateCandidateContract>()), Times.Once);
            _mockRepoCandidate.Verify(mrt => mrt.Update(It.IsAny<Candidate>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateCandidateContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateCandidateContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<Candidate>(It.IsAny<UpdateCandidateContract>())).Returns(new Candidate());

            var exception = Assert.Throws<Model.Exceptions.Candidate.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateCandidateContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Candidate>(It.IsAny<UpdateCandidateContract>()), Times.Never);
            _mockRepoCandidate.Verify(mrt => mrt.Update(It.IsAny<Candidate>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<Candidate>() { new Candidate() { Id = 1 } }.AsQueryable();
            var readedCandidateList = new List<ReadedCandidateContract> { new ReadedCandidateContract { Id = 1 } };
            _mockRepoCandidate.Setup(mrt => mrt.QueryEager()).Returns(communities);
            _mockMapper.Setup(mm => mm.Map<List<ReadedCandidateContract>>(It.IsAny<List<Candidate>>())).Returns(readedCandidateList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedCandidateContract>>(It.IsAny<List<Candidate>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<Candidate>() { new Candidate() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCandidate = new ReadedCandidateContract { Id = 1, Name = "Name" };
            _mockRepoCandidate.Setup(mrt => mrt.QueryEager()).Returns(communities);
            _mockMapper.Setup(mm => mm.Map<ReadedCandidateContract>(It.IsAny<Candidate>())).Returns(readedCandidate);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            _mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedCandidateContract>(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read with filter rule returns a value")]
        public void GivenReadWithFilterRule_WhenRegularCall_ReturnsValue()
        {
            var candidateList = new List<Candidate>() { new Candidate() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCandidateList = new List<ReadedCandidateContract> { new ReadedCandidateContract { Id = 1, Name = "Name" } };
            _mockRepoCandidate.Setup(mrt => mrt.QueryEager()).Returns(candidateList);
            _mockMapper.Setup(mm => mm.Map<List<ReadedCandidateContract>>(It.IsAny<List<Candidate>>())).Returns(readedCandidateList);
            Func<Candidate, bool> filter = candidate => true;

            var actualResult = _service.Read(filter);

            Assert.NotNull(actualResult);            
            _mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedCandidateContract>>(It.IsAny<List<Candidate>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that Exists returns a value")]
        public void GivenExists_WhenRegularCall_ReturnsValue()
        {
            var candidateList = new List<Candidate>() { new Candidate() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCandidate = new ReadedCandidateContract { Id = 1, Name = "Name" };
            _mockRepoCandidate.Setup(mrt => mrt.QueryEager()).Returns(candidateList);
            _mockMapper.Setup(mm => mm.Map<ReadedCandidateContract>(It.IsAny<Candidate>())).Returns(readedCandidate);            

            var actualResult = _service.Exists(1);

            Assert.NotNull(actualResult);
            _mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedCandidateContract>(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that ListApp returns a value")]
        public void GivenListApp_WhenRegularCall_ReturnsValue()
        {
            var candidateList = new List<Candidate>() { new Candidate() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCandidateList = new List<ReadedCandidateAppContract> { new ReadedCandidateAppContract () };
            _mockRepoCandidate.Setup(mrt => mrt.QueryEager()).Returns(candidateList);
            _mockMapper.Setup(mm => mm.Map<List<ReadedCandidateAppContract>>(It.IsAny<List<Candidate>>())).Returns(readedCandidateList);

            var actualResult = _service.ListApp();

            Assert.NotNull(actualResult);
            _mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedCandidateAppContract>>(It.IsAny<List<Candidate>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetCandidate returns a value")]
        public void GivenGetCandidate_WhenRegularCall_ReturnsValue()
        {
            var candidateList = new List<Candidate>() { new Candidate() { Id = 1 } }.AsQueryable();            
            _mockRepoCandidate.Setup(x => x.QueryEager()).Returns(candidateList);            

            var actualResult = _service.GetCandidate(1);

            Assert.NotNull(actualResult);            
            _mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);            
        }
    }
}