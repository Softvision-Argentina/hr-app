// <copyright file="HireProjectionServiceTest.cs" company="Softvision">
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
    using Domain.Services.Contracts.HireProjection;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.HireProjection;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class HireProjectionServiceTest : BaseDomainTest
    {
        private readonly HireProjectionService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<HireProjection>> mockRepositoryHireProjection;
        private readonly Mock<ILog<HireProjectionService>> mockLogHireProjectionService;
        private readonly Mock<UpdateHireProjectionContractValidator> mockUpdateHireProjectionContractValidator;
        private readonly Mock<CreateHireProjectionContractValidator> mockCreateHireProjectionContractValidator;

        public HireProjectionServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryHireProjection = new Mock<IRepository<HireProjection>>();
            this.mockLogHireProjectionService = new Mock<ILog<HireProjectionService>>();
            this.mockUpdateHireProjectionContractValidator = new Mock<UpdateHireProjectionContractValidator>();
            this.mockCreateHireProjectionContractValidator = new Mock<CreateHireProjectionContractValidator>();
            this.service = new HireProjectionService(
                this.mockMapper.Object,
                this.mockRepositoryHireProjection.Object,
                this.MockUnitOfWork.Object,
                this.mockLogHireProjectionService.Object,
                this.mockUpdateHireProjectionContractValidator.Object,
                this.mockCreateHireProjectionContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create HireProjectionService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateHireProjectionService()
        {
            var contract = new CreateHireProjectionContract();
            var expectedHireProjection = new CreatedHireProjectionContract();
            this.mockCreateHireProjectionContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<HireProjection>(It.IsAny<CreateHireProjectionContract>())).Returns(new HireProjection());
            this.mockRepositoryHireProjection.Setup(repoCom => repoCom.Create(It.IsAny<HireProjection>())).Returns(new HireProjection());
            this.mockMapper.Setup(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>())).Returns(expectedHireProjection);

            var createdHireProjection = this.service.Create(contract);

            Assert.NotNull(createdHireProjection);
            Assert.Equal(expectedHireProjection, createdHireProjection);
            this.mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateHireProjectionContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<CreateHireProjectionContract>()), Times.Once);
            this.mockRepositoryHireProjection.Verify(mrt => mrt.Create(It.IsAny<HireProjection>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateHireProjectionContract();
            var expectedHireProjection = new CreatedHireProjectionContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateHireProjectionContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>())).Returns(expectedHireProjection);

            var exception = Assert.Throws<Model.Exceptions.HireProjection.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateHireProjectionContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<CreateHireProjectionContract>()), Times.Never);
            this.mockRepositoryHireProjection.Verify(mrt => mrt.Create(It.IsAny<HireProjection>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete HireProjectionService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteHireProjectionService()
        {
            var hireProjections = new List<HireProjection>() { new HireProjection() { Id = 1 } }.AsQueryable();
            this.mockRepositoryHireProjection.Setup(mrt => mrt.Query()).Returns(hireProjections);

            this.service.Delete(1);

            this.mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryHireProjection.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryHireProjection.Verify(mrt => mrt.Delete(It.IsAny<HireProjection>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteHireProjectionNotFoundException()
        {
            var expectedErrorMEssage = $"Hire projection not found for the hireProjectionId: {0}";

            var exception = Assert.Throws<Model.Exceptions.HireProjection.DeleteHireProjectionNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryHireProjection.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryHireProjection.Verify(mrt => mrt.Delete(It.IsAny<HireProjection>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update HireProjectionService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateHireProjectionContract();
            this.mockUpdateHireProjectionContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>())).Returns(new HireProjection());

            this.service.Update(contract);

            this.mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateHireProjectionContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>()), Times.Once);
            this.mockRepositoryHireProjection.Verify(mrt => mrt.Update(It.IsAny<HireProjection>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateHireProjectionContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateHireProjectionContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>())).Returns(new HireProjection());

            var exception = Assert.Throws<Model.Exceptions.HireProjection.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateHireProjectionContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>()), Times.Never);
            this.mockRepositoryHireProjection.Verify(mrt => mrt.Update(It.IsAny<HireProjection>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var hireProjections = new List<HireProjection>() { new HireProjection() { Id = 1 } }.AsQueryable();
            var readedHireProjectionList = new List<ReadedHireProjectionContract> { new ReadedHireProjectionContract { Id = 1 } };
            this.mockRepositoryHireProjection.Setup(mrt => mrt.QueryEager()).Returns(hireProjections);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedHireProjectionContract>>(It.IsAny<List<HireProjection>>())).Returns(readedHireProjectionList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryHireProjection.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedHireProjectionContract>>(It.IsAny<List<HireProjection>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var hireProjections = new List<HireProjection>() { new HireProjection() { Id = 1 } }.AsQueryable();
            var readedHireProjection = new ReadedHireProjectionContract { Id = 1 };
            this.mockRepositoryHireProjection.Setup(mrt => mrt.QueryEager()).Returns(hireProjections);
            this.mockMapper.Setup(mm => mm.Map<ReadedHireProjectionContract>(It.IsAny<HireProjection>())).Returns(readedHireProjection);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedHireProjection, actualResult);
            this.mockRepositoryHireProjection.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedHireProjectionContract>(It.IsAny<HireProjection>()), Times.Once);
        }
    }
}