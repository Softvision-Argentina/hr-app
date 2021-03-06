﻿using AutoMapper;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Model.Exceptions.Office;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Process;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Mailer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class ProcessServiceTest : BaseDomainTest
    {
        private readonly ProcessService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IProcessRepository> _mockRepositoryProcess;
        private readonly Mock<IProcessStageRepository> _mockStageRepository;        
        private readonly Mock<IRepository<Community>> _mockRepositoryCommunity;
        private readonly Mock<IRepository<CandidateProfile>> _mockRepositoryCandidateProfile;
        private readonly Mock<IRepository<Candidate>> _mockRepositoryCandidate;
        private readonly Mock<IRepository<Office>> _mockRepositoryOffice;
        private readonly Mock<IRepository<DeclineReason>> _mockRepositoryDeclineReason;
        private readonly Mock<IHrStageRepository> _mockRepoHrStage;
        private readonly Mock<ITechnicalStageRepository> _mockRepoTechnicalStage;
        private readonly Mock<IClientStageRepository> _mockRepoClientStage;
        private readonly Mock<IPreOfferStageRepository> _mockRepoPreOfferStage;
        private readonly Mock<IOfferStageRepository> _mockRepoOfferStage;
        private readonly Mock<INotificationRepository> _mockRepoINotification;
        private readonly Mock<IRepository<User>> _mockRepoUser;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IHttpContextAccessor> _mockhttpContext;
        private readonly Mock<IMailSender> _mockMailSender;
        private readonly Mock<IValidator<CreateProcessContract>> _mockCreateProcessValidator;
        private readonly Mock<IValidator<UpdateProcessContract>> _mockUpdateProcessValidator;

        public ProcessServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryProcess = new Mock<IProcessRepository>();
            _mockStageRepository = new Mock<IProcessStageRepository>();            
            _mockRepositoryCommunity = new Mock<IRepository<Community>>();
            _mockRepositoryCandidateProfile = new Mock<IRepository<CandidateProfile>>();
            _mockRepositoryCandidate = new Mock<IRepository<Candidate>>();
            _mockRepositoryOffice = new Mock<IRepository<Office>>();
            _mockRepositoryDeclineReason = new Mock<IRepository<DeclineReason>>();
            _mockRepoHrStage = new Mock<IHrStageRepository>();
            _mockRepoTechnicalStage = new Mock<ITechnicalStageRepository>();
            _mockRepoClientStage = new Mock<IClientStageRepository>();
            _mockRepoPreOfferStage = new Mock<IPreOfferStageRepository>();
            _mockRepoOfferStage = new Mock<IOfferStageRepository>();
            _mockRepoINotification = new Mock<INotificationRepository>();
            _mockRepoUser = new Mock<IRepository<User>>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockhttpContext = new Mock<IHttpContextAccessor>();
            _mockMailSender = new Mock<IMailSender>();
            _mockUpdateProcessValidator = new Mock<IValidator<UpdateProcessContract>>();
            _mockCreateProcessValidator = new Mock<IValidator<CreateProcessContract>>();

            _service = new ProcessService(
                _mockMapper.Object,
                _mockRepoUser.Object,
                _mockRepositoryCandidate.Object,
                _mockRepositoryCandidateProfile.Object,
                _mockRepositoryCommunity.Object,
                _mockRepositoryOffice.Object,
                _mockRepositoryDeclineReason.Object,
                _mockRepositoryProcess.Object,
                _mockStageRepository.Object,
                _mockRepoHrStage.Object,
                _mockRepoTechnicalStage.Object,
                _mockRepoClientStage.Object,
                _mockRepoOfferStage.Object,
                MockUnitOfWork.Object,
                _mockRepoINotification.Object,                
                _mockConfiguration.Object,
                _mockhttpContext.Object,
                _mockRepoPreOfferStage.Object,
                _mockMailSender.Object,
                _mockUpdateProcessValidator.Object,
                _mockCreateProcessValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateProcessContract    
            { Candidate = new UpdateCandidateContract { PreferredOfficeId = 1 } };                           
            _mockMapper.Setup(mm => mm.Map<Process>(It.IsAny<CreateProcessContract>())).Returns(new Process());

            _mockCreateProcessValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<CreateProcessContract>>())).Returns(new ValidationResult());

            var exception = Assert.Throws<OfficeNotFoundException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal("The Office 1 was not found.", exception.Message);
            _mockMapper.Verify(x => x.Map<Process>(It.IsAny<CreateProcessContract>()), Times.Once);            
        }


        [Fact(DisplayName = "Verify that delete ProcessService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteProcessService()
        {
            var processs = new List<Process>() { 
                new Process() {
                    Id = 1, 
                    Candidate= new Candidate() 
                } 
            }.AsQueryable();

            _mockRepositoryProcess.Setup(x => x.QueryEager()).Returns(processs);

            _service.Delete(1);
            
            _mockRepositoryProcess.Verify(x => x.QueryEager(), Times.Once);
            _mockRepositoryProcess.Verify(x => x.Delete(It.IsAny<Process>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var processs = new List<Process>() { new Process() { Id = 1 } }.AsQueryable();
            var readedProcessList = new List<ReadedProcessContract> { new ReadedProcessContract { Id = 1 } };
            _mockRepositoryProcess.Setup(mrt => mrt.QueryEager()).Returns(processs);
            _mockMapper.Setup(mm => mm.Map<List<ReadedProcessContract>>(It.IsAny<List<Process>>())).Returns(readedProcessList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryProcess.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedProcessContract>>(It.IsAny<List<Process>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var processs = new List<Process>() { new Process() { Id = 1 } }.AsQueryable();
            var readedProcess = new ReadedProcessContract { Id = 1 };
            _mockRepositoryProcess.Setup(mrt => mrt.QueryEager()).Returns(processs);
            _mockMapper.Setup(mm => mm.Map<ReadedProcessContract>(It.IsAny<Process>())).Returns(readedProcess);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.Id);
            _mockRepositoryProcess.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedProcessContract>(It.IsAny<Process>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetProcessesByCommunity returns enumerable")]
        public void GivenGetProcessesByCommunity_WhenRegularCall_ReturnsValue()
        {
            var process = new Process { Candidate = new Candidate { Community = new Community { Name = "community1" } } };
            var processs = new List<Process>() { process }.AsQueryable();
            var readedProcessList = new List<ReadedProcessContract> { new ReadedProcessContract { Id = 1 } };
            _mockRepositoryProcess.Setup(mrt => mrt.QueryEager()).Returns(processs);
            _mockMapper.Setup(mm => mm.Map<List<ReadedProcessContract>>(It.IsAny<List<Process>>())).Returns(readedProcessList);

            var actualResult = _service.GetProcessesByCommunity("community1");

            Assert.NotNull(actualResult);
            Assert.Single(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryProcess.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedProcessContract>>(It.IsAny<List<Process>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetActiveByCandidateId returns enumerable")]
        public void GivenGetActiveByCandidateId_WhenRegularCall_ReturnsValue()
        {            
            var processs = new List<Process>().AsQueryable();
            var readedProcessList = new List<ReadedProcessContract> { new ReadedProcessContract { Id = 1 } };
            _mockRepositoryProcess.Setup(mrt => mrt.QueryEager()).Returns(processs);
            _mockMapper.Setup(mm => mm.Map<IEnumerable<ReadedProcessContract>>(It.IsAny<IQueryable<Process>>())).Returns(readedProcessList);

            var actualResult = _service.GetActiveByCandidateId(1);

            Assert.NotNull(actualResult);            
            Assert.Single(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryProcess.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<IEnumerable<ReadedProcessContract>>(It.IsAny<IQueryable<Process>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that Approve runs correctly")]
        public void GivenApprove_WhenRegularCall_RunsCorrectly()
        {
            var processs = new List<Process> {
                new Process { 
                    Candidate = new Candidate(),
                    Status = ProcessStatus.NA,
                    Id = 1
                },
            }.AsQueryable();
            _mockRepositoryProcess.Setup(x => x.QueryEager()).Returns(processs);

            _service.Approve(1);

            _mockRepositoryProcess.Verify(_ => _.QueryEager(), Times.Once);
            _mockRepositoryProcess.Verify(_ => _.Approve(It.IsAny<int>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that Reject runs correctly")]
        public void GivenReject_WhenRegularCall_ReturnsValue()
        {
            var processs = new List<Process> {
                new Process {
                    Candidate = new Candidate(),
                    Status = ProcessStatus.NA,
                    Id = 1
                },
            }.AsQueryable();
            _mockRepositoryProcess.Setup(x => x.QueryEager()).Returns(processs);

            _service.Reject(1,"reason");

            _mockRepositoryProcess.Verify(_ => _.QueryEager(), Times.Once);
            _mockRepositoryProcess.Verify(_ => _.Reject(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }
    }
}