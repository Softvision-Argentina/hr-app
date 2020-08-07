// <copyright file="EmployeeCasualtyServiceTest.cs" company="Softvision">
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
    using Domain.Services.Contracts.EmployeeCasualty;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.EmployeeCasualty;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class EmployeeCasualtyServiceTest : BaseDomainTest
    {
        private readonly EmployeeCasualtyService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<EmployeeCasualty>> mockRepositoryEmployeeCasualty;
        private readonly Mock<ILog<EmployeeCasualtyService>> mockLogEmployeeCasualtyService;
        private readonly Mock<UpdateEmployeeCasualtyContractValidator> mockUpdateEmployeeCasualtyContractValidator;
        private readonly Mock<CreateEmployeeCasualtyContractValidator> mockCreateEmployeeCasualtyContractValidator;

        public EmployeeCasualtyServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryEmployeeCasualty = new Mock<IRepository<EmployeeCasualty>>();
            this.mockLogEmployeeCasualtyService = new Mock<ILog<EmployeeCasualtyService>>();
            this.mockUpdateEmployeeCasualtyContractValidator = new Mock<UpdateEmployeeCasualtyContractValidator>();
            this.mockCreateEmployeeCasualtyContractValidator = new Mock<CreateEmployeeCasualtyContractValidator>();
            this.service = new EmployeeCasualtyService(
                this.mockMapper.Object,
                this.mockRepositoryEmployeeCasualty.Object,
                this.MockUnitOfWork.Object,
                this.mockLogEmployeeCasualtyService.Object,
                this.mockUpdateEmployeeCasualtyContractValidator.Object,
                this.mockCreateEmployeeCasualtyContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create EmployeeCasualtyService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateEmployeeCasualtyService()
        {
            var contract = new CreateEmployeeCasualtyContract();
            var expectedEmployeeCasualty = new CreatedEmployeeCasualtyContract();
            this.mockCreateEmployeeCasualtyContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<EmployeeCasualty>(It.IsAny<CreateEmployeeCasualtyContract>())).Returns(new EmployeeCasualty());
            this.mockRepositoryEmployeeCasualty.Setup(repoCom => repoCom.Create(It.IsAny<EmployeeCasualty>())).Returns(new EmployeeCasualty());
            this.mockMapper.Setup(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>())).Returns(expectedEmployeeCasualty);

            var createdEmployeeCasualty = this.service.Create(contract);

            Assert.NotNull(createdEmployeeCasualty);
            Assert.Equal(expectedEmployeeCasualty, createdEmployeeCasualty);
            this.mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateEmployeeCasualtyContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<CreateEmployeeCasualtyContract>()), Times.Once);
            this.mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Create(It.IsAny<EmployeeCasualty>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateEmployeeCasualtyContract();
            var expectedEmployeeCasualty = new CreatedEmployeeCasualtyContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateEmployeeCasualtyContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>())).Returns(expectedEmployeeCasualty);

            var exception = Assert.Throws<Model.Exceptions.EmployeeCasualty.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateEmployeeCasualtyContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<CreateEmployeeCasualtyContract>()), Times.Never);
            this.mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Create(It.IsAny<EmployeeCasualty>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete EmployeeCasualtyService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteEmployeeCasualtyService()
        {
            var employeeCasualtys = new List<EmployeeCasualty>() { new EmployeeCasualty() { Id = 1 } }.AsQueryable();
            this.mockRepositoryEmployeeCasualty.Setup(mrt => mrt.Query()).Returns(employeeCasualtys);

            this.service.Delete(1);

            this.mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Delete(It.IsAny<EmployeeCasualty>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteEmployeeCasualtyNotFoundException()
        {
            var expectedErrorMEssage = $"Employee casualty not found for the EmployeeCasualtyId: {0}";

            var exception = Assert.Throws<Model.Exceptions.EmployeeCasualty.DeleteEmployeeCasualtyNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Delete(It.IsAny<EmployeeCasualty>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update EmployeeCasualtyService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateEmployeeCasualtyContract();
            this.mockUpdateEmployeeCasualtyContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>())).Returns(new EmployeeCasualty());

            this.service.Update(contract);

            this.mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateEmployeeCasualtyContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>()), Times.Once);
            this.mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Update(It.IsAny<EmployeeCasualty>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateEmployeeCasualtyContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateEmployeeCasualtyContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>())).Returns(new EmployeeCasualty());

            var exception = Assert.Throws<Model.Exceptions.EmployeeCasualty.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateEmployeeCasualtyContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>()), Times.Never);
            this.mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Update(It.IsAny<EmployeeCasualty>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var employeeCasualtys = new List<EmployeeCasualty>() { new EmployeeCasualty() { Id = 1 } }.AsQueryable();
            var readedEmployeeCasualtyList = new List<ReadedEmployeeCasualtyContract> { new ReadedEmployeeCasualtyContract { Id = 1 } };
            this.mockRepositoryEmployeeCasualty.Setup(mrt => mrt.QueryEager()).Returns(employeeCasualtys);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedEmployeeCasualtyContract>>(It.IsAny<List<EmployeeCasualty>>())).Returns(readedEmployeeCasualtyList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryEmployeeCasualty.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedEmployeeCasualtyContract>>(It.IsAny<List<EmployeeCasualty>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var employeeCasualtys = new List<EmployeeCasualty>() { new EmployeeCasualty() { Id = 1 } }.AsQueryable();
            var readedEmployeeCasualty = new ReadedEmployeeCasualtyContract { Id = 1 };
            this.mockRepositoryEmployeeCasualty.Setup(mrt => mrt.QueryEager()).Returns(employeeCasualtys);
            this.mockMapper.Setup(mm => mm.Map<ReadedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>())).Returns(readedEmployeeCasualty);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            this.mockRepositoryEmployeeCasualty.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>()), Times.Once);
        }
    }
}