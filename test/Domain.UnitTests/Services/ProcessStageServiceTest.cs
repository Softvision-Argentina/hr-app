// <copyright file="ProcessStageServiceTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Domain.Model;
    using Domain.Services.Contracts.Process;
    using Domain.Services.Contracts.Stage;
    using Domain.Services.Contracts.Stage.StageItem;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.Stage;
    using Domain.Services.Interfaces.Repositories;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class ProcessStageServiceTest : BaseDomainTest
    {
        private readonly ProcessStageService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IProcessStageRepository> mockRepositoryProcessStage;
        private readonly Mock<IStageItemRepository> mockRepositoryStageItem;
        private readonly Mock<IProcessRepository> mockRepositoryProcess;
        private readonly Mock<ILog<ProcessStageService>> mockLogProcessStageService;
        private readonly Mock<UpdateStageContractValidator> mockUpdateProcessStageContractValidator;
        private readonly Mock<CreateStageContractValidator> mockCreateStageContractValidator;
        private readonly Mock<ProcessStatusContractValidator> mockProcessStatusContractValidator;

        public ProcessStageServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryProcessStage = new Mock<IProcessStageRepository>();
            this.mockRepositoryStageItem = new Mock<IStageItemRepository>();
            this.mockRepositoryProcess = new Mock<IProcessRepository>();
            this.mockLogProcessStageService = new Mock<ILog<ProcessStageService>>();
            this.mockUpdateProcessStageContractValidator = new Mock<UpdateStageContractValidator>();
            this.mockCreateStageContractValidator = new Mock<CreateStageContractValidator>();
            this.mockProcessStatusContractValidator = new Mock<ProcessStatusContractValidator>();
            this.service = new ProcessStageService(
                this.mockMapper.Object,
                this.mockRepositoryProcessStage.Object,
                this.mockRepositoryStageItem.Object,
                this.mockRepositoryProcess.Object,
                this.MockUnitOfWork.Object,
                this.mockLogProcessStageService.Object,
                this.mockUpdateProcessStageContractValidator.Object,
                this.mockCreateStageContractValidator.Object,
                this.mockProcessStatusContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create ProcessStageService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateProcessStageService()
        {
            var contract = new CreateStageContract();
            var expectedProcessStage = new CreatedStageContract();
            var processes = new List<Process> { new Process { Id = 0 } }.AsQueryable();
            this.mockCreateStageContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Stage>(It.IsAny<CreateStageContract>())).Returns(new Stage());
            this.mockRepositoryProcessStage.Setup(repoCom => repoCom.Create(It.IsAny<Stage>())).Returns(new Stage());
            this.mockRepositoryProcess.Setup(x => x.Query()).Returns(processes);
            this.mockMapper.Setup(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>())).Returns(expectedProcessStage);
            this.mockProcessStatusContractValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ReadedProcessContract>>())).Returns(new ValidationResult());

            var createdProcessStage = this.service.Create(contract);

            Assert.NotNull(createdProcessStage);
            Assert.Equal(expectedProcessStage, createdProcessStage);
            this.mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateStageContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<CreateStageContract>()), Times.Once);
            this.mockRepositoryProcessStage.Verify(mrt => mrt.Create(It.IsAny<Stage>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateStageContract();
            var expectedProcessStage = new CreatedStageContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateStageContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>())).Returns(expectedProcessStage);

            var exception = Assert.Throws<Model.Exceptions.Stage.CreateStageInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateStageContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<CreateStageContract>()), Times.Never);
            this.mockRepositoryProcessStage.Verify(mrt => mrt.Create(It.IsAny<Stage>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete ProcessStageService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteProcessStageService()
        {
            var processStages = new List<Stage>() { new Stage() { Id = 1 } }.AsQueryable();
            this.mockRepositoryProcessStage.Setup(mrt => mrt.QueryEager()).Returns(processStages);

            this.service.Delete(1);

            this.mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryProcessStage.Verify(mrt => mrt.QueryEager(), Times.Once);
            this.mockRepositoryProcessStage.Verify(mrt => mrt.Delete(It.IsAny<Stage>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update ProcessStageService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateStageContract();
            var processes = new List<Process> { new Process { Id = 0 } }.AsQueryable();
            this.mockUpdateProcessStageContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>())).Returns(new Stage());
            this.mockRepositoryProcess.Setup(x => x.Query()).Returns(processes);
            this.mockProcessStatusContractValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ReadedProcessContract>>())).Returns(new ValidationResult());

            this.service.Update(contract);

            this.mockUpdateProcessStageContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateStageContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateProcessStageContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>())).Returns(new Stage());

            var exception = Assert.Throws<Model.Exceptions.Stage.UpdateStageInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockUpdateProcessStageContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>()), Times.Never);
            this.mockRepositoryProcessStage.Verify(mrt => mrt.Update(It.IsAny<Stage>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var processStages = new List<Stage>() { new Stage() { Id = 1 } }.AsQueryable();
            var readedProcessStageList = new List<ReadedStageContract> { new ReadedStageContract { Id = 1 } };
            this.mockRepositoryProcessStage.Setup(mrt => mrt.QueryEager()).Returns(processStages);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedStageContract>>(It.IsAny<List<Stage>>())).Returns(readedProcessStageList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryProcessStage.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedStageContract>>(It.IsAny<List<Stage>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var processStages = new List<Stage>() { new Stage() { Id = 1 } }.AsQueryable();
            var readedProcessStage = new ReadedStageContract { Id = 1 };
            this.mockRepositoryProcessStage.Setup(mrt => mrt.QueryEager()).Returns(processStages);
            this.mockMapper.Setup(mm => mm.Map<ReadedStageContract>(It.IsAny<Stage>())).Returns(readedProcessStage);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedProcessStage, actualResult);
            this.mockRepositoryProcessStage.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedStageContract>(It.IsAny<Stage>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that AddItemToStage returns a value")]
        public void GivenAddItemToStage_WhenRegularCall_ReturnsValue()
        {
            var processStages = new List<Stage>().AsQueryable();
            var createdStageItemContract = new CreatedStageItemContract { Id = 1 };
            this.mockMapper.Setup(mm => mm.Map<StageItem>(It.IsAny<CreateStageItemContract>())).Returns(new StageItem());
            this.mockMapper.Setup(mm => mm.Map<CreatedStageItemContract>(It.IsAny<StageItem>())).Returns(createdStageItemContract);

            var actualResult = this.service.AddItemToStage(new CreateStageItemContract());

            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Id, createdStageItemContract.Id);
            this.mockMapper.Verify(_ => _.Map<StageItem>(It.IsAny<CreateStageItemContract>()), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedStageItemContract>(It.IsAny<StageItem>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that RemoveItemToStage runs")]
        public void GivenRemoveItemToStage_WhenRegularCall_RemoveItem()
        {
            this.mockMapper.Setup(mm => mm.Map<StageItem>(It.IsAny<CreateStageItemContract>())).Returns(new StageItem());
            this.mockMapper.Setup(mm => mm.Map<CreatedStageItemContract>(It.IsAny<StageItem>())).Returns(new CreatedStageItemContract());

            this.service.RemoveItemToStage(1);

            this.mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryStageItem.Verify(x => x.Delete(It.IsAny<StageItem>()), Times.Exactly(1));
            this.MockUnitOfWork.Verify(x => x.Complete(), Times.Exactly(1));
        }

        [Fact(DisplayName = "Verify that UpdateStageItem runs")]
        public void GivenUpdateStageItem_WhenRegularCall_RunsCorrectly()
        {
            this.mockMapper.Setup(mm => mm.Map<StageItem>(It.IsAny<CreateStageItemContract>())).Returns(new StageItem());

            this.service.UpdateStageItem(new UpdateStageItemContract());

            this.mockRepositoryStageItem.Verify(x => x.Update(It.IsAny<StageItem>()), Times.Exactly(1));
            this.MockUnitOfWork.Verify(x => x.Complete(), Times.Exactly(1));
        }
    }
}