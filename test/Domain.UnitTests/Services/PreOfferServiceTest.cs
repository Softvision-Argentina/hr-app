using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.PreOffer;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.PreOffer;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class PreOfferServiceTest : BaseDomainTest
    {
        private readonly PreOfferService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<PreOffer>> _mockRepositoryPreOffer;        
        private readonly Mock<ILog<PreOfferService>> _mockLogPreOfferService;
        private readonly Mock<UpdatePreOfferContractValidator> _mockUpdatePreOfferContractValidator;
        private readonly Mock<CreatePreOfferContractValidator> _mockCreatePreOfferContractValidator;

        public PreOfferServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryPreOffer = new Mock<IRepository<PreOffer>>();            
            _mockLogPreOfferService = new Mock<ILog<PreOfferService>>();
            _mockUpdatePreOfferContractValidator = new Mock<UpdatePreOfferContractValidator>();
            _mockCreatePreOfferContractValidator = new Mock<CreatePreOfferContractValidator>();
            _service = new PreOfferService(
                _mockMapper.Object,
                _mockRepositoryPreOffer.Object,
                MockUnitOfWork.Object,
                _mockLogPreOfferService.Object,
                _mockUpdatePreOfferContractValidator.Object,
                _mockCreatePreOfferContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create PreOfferService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreatePreOfferService()
        {
            var contract = new CreatePreOfferContract();
            var expectedPreOffer = new CreatedPreOfferContract();
            _mockCreatePreOfferContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreatePreOfferContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<PreOffer>(It.IsAny<CreatePreOfferContract>())).Returns(new PreOffer());
            _mockRepositoryPreOffer.Setup(repoCom => repoCom.Create(It.IsAny<PreOffer>())).Returns(new PreOffer());
            _mockMapper.Setup(mm => mm.Map<CreatedPreOfferContract>(It.IsAny<PreOffer>())).Returns(expectedPreOffer);

            var createdPreOffer = _service.Create(contract);

            Assert.NotNull(createdPreOffer);
            Assert.Equal(expectedPreOffer, createdPreOffer);
            _mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreatePreOfferContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreatePreOfferContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<PreOffer>(It.IsAny<CreatePreOfferContract>()), Times.Once);
            _mockRepositoryPreOffer.Verify(mrt => mrt.Create(It.IsAny<PreOffer>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedPreOfferContract>(It.IsAny<PreOffer>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreatePreOfferContract();
            var expectedPreOffer = new CreatedPreOfferContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreatePreOfferContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreatePreOfferContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedPreOfferContract>(It.IsAny<PreOffer>())).Returns(expectedPreOffer);

            var exception = Assert.Throws<Model.Exceptions.PreOffer.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreatePreOfferContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreatePreOfferContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<PreOffer>(It.IsAny<CreatePreOfferContract>()), Times.Never);
            _mockRepositoryPreOffer.Verify(mrt => mrt.Create(It.IsAny<PreOffer>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedPreOfferContract>(It.IsAny<PreOffer>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete PreOfferService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeletePreOfferService()
        {
            var preOffers = new List<PreOffer>() { new PreOffer() { Id = 1 } }.AsQueryable();
            _mockRepositoryPreOffer.Setup(mrt => mrt.Query()).Returns(preOffers);

            _service.Delete(1);

            _mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryPreOffer.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryPreOffer.Verify(mrt => mrt.Delete(It.IsAny<PreOffer>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeletePreOfferNotFoundException()
        {
            var expectedErrorMEssage = $"PreOffer not found for the preOfferId: {0}";

            var exception = Assert.Throws<Model.Exceptions.PreOffer.DeletePreOfferNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryPreOffer.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryPreOffer.Verify(mrt => mrt.Delete(It.IsAny<PreOffer>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update PreOfferService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdatePreOfferContract();
            _mockUpdatePreOfferContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdatePreOfferContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<PreOffer>(It.IsAny<UpdatePreOfferContract>())).Returns(new PreOffer());

            _service.Update(contract);

            _mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdatePreOfferContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdatePreOfferContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<PreOffer>(It.IsAny<UpdatePreOfferContract>()), Times.Once);
            _mockRepositoryPreOffer.Verify(mrt => mrt.Update(It.IsAny<PreOffer>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdatePreOfferContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdatePreOfferContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdatePreOfferContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<PreOffer>(It.IsAny<UpdatePreOfferContract>())).Returns(new PreOffer());

            var exception = Assert.Throws<Model.Exceptions.PreOffer.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdatePreOfferContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdatePreOfferContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<PreOffer>(It.IsAny<UpdatePreOfferContract>()), Times.Never);
            _mockRepositoryPreOffer.Verify(mrt => mrt.Update(It.IsAny<PreOffer>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var preOffers = new List<PreOffer>() { new PreOffer() { Id = 1 } }.AsQueryable();
            var readedPreOfferList = new List<ReadedPreOfferContract> { new ReadedPreOfferContract { Id = 1 } };
            _mockRepositoryPreOffer.Setup(mrt => mrt.QueryEager()).Returns(preOffers);
            _mockMapper.Setup(mm => mm.Map<List<ReadedPreOfferContract>>(It.IsAny<List<PreOffer>>())).Returns(readedPreOfferList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryPreOffer.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedPreOfferContract>>(It.IsAny<List<PreOffer>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var preOffers = new List<PreOffer>() { new PreOffer() { Id = 1 } }.AsQueryable();
            var readedPreOffer = new ReadedPreOfferContract { Id = 1 };
            _mockRepositoryPreOffer.Setup(mrt => mrt.QueryEager()).Returns(preOffers);
            _mockMapper.Setup(mm => mm.Map<ReadedPreOfferContract>(It.IsAny<PreOffer>())).Returns(readedPreOffer);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedPreOffer, actualResult);
            _mockRepositoryPreOffer.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedPreOfferContract>(It.IsAny<PreOffer>()), Times.Once);
        }
    }
}