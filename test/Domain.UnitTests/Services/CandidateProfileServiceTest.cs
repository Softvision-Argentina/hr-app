// <copyright file="CandidateProfileServiceTest.cs" company="Softvision">
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
    using Domain.Model.Exceptions.CandidateProfile;
    using Domain.Services.Contracts.CandidateProfile;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.CandidateProfile;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class CandidateProfileServiceTest : BaseDomainTest
    {
        private readonly CandidateProfileService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<CandidateProfile>> mockRepositoryCandidateProfile;
        private readonly Mock<IRepository<Community>> mockRepositoryModelCommunity;
        private readonly Mock<ILog<CandidateProfileService>> mockLogCandidateProfileService;
        private readonly Mock<UpdateCandidateProfileContractValidator> mockUpdateCandidateProfileContractValidator;
        private readonly Mock<CreateCandidateProfileContractValidator> mockCreateCandidateProfileContractValidator;

        public CandidateProfileServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryCandidateProfile = new Mock<IRepository<CandidateProfile>>();
            this.mockRepositoryModelCommunity = new Mock<IRepository<Community>>();
            this.mockLogCandidateProfileService = new Mock<ILog<CandidateProfileService>>();
            this.mockUpdateCandidateProfileContractValidator = new Mock<UpdateCandidateProfileContractValidator>();
            this.mockCreateCandidateProfileContractValidator = new Mock<CreateCandidateProfileContractValidator>();
            this.service = new CandidateProfileService(
                this.mockMapper.Object,
                this.mockRepositoryCandidateProfile.Object,
                this.MockUnitOfWork.Object,
                this.mockLogCandidateProfileService.Object,
                this.mockUpdateCandidateProfileContractValidator.Object,
                this.mockCreateCandidateProfileContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create CandidateProfileService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateCandidateProfileService()
        {
            var contract = new CreateCandidateProfileContract();
            var expectedCandidateProfile = new CreatedCandidateProfileContract();
            this.mockCreateCandidateProfileContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>())).Returns(expectedCandidateProfile);

            var createdCandidateProfile = this.service.Create(contract);

            Assert.NotNull(createdCandidateProfile);
            Assert.Equal(expectedCandidateProfile, createdCandidateProfile);
            this.mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateCandidateProfileContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<CreateCandidateProfileContract>()), Times.Once);
            this.mockRepositoryCandidateProfile.Verify(mrt => mrt.Create(It.IsAny<CandidateProfile>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateCandidateProfileContract();
            var expectedCandidateProfile = new CreatedCandidateProfileContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateCandidateProfileContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>())).Returns(expectedCandidateProfile);

            var exception = Assert.Throws<Model.Exceptions.CandidateProfile.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateCandidateProfileContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<CreateCandidateProfileContract>()), Times.Never);
            this.mockRepositoryCandidateProfile.Verify(mrt => mrt.Create(It.IsAny<CandidateProfile>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that given a model that already exists on the database, throws an exception")]
        public void GivenCreate_WhenModelExists_ThrowsInvalidCandidateProfileException()
        {
            var contract = new CreateCandidateProfileContract { Name = "Name" };
            var candidateProfiles = new List<CandidateProfile>() { new CandidateProfile { Name = "Name", Id = 1 } }.AsQueryable();
            this.mockCreateCandidateProfileContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>())).Returns(new ValidationResult());
            this.mockRepositoryCandidateProfile.Setup(mockRep => mockRep.Query()).Returns(candidateProfiles);

            Exception ex = Assert.Throws<InvalidCandidateProfileException>(() => this.service.Create(contract));

            this.mockCreateCandidateProfileContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>()), Times.Once);
            this.mockRepositoryCandidateProfile.Verify(mrt => mrt.Query(), Times.Once);
            Assert.NotNull(ex);
            Assert.IsType<InvalidCandidateProfileException>(ex);
            Assert.Equal($"The Profile already exists .", ex.Message);
        }

        [Fact(DisplayName = "Verify that delete CandidateProfileService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteCandidateProfileService()
        {
            var candidateProfiles = new List<CandidateProfile>() { new CandidateProfile() { Id = 1 } }.AsQueryable();
            this.mockRepositoryCandidateProfile.Setup(mrt => mrt.Query()).Returns(candidateProfiles);

            this.service.Delete(1);

            this.mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryCandidateProfile.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryCandidateProfile.Verify(mrt => mrt.Delete(It.IsAny<CandidateProfile>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteCandidateProfileNotFoundException()
        {
            var expectedErrorMEssage = $"Profile not found for the Profile Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.CandidateProfile.DeleteCandidateProfileNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryCandidateProfile.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryCandidateProfile.Verify(mrt => mrt.Delete(It.IsAny<CandidateProfile>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update CandidateProfileService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateCandidateProfileContract();
            this.mockUpdateCandidateProfileContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>())).Returns(new CandidateProfile());

            this.service.Update(contract);

            this.mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateCandidateProfileContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>()), Times.Once);
            this.mockRepositoryCandidateProfile.Verify(mrt => mrt.Update(It.IsAny<CandidateProfile>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateCandidateProfileContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateCandidateProfileContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>())).Returns(new CandidateProfile());

            var exception = Assert.Throws<Model.Exceptions.CandidateProfile.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateCandidateProfileContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>()), Times.Never);
            this.mockRepositoryCandidateProfile.Verify(mrt => mrt.Update(It.IsAny<CandidateProfile>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var candidateProfiles = new List<CandidateProfile>() { new CandidateProfile() { Id = 1 } }.AsQueryable();
            var readedCandidatePCList = new List<ReadedCandidateProfileContract> { new ReadedCandidateProfileContract { Id = 1 } };
            this.mockRepositoryCandidateProfile.Setup(mrt => mrt.QueryEager()).Returns(candidateProfiles);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedCandidateProfileContract>>(It.IsAny<List<CandidateProfile>>())).Returns(readedCandidatePCList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryCandidateProfile.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedCandidateProfileContract>>(It.IsAny<List<CandidateProfile>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var candidateProfiles = new List<CandidateProfile>() { new CandidateProfile() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCandidatePC = new ReadedCandidateProfileContract { Id = 1, Name = "Name" };
            this.mockRepositoryCandidateProfile.Setup(mrt => mrt.QueryEager()).Returns(candidateProfiles);
            this.mockMapper.Setup(mm => mm.Map<ReadedCandidateProfileContract>(It.IsAny<CandidateProfile>())).Returns(readedCandidatePC);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            this.mockRepositoryCandidateProfile.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedCandidateProfileContract>(It.IsAny<CandidateProfile>()), Times.Once);
        }
    }
}