// <copyright file="CommunityServiceTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Contracts.Community;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.Community;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class CommunityServiceTest : BaseDomainTest
    {
        private readonly CommunityService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Community>> mockRepositoryCommunity;
        private readonly Mock<IRepository<CandidateProfile>> mockRepositoryCandidateProfile;
        private readonly Mock<ILog<CommunityService>> mockLogCommunityService;
        private readonly Mock<UpdateCommunityContractValidator> mockUpdateCommunityContractValidator;
        private readonly Mock<CreateCommunityContractValidator> mockCreateCommunityContractValidator;

        public CommunityServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryCommunity = new Mock<IRepository<Community>>();
            this.mockRepositoryCandidateProfile = new Mock<IRepository<CandidateProfile>>();
            this.mockLogCommunityService = new Mock<ILog<CommunityService>>();
            this.mockUpdateCommunityContractValidator = new Mock<UpdateCommunityContractValidator>();
            this.mockCreateCommunityContractValidator = new Mock<CreateCommunityContractValidator>();
            this.service = new CommunityService(
                this.mockMapper.Object,
                this.mockRepositoryCommunity.Object,
                this.mockRepositoryCandidateProfile.Object,
                this.MockUnitOfWork.Object,
                this.mockLogCommunityService.Object,
                this.mockUpdateCommunityContractValidator.Object,
                this.mockCreateCommunityContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create CommunityService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateCommunityService()
        {
            var contract = new CreateCommunityContract();
            var expectedCommunity = new CreatedCommunityContract();
            this.mockCreateCommunityContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Community>(It.IsAny<CreateCommunityContract>())).Returns(new Community());
            this.mockRepositoryCommunity.Setup(repoCom => repoCom.Create(It.IsAny<Community>())).Returns(new Community());
            this.mockMapper.Setup(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>())).Returns(expectedCommunity);

            var createdCommunity = this.service.Create(contract);

            Assert.NotNull(createdCommunity);
            Assert.Equal(expectedCommunity, createdCommunity);
            this.mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateCommunityContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<CreateCommunityContract>()), Times.Once);
            this.mockRepositoryCommunity.Verify(mrt => mrt.Create(It.IsAny<Community>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateCommunityContract();
            var expectedCommunity = new CreatedCommunityContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateCommunityContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>())).Returns(expectedCommunity);

            var exception = Assert.Throws<Model.Exceptions.Community.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateCommunityContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<CreateCommunityContract>()), Times.Never);
            this.mockRepositoryCommunity.Verify(mrt => mrt.Create(It.IsAny<Community>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete CommunityService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteCommunityService()
        {
            var communities = new List<Community>() { new Community() { Id = 1 } }.AsQueryable();
            this.mockRepositoryCommunity.Setup(mrt => mrt.Query()).Returns(communities);

            this.service.Delete(1);

            this.mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryCommunity.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryCommunity.Verify(mrt => mrt.Delete(It.IsAny<Community>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteCommunityNotFoundException()
        {
            var expectedErrorMEssage = $"Community not found for the CommunityId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Community.DeleteCommunityNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryCommunity.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryCommunity.Verify(mrt => mrt.Delete(It.IsAny<Community>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update CommunityService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateCommunityContract();
            this.mockUpdateCommunityContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>())).Returns(new Community());

            this.service.Update(contract);

            this.mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateCommunityContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>()), Times.Once);
            this.mockRepositoryCommunity.Verify(mrt => mrt.Update(It.IsAny<Community>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateCommunityContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateCommunityContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>())).Returns(new Community());

            var exception = Assert.Throws<Model.Exceptions.Community.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateCommunityContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>()), Times.Never);
            this.mockRepositoryCommunity.Verify(mrt => mrt.Update(It.IsAny<Community>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<Community>() { new Community() { Id = 1 } }.AsQueryable();
            var readedCommunityList = new List<ReadedCommunityContract> { new ReadedCommunityContract { Id = 1 } };
            this.mockRepositoryCommunity.Setup(mrt => mrt.Query()).Returns(communities);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedCommunityContract>>(It.IsAny<List<Community>>())).Returns(readedCommunityList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryCommunity.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedCommunityContract>>(It.IsAny<List<Community>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<Community>() { new Community() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCommunity = new ReadedCommunityContract { Id = 1, Name = "Name" };
            this.mockRepositoryCommunity.Setup(mrt => mrt.Query()).Returns(communities);
            this.mockMapper.Setup(mm => mm.Map<ReadedCommunityContract>(It.IsAny<Community>())).Returns(readedCommunity);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            this.mockRepositoryCommunity.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedCommunityContract>(It.IsAny<Community>()), Times.Once);
        }
    }
}