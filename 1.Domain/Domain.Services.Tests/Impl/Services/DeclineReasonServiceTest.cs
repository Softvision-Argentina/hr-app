using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions;
using Domain.Model.Exceptions.Skill;
using Domain.Services.Contracts;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class DeclineReasonServiceTest : BaseDomainTest
    {
        private readonly DeclineReasonService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<DeclineReason>> mockRepositoryDeclineReason;
        private readonly Mock<IRepository<CandidateProfile>> mockRepositoryCandidateProfile;
        private readonly Mock<IRepository<Model.DeclineReason>> mockRepositoryModelDeclineReason;
        private readonly Mock<ILog<DeclineReasonService>> mockLogDeclineReasonService;
        private readonly Mock<UpdateDeclineReasonContractValidator> mockUpdateDeclineReasonContractValidator;
        private readonly Mock<CreateDeclineReasonContractValidator> mockCreateDeclineReasonContractValidator;

        public DeclineReasonServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryDeclineReason = new Mock<IRepository<DeclineReason>>();
            mockRepositoryCandidateProfile = new Mock<IRepository<CandidateProfile>>();
            mockRepositoryModelDeclineReason = new Mock<IRepository<Model.DeclineReason>>();
            mockLogDeclineReasonService = new Mock<ILog<DeclineReasonService>>();
            mockUpdateDeclineReasonContractValidator = new Mock<UpdateDeclineReasonContractValidator>();
            mockCreateDeclineReasonContractValidator = new Mock<CreateDeclineReasonContractValidator>();
            service = new DeclineReasonService(
                mockMapper.Object,
                mockRepositoryDeclineReason.Object,                
                MockUnitOfWork.Object,
                mockLogDeclineReasonService.Object,
                mockUpdateDeclineReasonContractValidator.Object,
                mockCreateDeclineReasonContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create DeclineReasonService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateDeclineReasonService()
        {
            var contract = new CreateDeclineReasonContract();
            var expectedDeclineReason = new CreatedDeclineReasonContract();
            mockCreateDeclineReasonContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<DeclineReason>(It.IsAny<CreateDeclineReasonContract>())).Returns(new DeclineReason());
            mockRepositoryDeclineReason.Setup(repoCom => repoCom.Create(It.IsAny<DeclineReason>())).Returns(new DeclineReason());
            mockMapper.Setup(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>())).Returns(expectedDeclineReason);

            var createdDeclineReason = service.Create(contract);

            Assert.NotNull(createdDeclineReason);
            Assert.Equal(expectedDeclineReason, createdDeclineReason);
            mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateDeclineReasonContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<CreateDeclineReasonContract>()), Times.Once);
            mockRepositoryDeclineReason.Verify(mrt => mrt.Create(It.IsAny<DeclineReason>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateDeclineReasonContract();
            var expectedDeclineReason = new CreatedDeclineReasonContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateDeclineReasonContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>())).Returns(expectedDeclineReason);

            var exception = Assert.Throws<CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateDeclineReasonContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDeclineReasonContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<CreateDeclineReasonContract>()), Times.Never);
            mockRepositoryDeclineReason.Verify(mrt => mrt.Create(It.IsAny<DeclineReason>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            mockMapper.Verify(mm => mm.Map<CreatedDeclineReasonContract>(It.IsAny<DeclineReason>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete DeclineReasonService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteDeclineReasonService()
        {
            var DeclineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1 } }.AsQueryable();
            mockRepositoryDeclineReason.Setup(mrt => mrt.Query()).Returns(DeclineReasons);

            service.Delete(1);

            mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryDeclineReason.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryDeclineReason.Verify(mrt => mrt.Delete(It.IsAny<DeclineReason>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteDeclineReasonNotFoundException()
        {
            var expectedErrorMEssage = $"Skill not found for the skillId: {0}";

            var exception = Assert.Throws<DeleteDeclineReasonNotFoundException>(() => service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositoryDeclineReason.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryDeclineReason.Verify(mrt => mrt.Delete(It.IsAny<DeclineReason>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update DeclineReasonService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateDeclineReasonContract();
            mockUpdateDeclineReasonContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>())).Returns(new DeclineReason());

            service.Update(contract);

            mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateDeclineReasonContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>()), Times.Once);
            mockRepositoryDeclineReason.Verify(mrt => mrt.Update(It.IsAny<DeclineReason>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateDeclineReasonContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateDeclineReasonContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>())).Returns(new DeclineReason());

            var exception = Assert.Throws<CreateContractInvalidException>(() => service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogDeclineReasonService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateDeclineReasonContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDeclineReasonContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<DeclineReason>(It.IsAny<UpdateDeclineReasonContract>()), Times.Never);
            mockRepositoryDeclineReason.Verify(mrt => mrt.Update(It.IsAny<DeclineReason>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var DeclineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1 } }.AsQueryable();
            var readedDeclineReasonList = new List<ReadedDeclineReasonContract> { new ReadedDeclineReasonContract { Id = 1 } };
            mockRepositoryDeclineReason.Setup(mrt => mrt.QueryEager()).Returns(DeclineReasons);
            mockMapper.Setup(mm => mm.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>())).Returns(readedDeclineReasonList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryDeclineReason.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that list named returns a value")]
        public void GivenListNamed_WhenRegularCall_ReturnsValue()
        {
            var DeclineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedDeclineReasonList = new List<ReadedDeclineReasonContract> { new ReadedDeclineReasonContract { Id = 1 } };
            mockRepositoryDeclineReason.Setup(mrt => mrt.QueryEager()).Returns(DeclineReasons);
            mockMapper.Setup(mm => mm.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>())).Returns(readedDeclineReasonList);

            var actualResult = service.ListNamed();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryDeclineReason.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedDeclineReasonContract>>(It.IsAny<List<DeclineReason>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var DeclineReasons = new List<DeclineReason>() { new DeclineReason() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedDeclineReason = new ReadedDeclineReasonContract { Id = 1, Name = "Name" };
            mockRepositoryDeclineReason.Setup(mrt => mrt.QueryEager()).Returns(DeclineReasons);
            mockMapper.Setup(mm => mm.Map<ReadedDeclineReasonContract>(It.IsAny<DeclineReason>())).Returns(readedDeclineReason);

            var actualResult = service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            mockRepositoryDeclineReason.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedDeclineReasonContract>(It.IsAny<DeclineReason>()), Times.Once);
        }
    }
}