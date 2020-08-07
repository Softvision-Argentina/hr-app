// <copyright file="DaysOffServiceTest.cs" company="Softvision">
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
    using Domain.Model.Enum;
    using Domain.Model.Exceptions.DaysOff;
    using Domain.Services.Contracts.DaysOff;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.DaysOff;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class DaysOffServiceTest : BaseDomainTest
    {
        private readonly DaysOffService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<DaysOff>> mockRepositoryDaysOff;
        private readonly Mock<ILog<DaysOffService>> mockLogDaysOffService;
        private readonly Mock<IGoogleCalendarService> mockCalendarservice;
        private readonly Mock<UpdateDaysOffContractValidator> mockUpdateDaysOffContractValidator;
        private readonly Mock<CreateDaysOffContractValidator> mockCreateDaysOffContractValidator;

        public DaysOffServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryDaysOff = new Mock<IRepository<DaysOff>>();
            this.mockCalendarservice = new Mock<IGoogleCalendarService>();
            this.mockLogDaysOffService = new Mock<ILog<DaysOffService>>();
            this.mockUpdateDaysOffContractValidator = new Mock<UpdateDaysOffContractValidator>();
            this.mockCreateDaysOffContractValidator = new Mock<CreateDaysOffContractValidator>();
            this.service = new DaysOffService(
                this.mockMapper.Object,
                this.mockRepositoryDaysOff.Object,
                this.MockUnitOfWork.Object,
                this.mockLogDaysOffService.Object,
                this.mockCalendarservice.Object,
                this.mockUpdateDaysOffContractValidator.Object,
                this.mockCreateDaysOffContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create DaysOffService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateDaysOffService()
        {
            var contract = new CreateDaysOffContract();
            var expectedDaysOff = new CreatedDaysOffContract();
            var daysOff = new DaysOff
            {
                Status = DaysOffStatus.Accepted,
                Employee = new Employee { EmailAddress = "testc@mail" },
            };
            this.mockCreateDaysOffContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDaysOffContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<DaysOff>(It.IsAny<CreateDaysOffContract>())).Returns(daysOff);
            this.mockRepositoryDaysOff.Setup(repoCom => repoCom.Create(It.IsAny<DaysOff>())).Returns(new DaysOff());
            this.mockMapper.Setup(mm => mm.Map<CreatedDaysOffContract>(It.IsAny<DaysOff>())).Returns(expectedDaysOff);

            var createdDaysOff = this.service.Create(contract);

            Assert.NotNull(createdDaysOff);
            Assert.Equal(expectedDaysOff, createdDaysOff);
            this.mockCreateDaysOffContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDaysOffContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<DaysOff>(It.IsAny<CreateDaysOffContract>()), Times.Once);
            this.mockRepositoryDaysOff.Verify(mrt => mrt.Create(It.IsAny<DaysOff>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedDaysOffContract>(It.IsAny<DaysOff>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateDaysOffContract();
            var expectedDaysOff = new CreatedDaysOffContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateDaysOffContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDaysOffContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedDaysOffContract>(It.IsAny<DaysOff>())).Returns(expectedDaysOff);

            var exception = Assert.Throws<Model.Exceptions.DaysOff.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockCreateDaysOffContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDaysOffContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<DaysOff>(It.IsAny<CreateDaysOffContract>()), Times.Never);
            this.mockRepositoryDaysOff.Verify(mrt => mrt.Create(It.IsAny<DaysOff>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedDaysOffContract>(It.IsAny<DaysOff>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete DaysOffService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteDaysOffService()
        {
            var daysOffList = new List<DaysOff>() { new DaysOff() { Id = 1 } }.AsQueryable();
            this.mockRepositoryDaysOff.Setup(mrt => mrt.Query()).Returns(daysOffList);

            this.service.Delete(1);

            this.mockRepositoryDaysOff.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryDaysOff.Verify(mrt => mrt.Delete(It.IsAny<DaysOff>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteDaysOffNotFoundException()
        {
            var expectedErrorMEssage = $"Days off not found for the DaysOffId: {0}";

            var exception = Assert.Throws<Model.Exceptions.DaysOff.DeleteDaysOffNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockRepositoryDaysOff.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryDaysOff.Verify(mrt => mrt.Delete(It.IsAny<DaysOff>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update DaysOffService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateDaysOffContract();
            var daysOff = new DaysOff
            {
                Status = DaysOffStatus.Accepted,
                Employee = new Employee { EmailAddress = "test@mail.com" },
            };
            this.mockRepositoryDaysOff.Setup(x => x.Update(It.IsAny<DaysOff>())).Returns(new DaysOff());
            this.mockUpdateDaysOffContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDaysOffContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<DaysOff>(It.IsAny<UpdateDaysOffContract>())).Returns(daysOff);

            this.service.Update(contract);

            this.mockUpdateDaysOffContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDaysOffContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<DaysOff>(It.IsAny<UpdateDaysOffContract>()), Times.Once);
            this.mockRepositoryDaysOff.Verify(mrt => mrt.Update(It.IsAny<DaysOff>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateDaysOffContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateDaysOffContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDaysOffContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<DaysOff>(It.IsAny<UpdateDaysOffContract>())).Returns(new DaysOff());

            var exception = Assert.Throws<CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockUpdateDaysOffContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDaysOffContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<DaysOff>(It.IsAny<UpdateDaysOffContract>()), Times.Never);
            this.mockRepositoryDaysOff.Verify(mrt => mrt.Update(It.IsAny<DaysOff>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var daysOffList = new List<DaysOff>() { new DaysOff() { Id = 1 } }.AsQueryable();
            var readedDaysOffList = new List<ReadedDaysOffContract> { new ReadedDaysOffContract { Id = 1 } };
            this.mockRepositoryDaysOff.Setup(mrt => mrt.QueryEager()).Returns(daysOffList);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedDaysOffContract>>(It.IsAny<List<DaysOff>>())).Returns(readedDaysOffList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryDaysOff.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDaysOffContract>>(It.IsAny<List<DaysOff>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<DaysOff>() { new DaysOff() { Id = 1 } }.AsQueryable();
            var readedDaysOff = new ReadedDaysOffContract { Id = 1 };
            this.mockRepositoryDaysOff.Setup(mrt => mrt.QueryEager()).Returns(communities);
            this.mockMapper.Setup(mm => mm.Map<ReadedDaysOffContract>(It.IsAny<DaysOff>())).Returns(readedDaysOff);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            this.mockRepositoryDaysOff.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedDaysOffContract>(It.IsAny<DaysOff>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that AcceptPetition runs correctly")]
        public void GivenAcceptPetition_WhenValidData_ReturnsValue()
        {
            var daysOffList = new List<DaysOff>()
            {
                new DaysOff()
                {
                    Id = 1,
                    Employee = new Employee { EmailAddress = "test@mail.com" },
                },
            }.AsQueryable();
            var readedDaysOff = new ReadedDaysOffContract { Id = 1 };
            this.mockRepositoryDaysOff.Setup(x => x.Query()).Returns(daysOffList);
            this.mockRepositoryDaysOff.Setup(x => x.Update(It.IsAny<DaysOff>())).Returns(new DaysOff());

            this.service.AcceptPetition(1);

            this.mockRepositoryDaysOff.Verify(_ => _.Query(), Times.Once);
            this.mockRepositoryDaysOff.Verify(_ => _.Update(It.IsAny<DaysOff>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that AcceptPetition throws exception")]
        public void GivenAcceptPetition_WhenInvalidData_ThrowsUpdateDaysOffNotFoundException()
        {
            var exception = Assert.Throws<UpdateDaysOffNotFoundException>(() => this.service.AcceptPetition(1));

            Assert.NotNull(exception);
        }

        [Fact(DisplayName = "Verify that ReadByDni returns a value")]
        public void GivenReadByDni_WhenRegularCall_ReturnsValue()
        {
            var daysOffList = new List<DaysOff>()
            {
                new DaysOff() { Id = 1, Employee = new Employee { DNI = 1 } },
            }
            .AsQueryable();
            var readedDaysOffList = new List<ReadedDaysOffContract> { new ReadedDaysOffContract { Id = 1 } };
            this.mockRepositoryDaysOff.Setup(mrt => mrt.QueryEager()).Returns(daysOffList);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedDaysOffContract>>(It.IsAny<List<DaysOff>>())).Returns(readedDaysOffList);

            var actualResult = this.service.ReadByDni(1);

            Assert.NotNull(actualResult);
            this.mockRepositoryDaysOff.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedDaysOffContract>>(It.IsAny<List<DaysOff>>()), Times.Once);
        }
    }
}