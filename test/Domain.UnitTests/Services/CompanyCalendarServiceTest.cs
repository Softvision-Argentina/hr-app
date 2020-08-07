// <copyright file="CompanyCalendarServiceTest.cs" company="Softvision">
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
    using Domain.Services.Contracts.CompanyCalendar;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.CompanyCalendar;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class CompanyCalendarServiceTest : BaseDomainTest
    {
        private readonly CompanyCalendarService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<CompanyCalendar>> mockRepositoryCompanyCalendar;
        private readonly Mock<ILog<CompanyCalendarService>> mockLogCompanyCalendarService;
        private readonly Mock<IGoogleCalendarService> mockGoogleCalendarService;
        private readonly Mock<UpdateCompanyCalendarContractValidator> mockUpdateCompanyCalendarContractValidator;
        private readonly Mock<CreateCompanyCalendarContractValidator> mockCreateCompanyCalendarContractValidator;

        public CompanyCalendarServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryCompanyCalendar = new Mock<IRepository<CompanyCalendar>>();
            this.mockLogCompanyCalendarService = new Mock<ILog<CompanyCalendarService>>();
            this.mockGoogleCalendarService = new Mock<IGoogleCalendarService>();
            this.mockUpdateCompanyCalendarContractValidator = new Mock<UpdateCompanyCalendarContractValidator>();
            this.mockCreateCompanyCalendarContractValidator = new Mock<CreateCompanyCalendarContractValidator>();
            this.service = new CompanyCalendarService(
                this.mockMapper.Object,
                this.mockRepositoryCompanyCalendar.Object,
                this.MockUnitOfWork.Object,
                this.mockLogCompanyCalendarService.Object,
                this.mockGoogleCalendarService.Object,
                this.mockUpdateCompanyCalendarContractValidator.Object,
                this.mockCreateCompanyCalendarContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create CompanyCalendarService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateCompanyCalendarService()
        {
            var contract = new CreateCompanyCalendarContract();
            var expectedCompanyCalendar = new CreatedCompanyCalendarContract();
            this.mockCreateCompanyCalendarContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCompanyCalendarContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<CompanyCalendar>(It.IsAny<CreateCompanyCalendarContract>())).Returns(new CompanyCalendar());
            this.mockMapper.Setup(mm => mm.Map<CreatedCompanyCalendarContract>(It.IsAny<CompanyCalendar>())).Returns(expectedCompanyCalendar);
            this.mockRepositoryCompanyCalendar.Setup(repoCom => repoCom.Create(It.IsAny<CompanyCalendar>())).Returns(new CompanyCalendar());

            var createdCompanyCalendar = this.service.Create(contract);

            Assert.NotNull(createdCompanyCalendar);
            Assert.Equal(expectedCompanyCalendar, createdCompanyCalendar);
            this.mockCreateCompanyCalendarContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCompanyCalendarContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CompanyCalendar>(It.IsAny<CreateCompanyCalendarContract>()), Times.Once);
            this.mockRepositoryCompanyCalendar.Verify(mrt => mrt.Create(It.IsAny<CompanyCalendar>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedCompanyCalendarContract>(It.IsAny<CompanyCalendar>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateCompanyCalendarContract();
            var expectedCompanyCalendar = new CreatedCompanyCalendarContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateCompanyCalendarContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCompanyCalendarContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedCompanyCalendarContract>(It.IsAny<CompanyCalendar>())).Returns(expectedCompanyCalendar);

            var exception = Assert.Throws<Model.Exceptions.CompanyCalendar.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockCreateCompanyCalendarContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCompanyCalendarContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CompanyCalendar>(It.IsAny<CreateCompanyCalendarContract>()), Times.Never);
            this.mockRepositoryCompanyCalendar.Verify(mrt => mrt.Create(It.IsAny<CompanyCalendar>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedCompanyCalendarContract>(It.IsAny<CompanyCalendar>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete CompanyCalendarService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteCompanyCalendarService()
        {
            var communities = new List<CompanyCalendar>() { new CompanyCalendar() { Id = 1 } }.AsQueryable();
            this.mockRepositoryCompanyCalendar.Setup(mrt => mrt.Query()).Returns(communities);

            this.service.Delete(1);

            this.mockLogCompanyCalendarService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryCompanyCalendar.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryCompanyCalendar.Verify(mrt => mrt.Delete(It.IsAny<CompanyCalendar>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteCompanyCalendarNotFoundException()
        {
            var expectedErrorMEssage = $"Company calendar not found for the Company calendar Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.CompanyCalendar.DeleteCompanyCalendarNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogCompanyCalendarService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryCompanyCalendar.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryCompanyCalendar.Verify(mrt => mrt.Delete(It.IsAny<CompanyCalendar>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update CompanyCalendarService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateCompanyCalendarContract();
            this.mockUpdateCompanyCalendarContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCompanyCalendarContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<CompanyCalendar>(It.IsAny<CompanyCalendar>())).Returns(new CompanyCalendar());
            this.mockMapper.Setup(mm => mm.Map<CompanyCalendar>(It.IsAny<UpdateCompanyCalendarContract>())).Returns(new CompanyCalendar());
            this.mockRepositoryCompanyCalendar.Setup(x => x.Update(It.IsAny<CompanyCalendar>())).Returns(new CompanyCalendar());

            this.service.Update(contract);

            this.mockUpdateCompanyCalendarContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCompanyCalendarContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CompanyCalendar>(It.IsAny<UpdateCompanyCalendarContract>()), Times.Once);
            this.mockRepositoryCompanyCalendar.Verify(mrt => mrt.Update(It.IsAny<CompanyCalendar>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateCompanyCalendarContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateCompanyCalendarContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCompanyCalendarContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CompanyCalendar>(It.IsAny<UpdateCompanyCalendarContract>())).Returns(new CompanyCalendar());
            this.mockMapper.Setup(mm => mm.Map<CompanyCalendar>(It.IsAny<CompanyCalendar>())).Returns(new CompanyCalendar());

            var exception = Assert.Throws<Model.Exceptions.CompanyCalendar.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockUpdateCompanyCalendarContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCompanyCalendarContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CompanyCalendar>(It.IsAny<UpdateCompanyCalendarContract>()), Times.Never);
            this.mockRepositoryCompanyCalendar.Verify(mrt => mrt.Update(It.IsAny<CompanyCalendar>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            this.mockCreateCompanyCalendarContractValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreateCompanyCalendarContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("Tittle", "isEmmpty") }));
            var companyCalendarList = new List<CompanyCalendar>() { new CompanyCalendar() }.AsQueryable();
            var readedCompanyCalendarList = new List<ReadedCompanyCalendarContract> { new ReadedCompanyCalendarContract() };
            this.mockRepositoryCompanyCalendar.Setup(x => x.QueryEager()).Returns(companyCalendarList);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedCompanyCalendarContract>>(It.IsAny<List<CompanyCalendar>>())).Returns(readedCompanyCalendarList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Single(actualResult);
            this.mockRepositoryCompanyCalendar.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedCompanyCalendarContract>>(It.IsAny<List<CompanyCalendar>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<CompanyCalendar>() { new CompanyCalendar() { Id = 1 } }.AsQueryable();
            var readedCompanyCalendar = new ReadedCompanyCalendarContract { Id = 1 };
            this.mockRepositoryCompanyCalendar.Setup(mrt => mrt.QueryEager()).Returns(communities);
            this.mockMapper.Setup(mm => mm.Map<ReadedCompanyCalendarContract>(It.IsAny<CompanyCalendar>())).Returns(readedCompanyCalendar);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            this.mockRepositoryCompanyCalendar.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedCompanyCalendarContract>(It.IsAny<CompanyCalendar>()), Times.Once);
        }
    }
}