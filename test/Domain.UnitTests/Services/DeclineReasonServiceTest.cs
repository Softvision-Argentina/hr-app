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
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class DeclineReasonServiceTest : BaseDomainTest
    {
        private readonly DeclineReasonService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<DeclineReason>> _mockRepositoryDeclineReason;        
        private readonly Mock<ILog<DeclineReasonService>> _mockLogDeclineReasonService;
        private readonly Mock<UpdateDeclineReasonContractValidator> _mockUpdateDeclineReasonContractValidator;
        private readonly Mock<CreateDeclineReasonContractValidator> _mockCreateDeclineReasonContractValidator;

        public DeclineReasonServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryDeclineReason = new Mock<IRepository<DeclineReason>>();
            _mockLogDeclineReasonService = new Mock<ILog<DeclineReasonService>>();
            _mockUpdateDeclineReasonContractValidator = new Mock<UpdateDeclineReasonContractValidator>();
            _mockCreateDeclineReasonContractValidator = new Mock<CreateDeclineReasonContractValidator>();
            _service = new DeclineReasonService(
                _mockMapper.Object,
                _mockRepositoryDeclineReason.Object,                
                MockUnitOfWork.Object,
                _mockLogDeclineReasonService.Object,
                _mockUpdateDeclineReasonContractValidator.Object,
                _mockCreateDeclineReasonContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create DeclineReasonService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateDeclineReasonService()
        {
            var contract = new CreateDeclineReasonContract();
            var expectedDeclineReason = new CreatedDeclineReasonContract();
            _mockCreateDeclineReasonContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<DeclineReason>(It.IsAny<CreateDeclineReasonContract>())).Returns(new DeclineReason());
            _mockRepositoryDeclineReason.Setup(repoCom => repoCom.Create(It.IsAny<DeclineReason>())).Returns(new DeclineReason());
            _mockMapper.Setup(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>())).Returns(expectedDeclineReason);

            var createdDeclineReason = _service.Create(contract);

            Assert.NotNull(createdDeclineReason);
            Assert.Equal(expectedDeclineReason, createdDeclineReason);
            _mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateDeclineReasonContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<CreateDeclineReasonContract>()), Times.Once);
            _mockRepositoryDeclineReason.Verify(mrt => mrt.Create(It.IsAny<DeclineReason>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateDeclineReasonContract();
            var expectedDeclineReason = new CreatedDeclineReasonContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateDeclineReasonContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>())).Returns(expectedDeclineReason);

            var exception = Assert.Throws<CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateDeclineReasonContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<CreateDeclineReasonContract>()), Times.Never);
            _mockRepositoryDeclineReason.Verify(mrt => mrt.Create(It.IsAny<DeclineReason>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete DeclineReasonService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteDeclineReasonService()
        {
            var DeclineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1 } }.AsQueryable();
            _mockRepositoryDeclineReason.Setup(mrt => mrt.Query()).Returns(DeclineReasons);

            _service.Delete(1);

            _mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryDeclineReason.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryDeclineReason.Verify(mrt => mrt.Delete(It.IsAny<DeclineReason>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteDeclineReasonNotFoundException()
        {
            var expectedErrorMEssage = $"Skill type not found for the skillTypeId: {0}";

            var exception = Assert.Throws<DeleteDeclineReasonNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryDeclineReason.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryDeclineReason.Verify(mrt => mrt.Delete(It.IsAny<DeclineReason>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update DeclineReasonService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateDeclineReasonContract();
            _mockUpdateDeclineReasonContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>())).Returns(new DeclineReason());

            _service.Update(contract);

            _mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateDeclineReasonContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>()), Times.Once);
            _mockRepositoryDeclineReason.Verify(mrt => mrt.Update(It.IsAny<DeclineReason>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateDeclineReasonContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateDeclineReasonContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>())).Returns(new DeclineReason());

            var exception = Assert.Throws<CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateDeclineReasonContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>()), Times.Never);
            _mockRepositoryDeclineReason.Verify(mrt => mrt.Update(It.IsAny<DeclineReason>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var DeclineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1 } }.AsQueryable();
            var readedDeclineReasonList = new List<ReadedDeclineReasonContract> { new ReadedDeclineReasonContract { Id = 1 } };
            _mockRepositoryDeclineReason.Setup(mrt => mrt.QueryEager()).Returns(DeclineReasons);
            _mockMapper.Setup(mm => mm.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>())).Returns(readedDeclineReasonList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryDeclineReason.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that list named returns a value")]
        public void GivenListNamed_WhenRegularCall_ReturnsValue()
        {
            var DeclineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedDeclineReasonList = new List<ReadedDeclineReasonContract> { new ReadedDeclineReasonContract { Id = 1 } };
            _mockRepositoryDeclineReason.Setup(mrt => mrt.QueryEager()).Returns(DeclineReasons);
            _mockMapper.Setup(mm => mm.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>())).Returns(readedDeclineReasonList);

            var actualResult = _service.ListNamed();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryDeclineReason.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var DeclineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedDeclineReason = new ReadedDeclineReasonContract { Id = 1, Name = "Name" };
            _mockRepositoryDeclineReason.Setup(mrt => mrt.QueryEager()).Returns(DeclineReasons);
            _mockMapper.Setup(mm => mm.Map<ReadedDeclineReasonContract>(It.IsAny<DeclineReason>())).Returns(readedDeclineReason);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            _mockRepositoryDeclineReason.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedDeclineReasonContract>(It.IsAny<DeclineReason>()), Times.Once);
        }
    }
}