// <copyright file="CandidateServiceTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    using Xunit;

    public class CandidateServiceTest : BaseDomainTest
    {
        private readonly CandidateService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Process>> mockRepoProcess;
        private readonly Mock<IRepository<Candidate>> mockRepoCandidate;
        private readonly Mock<IRepository<User>> mockRepoUser;
        private readonly Mock<IRepository<Office>> mockRepoOffice;
        private readonly Mock<IRepository<Community>> mockRepoCommunity;
        private readonly Mock<IRepository<CandidateProfile>> mockRepoCandidateP;
        private readonly Mock<IRepository<OpenPosition>> mockRepoOpenPosition;
        private readonly Mock<ILog<CandidateService>> mockLogCandidateService;
        private readonly Mock<UpdateCandidateContractValidator> mockUpdateCandidateContractValidator;
        private readonly Mock<CreateCandidateContractValidator> mockCreateCandidateContractValidator;

        public CandidateServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepoProcess = new Mock<IRepository<Process>>();
            this.mockRepoCandidate = new Mock<IRepository<Candidate>>();
            this.mockRepoUser = new Mock<IRepository<User>>();
            this.mockRepoOffice = new Mock<IRepository<Office>>();
            this.mockRepoCommunity = new Mock<IRepository<Community>>();
            this.mockRepoCandidateP = new Mock<IRepository<CandidateProfile>>();
            this.mockRepoOpenPosition = new Mock<IRepository<OpenPosition>>();
            this.mockLogCandidateService = new Mock<ILog<CandidateService>>();
            this.mockUpdateCandidateContractValidator = new Mock<UpdateCandidateContractValidator>();
            this.mockCreateCandidateContractValidator = new Mock<CreateCandidateContractValidator>();
            this.service = new CandidateService(
                this.mockMapper.Object,
                this.mockRepoCandidate.Object,
                this.mockRepoCommunity.Object,
                this.mockRepoCandidateP.Object,
                this.mockRepoUser.Object,
                this.mockRepoOffice.Object,
                this.mockRepoProcess.Object,
                this.mockRepoOpenPosition.Object,
                this.MockUnitOfWork.Object,
                this.mockLogCandidateService.Object,
                this.mockUpdateCandidateContractValidator.Object,
                this.mockCreateCandidateContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create CandidateService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateCandidateService()
        {
            var contract = new CreateCandidateContract
            {
                User = new ReadedUserContract { Id = 1 },
                Community = new ReadedCommunityContract { Id = 1 },
                Profile = new ReadedCandidateProfileContract { Id = 1 },
            };
            var expectedCandidate = new CreatedCandidateContract();
            var userList = new List<User> { new User { Id = 1 } }.AsQueryable();
            var communityList = new List<Community> { new Community { Id = 1 } }.AsQueryable();
            var profileList = new List<CandidateProfile> { new CandidateProfile { Id = 1 } }.AsQueryable();
            this.mockCreateCandidateContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Candidate>(It.IsAny<CreateCandidateContract>())).Returns(new Candidate());
            this.mockRepoCandidate.Setup(repoCom => repoCom.Create(It.IsAny<Candidate>())).Returns(new Candidate());
            this.mockMapper.Setup(mm => mm.Map<CreatedCandidateContract>(It.IsAny<Candidate>())).Returns(expectedCandidate);
            this.mockRepoUser.Setup(x => x.Query()).Returns(userList);
            this.mockRepoCommunity.Setup(x => x.Query()).Returns(communityList);
            this.mockRepoCandidateP.Setup(x => x.Query()).Returns(profileList);

            var createdCandidate = this.service.Create(contract);

            Assert.NotNull(createdCandidate);
            Assert.Equal(expectedCandidate, createdCandidate);
            this.mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateCandidateContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Candidate>(It.IsAny<CreateCandidateContract>()), Times.Once);
            this.mockRepoCandidate.Verify(mrt => mrt.Create(It.IsAny<Candidate>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedCandidateContract>(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateCandidateContract();
            var expectedCandidate = new CreatedCandidateContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateCandidateContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedCandidateContract>(It.IsAny<Candidate>())).Returns(expectedCandidate);

            var exception = Assert.Throws<Model.Exceptions.Candidate.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateCandidateContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Candidate>(It.IsAny<CreateCandidateContract>()), Times.Never);
            this.mockRepoCandidate.Verify(mrt => mrt.Create(It.IsAny<Candidate>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedCandidateContract>(It.IsAny<Candidate>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that soft delete only updates CandidateService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteCandidateService()
        {
            var candidates = new List<Candidate>() { new Candidate() { Id = 1 } }.AsQueryable();
            this.mockRepoCandidate.Setup(mrt => mrt.Query()).Returns(candidates);
            this.service.Delete(1);

            this.mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepoCandidate.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepoCandidate.Verify(mrt => mrt.Update(It.IsAny<Candidate>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteCandidateNotFoundException()
        {
            var expectedErrorMEssage = $"Candidate not found for the CandidateId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Candidate.DeleteCandidateNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepoCandidate.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepoCandidate.Verify(mrt => mrt.Delete(It.IsAny<Candidate>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update CandidateService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateCandidateContract
            {
                User = new ReadedUserContract { Id = 1 },
                Community = new ReadedCommunityContract { Id = 1 },
                Profile = new ReadedCandidateProfileContract { Id = 1 },
                PreferredOfficeId = 1,
            };

            var userList = new List<User> { new User { Id = 1 } }.AsQueryable();
            var communityList = new List<Community> { new Community { Id = 1 } }.AsQueryable();
            var profileList = new List<CandidateProfile> { new CandidateProfile { Id = 1 } }.AsQueryable();
            var officeList = new List<Office> { new Office { Id = 1 } }.AsQueryable();
            this.mockUpdateCandidateContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Candidate>(It.IsAny<UpdateCandidateContract>())).Returns(new Candidate());
            this.mockRepoUser.Setup(x => x.Query()).Returns(userList);
            this.mockRepoCommunity.Setup(x => x.Query()).Returns(communityList);
            this.mockRepoCandidateP.Setup(x => x.Query()).Returns(profileList);
            this.mockRepoOffice.SetupSequence(x => x.Query()).Returns(officeList);

            this.service.Update(contract);

            this.mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateCandidateContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Candidate>(It.IsAny<UpdateCandidateContract>()), Times.Once);
            this.mockRepoCandidate.Verify(mrt => mrt.Update(It.IsAny<Candidate>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateCandidateContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateCandidateContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<Candidate>(It.IsAny<UpdateCandidateContract>())).Returns(new Candidate());

            var exception = Assert.Throws<Model.Exceptions.Candidate.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogCandidateService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateCandidateContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Candidate>(It.IsAny<UpdateCandidateContract>()), Times.Never);
            this.mockRepoCandidate.Verify(mrt => mrt.Update(It.IsAny<Candidate>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<Candidate>() { new Candidate() { Id = 1 } }.AsQueryable();
            var readedCandidateList = new List<ReadedCandidateContract> { new ReadedCandidateContract { Id = 1 } };
            this.mockRepoCandidate.Setup(mrt => mrt.QueryEager()).Returns(communities);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedCandidateContract>>(It.IsAny<List<Candidate>>())).Returns(readedCandidateList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedCandidateContract>>(It.IsAny<List<Candidate>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<Candidate>() { new Candidate() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCandidate = new ReadedCandidateContract { Id = 1, Name = "Name" };
            this.mockRepoCandidate.Setup(mrt => mrt.QueryEager()).Returns(communities);
            this.mockMapper.Setup(mm => mm.Map<ReadedCandidateContract>(It.IsAny<Candidate>())).Returns(readedCandidate);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            this.mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedCandidateContract>(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read with filter rule returns a value")]
        public void GivenReadWithFilterRule_WhenRegularCall_ReturnsValue()
        {
            var candidateList = new List<Candidate>() { new Candidate() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCandidateList = new List<ReadedCandidateContract> { new ReadedCandidateContract { Id = 1, Name = "Name" } };
            this.mockRepoCandidate.Setup(mrt => mrt.QueryEager()).Returns(candidateList);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedCandidateContract>>(It.IsAny<List<Candidate>>())).Returns(readedCandidateList);
            Func<Candidate, bool> filter = candidate => true;

            var actualResult = this.service.Read(filter);

            Assert.NotNull(actualResult);
            this.mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedCandidateContract>>(It.IsAny<List<Candidate>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that Exists returns a value")]
        public void GivenExists_WhenRegularCall_ReturnsValue()
        {
            var candidateList = new List<Candidate>() { new Candidate() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCandidate = new ReadedCandidateContract { Id = 1, Name = "Name" };
            this.mockRepoCandidate.Setup(mrt => mrt.QueryEager()).Returns(candidateList);
            this.mockMapper.Setup(mm => mm.Map<ReadedCandidateContract>(It.IsAny<Candidate>())).Returns(readedCandidate);

            var actualResult = this.service.Exists(1);

            Assert.NotNull(actualResult);
            this.mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedCandidateContract>(It.IsAny<Candidate>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that ListApp returns a value")]
        public void GivenListApp_WhenRegularCall_ReturnsValue()
        {
            var candidateList = new List<Candidate>() { new Candidate() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCandidateList = new List<ReadedCandidateAppContract> { new ReadedCandidateAppContract() };
            this.mockRepoCandidate.Setup(mrt => mrt.QueryEager()).Returns(candidateList);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedCandidateAppContract>>(It.IsAny<List<Candidate>>())).Returns(readedCandidateList);

            var actualResult = this.service.ListApp();

            Assert.NotNull(actualResult);
            this.mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedCandidateAppContract>>(It.IsAny<List<Candidate>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetCandidate returns a value")]
        public void GivenGetCandidate_WhenRegularCall_ReturnsValue()
        {
            var candidateList = new List<Candidate>() { new Candidate() { Id = 1 } }.AsQueryable();
            this.mockRepoCandidate.Setup(x => x.QueryEager()).Returns(candidateList);

            var actualResult = this.service.GetCandidate(1);

            Assert.NotNull(actualResult);
            this.mockRepoCandidate.Verify(_ => _.QueryEager(), Times.Once);
        }
    }
}