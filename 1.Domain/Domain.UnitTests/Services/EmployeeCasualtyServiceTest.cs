using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.EmployeeCasualty;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.EmployeeCasualty;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class EmployeeCasualtyServiceTest : BaseDomainTest
    {
        private readonly EmployeeCasualtyService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<EmployeeCasualty>> mockRepositoryEmployeeCasualty;
        private readonly Mock<IRepository<CandidateProfile>> mockRepositoryCandidateProfile;
        private readonly Mock<IRepository<Model.EmployeeCasualty>> mockRepositoryModelEmployeeCasualty;
        private readonly Mock<ILog<EmployeeCasualtyService>> mockLogEmployeeCasualtyService;
        private readonly Mock<UpdateEmployeeCasualtyContractValidator> mockUpdateEmployeeCasualtyContractValidator;
        private readonly Mock<CreateEmployeeCasualtyContractValidator> mockCreateEmployeeCasualtyContractValidator;

        public EmployeeCasualtyServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryEmployeeCasualty = new Mock<IRepository<EmployeeCasualty>>();
            mockRepositoryCandidateProfile = new Mock<IRepository<CandidateProfile>>();
            mockRepositoryModelEmployeeCasualty = new Mock<IRepository<Model.EmployeeCasualty>>();
            mockLogEmployeeCasualtyService = new Mock<ILog<EmployeeCasualtyService>>();
            mockUpdateEmployeeCasualtyContractValidator = new Mock<UpdateEmployeeCasualtyContractValidator>();
            mockCreateEmployeeCasualtyContractValidator = new Mock<CreateEmployeeCasualtyContractValidator>();
            service = new EmployeeCasualtyService(
                mockMapper.Object,
                mockRepositoryEmployeeCasualty.Object,                
                MockUnitOfWork.Object,
                mockLogEmployeeCasualtyService.Object,
                mockUpdateEmployeeCasualtyContractValidator.Object,
                mockCreateEmployeeCasualtyContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create EmployeeCasualtyService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateEmployeeCasualtyService()
        {
            var contract = new CreateEmployeeCasualtyContract();
            var expectedEmployeeCasualty = new CreatedEmployeeCasualtyContract();
            mockCreateEmployeeCasualtyContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<EmployeeCasualty>(It.IsAny<CreateEmployeeCasualtyContract>())).Returns(new EmployeeCasualty());
            mockRepositoryEmployeeCasualty.Setup(repoCom => repoCom.Create(It.IsAny<EmployeeCasualty>())).Returns(new EmployeeCasualty());
            mockMapper.Setup(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>())).Returns(expectedEmployeeCasualty);

            var createdEmployeeCasualty = service.Create(contract);

            Assert.NotNull(createdEmployeeCasualty);
            Assert.Equal(expectedEmployeeCasualty, createdEmployeeCasualty);
            mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateEmployeeCasualtyContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<CreateEmployeeCasualtyContract>()), Times.Once);
            mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Create(It.IsAny<EmployeeCasualty>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateEmployeeCasualtyContract();
            var expectedEmployeeCasualty = new CreatedEmployeeCasualtyContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateEmployeeCasualtyContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>())).Returns(expectedEmployeeCasualty);

            var exception = Assert.Throws<Model.Exceptions.EmployeeCasualty.CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateEmployeeCasualtyContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<CreateEmployeeCasualtyContract>()), Times.Never);
            mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Create(It.IsAny<EmployeeCasualty>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            mockMapper.Verify(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete EmployeeCasualtyService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteEmployeeCasualtyService()
        {
            var EmployeeCasualtys = new List<EmployeeCasualty>() { new EmployeeCasualty() { Id = 1 } }.AsQueryable();
            mockRepositoryEmployeeCasualty.Setup(mrt => mrt.Query()).Returns(EmployeeCasualtys);

            service.Delete(1);

            mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Delete(It.IsAny<EmployeeCasualty>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteEmployeeCasualtyNotFoundException()
        {
            var expectedErrorMEssage = $"Employee casualty not found for the EmployeeCasualtyId: {0}";

            var exception = Assert.Throws<Model.Exceptions.EmployeeCasualty.DeleteEmployeeCasualtyNotFoundException>(() => service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Delete(It.IsAny<EmployeeCasualty>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update EmployeeCasualtyService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateEmployeeCasualtyContract();
            mockUpdateEmployeeCasualtyContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>())).Returns(new EmployeeCasualty());

            service.Update(contract);

            mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateEmployeeCasualtyContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>()), Times.Once);
            mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Update(It.IsAny<EmployeeCasualty>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateEmployeeCasualtyContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateEmployeeCasualtyContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>())).Returns(new EmployeeCasualty());

            var exception = Assert.Throws<Model.Exceptions.EmployeeCasualty.CreateContractInvalidException>(() => service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateEmployeeCasualtyContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>()), Times.Never);
            mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Update(It.IsAny<EmployeeCasualty>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var EmployeeCasualtys = new List<EmployeeCasualty>() { new EmployeeCasualty() { Id = 1 } }.AsQueryable();
            var readedEmployeeCasualtyList = new List<ReadedEmployeeCasualtyContract> { new ReadedEmployeeCasualtyContract { Id = 1 } };
            mockRepositoryEmployeeCasualty.Setup(mrt => mrt.QueryEager()).Returns(EmployeeCasualtys);
            mockMapper.Setup(mm => mm.Map<List<ReadedEmployeeCasualtyContract>>(It.IsAny<List<EmployeeCasualty>>())).Returns(readedEmployeeCasualtyList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryEmployeeCasualty.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedEmployeeCasualtyContract>>(It.IsAny<List<EmployeeCasualty>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var EmployeeCasualtys = new List<EmployeeCasualty>() { new EmployeeCasualty() { Id = 1 } }.AsQueryable();
            var readedEmployeeCasualty = new ReadedEmployeeCasualtyContract { Id = 1 };
            mockRepositoryEmployeeCasualty.Setup(mrt => mrt.QueryEager()).Returns(EmployeeCasualtys);
            mockMapper.Setup(mm => mm.Map<ReadedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>())).Returns(readedEmployeeCasualty);

            var actualResult = service.Read(1);

            Assert.NotNull(actualResult);            
            mockRepositoryEmployeeCasualty.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>()), Times.Once);
        }
    }
}