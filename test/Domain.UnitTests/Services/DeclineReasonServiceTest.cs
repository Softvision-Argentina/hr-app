// <copyright file="DeclineReasonServiceTest.cs" company="Softvision">
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
    using Domain.Model.Exceptions;
    using Domain.Model.Exceptions.Skill;
    using Domain.Services.Contracts;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class DeclineReasonServiceTest : BaseDomainTest
    {
        private readonly DeclineReasonService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<DeclineReason>> mockRepositoryDeclineReason;
        private readonly Mock<ILog<DeclineReasonService>> mockLogDeclineReasonService;
        private readonly Mock<UpdateDeclineReasonContractValidator> mockUpdateDeclineReasonContractValidator;
        private readonly Mock<CreateDeclineReasonContractValidator> mockCreateDeclineReasonContractValidator;

        public DeclineReasonServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryDeclineReason = new Mock<IRepository<DeclineReason>>();
            this.mockLogDeclineReasonService = new Mock<ILog<DeclineReasonService>>();
            this.mockUpdateDeclineReasonContractValidator = new Mock<UpdateDeclineReasonContractValidator>();
            this.mockCreateDeclineReasonContractValidator = new Mock<CreateDeclineReasonContractValidator>();
            this.service = new DeclineReasonService(
                this.mockMapper.Object,
                this.mockRepositoryDeclineReason.Object,
                this.MockUnitOfWork.Object,
                this.mockLogDeclineReasonService.Object,
                this.mockUpdateDeclineReasonContractValidator.Object,
                this.mockCreateDeclineReasonContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create DeclineReasonService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateDeclineReasonService()
        {
            var contract = new CreateDeclineReasonContract();
            var expectedDeclineReason = new CreatedDeclineReasonContract();
            this.mockCreateDeclineReasonContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<DeclineReason>(It.IsAny<CreateDeclineReasonContract>())).Returns(new DeclineReason());
            this.mockRepositoryDeclineReason.Setup(repoCom => repoCom.Create(It.IsAny<DeclineReason>())).Returns(new DeclineReason());
            this.mockMapper.Setup(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>())).Returns(expectedDeclineReason);

            var createdDeclineReason = this.service.Create(contract);

            Assert.NotNull(createdDeclineReason);
            Assert.Equal(expectedDeclineReason, createdDeclineReason);
            this.mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateDeclineReasonContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<CreateDeclineReasonContract>()), Times.Once);
            this.mockRepositoryDeclineReason.Verify(mrt => mrt.Create(It.IsAny<DeclineReason>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateDeclineReasonContract();
            var expectedDeclineReason = new CreatedDeclineReasonContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateDeclineReasonContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>())).Returns(expectedDeclineReason);

            var exception = Assert.Throws<CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateDeclineReasonContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<CreateDeclineReasonContract>()), Times.Never);
            this.mockRepositoryDeclineReason.Verify(mrt => mrt.Create(It.IsAny<DeclineReason>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete DeclineReasonService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteDeclineReasonService()
        {
            var declineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1 } }.AsQueryable();
            this.mockRepositoryDeclineReason.Setup(mrt => mrt.Query()).Returns(declineReasons);

            this.service.Delete(1);

            this.mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryDeclineReason.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryDeclineReason.Verify(mrt => mrt.Delete(It.IsAny<DeclineReason>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteDeclineReasonNotFoundException()
        {
            var expectedErrorMEssage = $"Skill type not found for the skillTypeId: {0}";

            var exception = Assert.Throws<DeleteDeclineReasonNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryDeclineReason.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryDeclineReason.Verify(mrt => mrt.Delete(It.IsAny<DeclineReason>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update DeclineReasonService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateDeclineReasonContract();
            this.mockUpdateDeclineReasonContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>())).Returns(new DeclineReason());

            this.service.Update(contract);

            this.mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateDeclineReasonContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>()), Times.Once);
            this.mockRepositoryDeclineReason.Verify(mrt => mrt.Update(It.IsAny<DeclineReason>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateDeclineReasonContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateDeclineReasonContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>())).Returns(new DeclineReason());

            var exception = Assert.Throws<CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateDeclineReasonContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>()), Times.Never);
            this.mockRepositoryDeclineReason.Verify(mrt => mrt.Update(It.IsAny<DeclineReason>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var declineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1 } }.AsQueryable();
            var readedDeclineReasonList = new List<ReadedDeclineReasonContract> { new ReadedDeclineReasonContract { Id = 1 } };
            this.mockRepositoryDeclineReason.Setup(mrt => mrt.QueryEager()).Returns(declineReasons);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>())).Returns(readedDeclineReasonList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryDeclineReason.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that list named returns a value")]
        public void GivenListNamed_WhenRegularCall_ReturnsValue()
        {
            var declineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedDeclineReasonList = new List<ReadedDeclineReasonContract> { new ReadedDeclineReasonContract { Id = 1 } };
            this.mockRepositoryDeclineReason.Setup(mrt => mrt.QueryEager()).Returns(declineReasons);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>())).Returns(readedDeclineReasonList);

            var actualResult = this.service.ListNamed();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryDeclineReason.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var declineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedDeclineReason = new ReadedDeclineReasonContract { Id = 1, Name = "Name" };
            this.mockRepositoryDeclineReason.Setup(mrt => mrt.QueryEager()).Returns(declineReasons);
            this.mockMapper.Setup(mm => mm.Map<ReadedDeclineReasonContract>(It.IsAny<DeclineReason>())).Returns(readedDeclineReason);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            this.mockRepositoryDeclineReason.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDeclineReasonContract>(It.IsAny<DeclineReason>()), Times.Once);
        }
    }
}