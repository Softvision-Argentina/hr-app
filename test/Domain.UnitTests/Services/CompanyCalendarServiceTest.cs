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
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class CompanyCalendarServiceTest : BaseDomainTest
    {
        private readonly CompanyCalendarService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<CompanyCalendar>> _mockRepositoryCompanyCalendar;        
        private readonly Mock<ILog<CompanyCalendarService>> _mockLogCompanyCalendarService;
        private readonly Mock<IGoogleCalendarService> _mockGoogleCalendarService;
        private readonly Mock<UpdateCompanyCalendarContractValidator> _mockUpdateCompanyCalendarContractValidator;
        private readonly Mock<CreateCompanyCalendarContractValidator> _mockCreateCompanyCalendarContractValidator;

        public CompanyCalendarServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryCompanyCalendar = new Mock<IRepository<CompanyCalendar>>();            
            _mockLogCompanyCalendarService = new Mock<ILog<CompanyCalendarService>>();
            _mockGoogleCalendarService = new Mock<IGoogleCalendarService>();
            _mockUpdateCompanyCalendarContractValidator = new Mock<UpdateCompanyCalendarContractValidator>();
            _mockCreateCompanyCalendarContractValidator = new Mock<CreateCompanyCalendarContractValidator>();
            _service = new CompanyCalendarService(
                _mockMapper.Object,
                _mockRepositoryCompanyCalendar.Object,                
                MockUnitOfWork.Object,
                _mockLogCompanyCalendarService.Object,
                _mockGoogleCalendarService.Object,
                _mockUpdateCompanyCalendarContractValidator.Object,
                _mockCreateCompanyCalendarContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create CompanyCalendarService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateCompanyCalendarService()
        {
            var contract = new CreateCompanyCalendarContract();
            var expectedCompanyCalendar = new CreatedCompanyCalendarContract();
            _mockCreateCompanyCalendarContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCompanyCalendarContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<CompanyCalendar>(It.IsAny<CreateCompanyCalendarContract>())).Returns(new CompanyCalendar());
            _mockMapper.Setup(mm => mm.Map<CreatedCompanyCalendarContract>(It.IsAny<CompanyCalendar>())).Returns(expectedCompanyCalendar);            
            _mockRepositoryCompanyCalendar.Setup(repoCom => repoCom.Create(It.IsAny<CompanyCalendar>())).Returns(new CompanyCalendar());
            

            var createdCompanyCalendar = _service.Create(contract);

            Assert.NotNull(createdCompanyCalendar);
            Assert.Equal(expectedCompanyCalendar, createdCompanyCalendar);            
            _mockCreateCompanyCalendarContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCompanyCalendarContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CompanyCalendar>(It.IsAny<CreateCompanyCalendarContract>()), Times.Once);
            _mockRepositoryCompanyCalendar.Verify(mrt => mrt.Create(It.IsAny<CompanyCalendar>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedCompanyCalendarContract>(It.IsAny<CompanyCalendar>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateCompanyCalendarContract();
            var expectedCompanyCalendar = new CreatedCompanyCalendarContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateCompanyCalendarContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCompanyCalendarContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedCompanyCalendarContract>(It.IsAny<CompanyCalendar>())).Returns(expectedCompanyCalendar);

            var exception = Assert.Throws<Model.Exceptions.CompanyCalendar.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);            
            _mockCreateCompanyCalendarContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCompanyCalendarContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CompanyCalendar>(It.IsAny<CreateCompanyCalendarContract>()), Times.Never);
            _mockRepositoryCompanyCalendar.Verify(mrt => mrt.Create(It.IsAny<CompanyCalendar>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedCompanyCalendarContract>(It.IsAny<CompanyCalendar>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete CompanyCalendarService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteCompanyCalendarService()
        {
            var communities = new List<CompanyCalendar>() { new CompanyCalendar() { Id = 1 } }.AsQueryable();
            _mockRepositoryCompanyCalendar.Setup(mrt => mrt.Query()).Returns(communities);

            _service.Delete(1);

            _mockLogCompanyCalendarService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryCompanyCalendar.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryCompanyCalendar.Verify(mrt => mrt.Delete(It.IsAny<CompanyCalendar>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteCompanyCalendarNotFoundException()
        {
            var expectedErrorMEssage = $"Company calendar not found for the Company calendar Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.CompanyCalendar.DeleteCompanyCalendarNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogCompanyCalendarService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryCompanyCalendar.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryCompanyCalendar.Verify(mrt => mrt.Delete(It.IsAny<CompanyCalendar>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update CompanyCalendarService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateCompanyCalendarContract();
            _mockUpdateCompanyCalendarContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCompanyCalendarContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<CompanyCalendar>(It.IsAny<CompanyCalendar>())).Returns(new CompanyCalendar());
            _mockMapper.Setup(mm => mm.Map<CompanyCalendar>(It.IsAny<UpdateCompanyCalendarContract>())).Returns(new CompanyCalendar());
            _mockRepositoryCompanyCalendar.Setup(x => x.Update(It.IsAny<CompanyCalendar>())).Returns(new CompanyCalendar());

            _service.Update(contract);
            
            _mockUpdateCompanyCalendarContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCompanyCalendarContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CompanyCalendar>(It.IsAny<UpdateCompanyCalendarContract>()), Times.Once);
            _mockRepositoryCompanyCalendar.Verify(mrt => mrt.Update(It.IsAny<CompanyCalendar>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateCompanyCalendarContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateCompanyCalendarContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCompanyCalendarContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CompanyCalendar>(It.IsAny<UpdateCompanyCalendarContract>())).Returns(new CompanyCalendar());
            _mockMapper.Setup(mm => mm.Map<CompanyCalendar>(It.IsAny<CompanyCalendar>())).Returns(new CompanyCalendar());

            var exception = Assert.Throws<Model.Exceptions.CompanyCalendar.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockUpdateCompanyCalendarContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCompanyCalendarContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CompanyCalendar>(It.IsAny<UpdateCompanyCalendarContract>()), Times.Never);
            _mockRepositoryCompanyCalendar.Verify(mrt => mrt.Update(It.IsAny<CompanyCalendar>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            _mockCreateCompanyCalendarContractValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreateCompanyCalendarContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("Tittle","isEmmpty") }));
            var companyCalendarList = new List<CompanyCalendar>() { new CompanyCalendar() }.AsQueryable();
            var readedCompanyCalendarList = new List<ReadedCompanyCalendarContract> { new ReadedCompanyCalendarContract () };
            _mockRepositoryCompanyCalendar.Setup(x => x.QueryEager()).Returns(companyCalendarList);
            _mockMapper.Setup(mm => mm.Map<List<ReadedCompanyCalendarContract>>(It.IsAny<List<CompanyCalendar>>())).Returns(readedCompanyCalendarList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Single(actualResult);
            _mockRepositoryCompanyCalendar.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedCompanyCalendarContract>>(It.IsAny<List<CompanyCalendar>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<CompanyCalendar>() { new CompanyCalendar() { Id = 1 } }.AsQueryable();
            var readedCompanyCalendar = new ReadedCompanyCalendarContract { Id = 1};
            _mockRepositoryCompanyCalendar.Setup(mrt => mrt.QueryEager()).Returns(communities);
            _mockMapper.Setup(mm => mm.Map<ReadedCompanyCalendarContract>(It.IsAny<CompanyCalendar>())).Returns(readedCompanyCalendar);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);            
            _mockRepositoryCompanyCalendar.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedCompanyCalendarContract>(It.IsAny<CompanyCalendar>()), Times.Once);
        }
    }
}