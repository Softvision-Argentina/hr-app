// <copyright file="OfficeServiceTest.cs" company="Softvision">
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
    using Domain.Services.Contracts.Office;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.Office;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class OfficeServiceTest : BaseDomainTest
    {
        private readonly OfficeService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Office>> mockRepositoryOffice;
        private readonly Mock<IRepository<Room>> mockRepositoryModelRoom;
        private readonly Mock<ILog<OfficeService>> mockLogOfficeService;
        private readonly Mock<UpdateOfficeContractValidator> mockUpdateOfficeContractValidator;
        private readonly Mock<CreateOfficeContractValidator> mockCreateOfficeContractValidator;

        public OfficeServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryOffice = new Mock<IRepository<Office>>();
            this.mockRepositoryModelRoom = new Mock<IRepository<Room>>();
            this.mockLogOfficeService = new Mock<ILog<OfficeService>>();
            this.mockUpdateOfficeContractValidator = new Mock<UpdateOfficeContractValidator>();
            this.mockCreateOfficeContractValidator = new Mock<CreateOfficeContractValidator>();
            this.service = new OfficeService(
                this.mockMapper.Object,
                this.mockRepositoryOffice.Object,
                this.mockRepositoryModelRoom.Object,
                this.MockUnitOfWork.Object,
                this.mockLogOfficeService.Object,
                this.mockUpdateOfficeContractValidator.Object,
                this.mockCreateOfficeContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create OfficeService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateOfficeService()
        {
            var contract = new CreateOfficeContract();
            var expectedOffice = new CreatedOfficeContract();
            this.mockCreateOfficeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Office>(It.IsAny<CreateOfficeContract>())).Returns(new Office());
            this.mockRepositoryOffice.Setup(repoCom => repoCom.Create(It.IsAny<Office>())).Returns(new Office());
            this.mockMapper.Setup(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>())).Returns(expectedOffice);

            var createdOffice = this.service.Create(contract);

            Assert.NotNull(createdOffice);
            Assert.Equal(expectedOffice, createdOffice);
            this.mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateOfficeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<CreateOfficeContract>()), Times.Once);
            this.mockRepositoryOffice.Verify(mrt => mrt.Create(It.IsAny<Office>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateOfficeContract();
            var expectedOffice = new CreatedOfficeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateOfficeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>())).Returns(expectedOffice);

            var exception = Assert.Throws<Model.Exceptions.Office.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateOfficeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<CreateOfficeContract>()), Times.Never);
            this.mockRepositoryOffice.Verify(mrt => mrt.Create(It.IsAny<Office>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete OfficeService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteOfficeService()
        {
            var offices = new List<Office>() { new Office() { Id = 1 } }.AsQueryable();
            this.mockRepositoryOffice.Setup(mrt => mrt.Query()).Returns(offices);

            this.service.Delete(1);

            this.mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryOffice.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryOffice.Verify(mrt => mrt.Delete(It.IsAny<Office>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteOfficeNotFoundException()
        {
            var expectedErrorMEssage = $"Office not found for the Office Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.Office.DeleteOfficeNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryOffice.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryOffice.Verify(mrt => mrt.Delete(It.IsAny<Office>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update OfficeService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateOfficeContract();
            this.mockUpdateOfficeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>())).Returns(new Office());

            this.service.Update(contract);

            this.mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateOfficeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>()), Times.Once);
            this.mockRepositoryOffice.Verify(mrt => mrt.Update(It.IsAny<Office>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateOfficeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateOfficeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>())).Returns(new Office());

            var exception = Assert.Throws<Model.Exceptions.Office.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateOfficeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>()), Times.Never);
            this.mockRepositoryOffice.Verify(mrt => mrt.Update(It.IsAny<Office>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var offices = new List<Office>() { new Office() { Id = 1 } }.AsQueryable();
            var readedOfficeList = new List<ReadedOfficeContract> { new ReadedOfficeContract { Id = 1 } };
            this.mockRepositoryOffice.Setup(mrt => mrt.QueryEager()).Returns(offices);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedOfficeContract>>(It.IsAny<List<Office>>())).Returns(readedOfficeList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryOffice.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedOfficeContract>>(It.IsAny<List<Office>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var offices = new List<Office>() { new Office() { Id = 1 } }.AsQueryable();
            var readedOffice = new ReadedOfficeContract { Id = 1 };
            this.mockRepositoryOffice.Setup(mrt => mrt.QueryEager()).Returns(offices);
            this.mockMapper.Setup(mm => mm.Map<ReadedOfficeContract>(It.IsAny<Office>())).Returns(readedOffice);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedOffice, actualResult);
            this.mockRepositoryOffice.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedOfficeContract>(It.IsAny<Office>()), Times.Once);
        }
    }
}