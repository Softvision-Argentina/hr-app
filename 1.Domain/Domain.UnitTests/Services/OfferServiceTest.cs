using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Offer;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.Offer;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class OfferServiceTest : BaseDomainTest
    {
        private readonly OfferService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Offer>> _mockRepositoryOffer;        
        private readonly Mock<ILog<OfferService>> _mockLogOfferService;
        private readonly Mock<UpdateOfferContractValidator> _mockUpdateOfferContractValidator;
        private readonly Mock<CreateOfferContractValidator> _mockCreateOfferContractValidator;

        public OfferServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryOffer = new Mock<IRepository<Offer>>();            
            _mockLogOfferService = new Mock<ILog<OfferService>>();
            _mockUpdateOfferContractValidator = new Mock<UpdateOfferContractValidator>();
            _mockCreateOfferContractValidator = new Mock<CreateOfferContractValidator>();
            _service = new OfferService(
                _mockMapper.Object,
                _mockRepositoryOffer.Object,
                MockUnitOfWork.Object,
                _mockLogOfferService.Object,
                _mockUpdateOfferContractValidator.Object,
                _mockCreateOfferContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create OfferService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateOfferService()
        {
            var contract = new CreateOfferContract();
            var expectedOffer = new CreatedOfferContract();
            _mockCreateOfferContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfferContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Offer>(It.IsAny<CreateOfferContract>())).Returns(new Offer());
            _mockRepositoryOffer.Setup(repoCom => repoCom.Create(It.IsAny<Offer>())).Returns(new Offer());
            _mockMapper.Setup(mm => mm.Map<CreatedOfferContract>(It.IsAny<Offer>())).Returns(expectedOffer);

            var createdOffer = _service.Create(contract);

            Assert.NotNull(createdOffer);
            Assert.Equal(expectedOffer, createdOffer);
            _mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateOfferContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfferContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Offer>(It.IsAny<CreateOfferContract>()), Times.Once);
            _mockRepositoryOffer.Verify(mrt => mrt.Create(It.IsAny<Offer>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedOfferContract>(It.IsAny<Offer>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateOfferContract();
            var expectedOffer = new CreatedOfferContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateOfferContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfferContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedOfferContract>(It.IsAny<Offer>())).Returns(expectedOffer);

            var exception = Assert.Throws<Model.Exceptions.Offer.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateOfferContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfferContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Offer>(It.IsAny<CreateOfferContract>()), Times.Never);
            _mockRepositoryOffer.Verify(mrt => mrt.Create(It.IsAny<Offer>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedOfferContract>(It.IsAny<Offer>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete OfferService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteOfferService()
        {
            var Offers = new List<Offer>() { new Offer() { Id = 1 } }.AsQueryable();
            _mockRepositoryOffer.Setup(mrt => mrt.Query()).Returns(Offers);

            _service.Delete(1);

            _mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryOffer.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryOffer.Verify(mrt => mrt.Delete(It.IsAny<Offer>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteOfferNotFoundException()
        {
            var expectedErrorMEssage = $"Offer not found for the offerId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Offer.DeleteOfferNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryOffer.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryOffer.Verify(mrt => mrt.Delete(It.IsAny<Offer>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update OfferService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateOfferContract();
            _mockUpdateOfferContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfferContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Offer>(It.IsAny<UpdateOfferContract>())).Returns(new Offer());

            _service.Update(contract);

            _mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateOfferContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfferContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Offer>(It.IsAny<UpdateOfferContract>()), Times.Once);
            _mockRepositoryOffer.Verify(mrt => mrt.Update(It.IsAny<Offer>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateOfferContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateOfferContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfferContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<Offer>(It.IsAny<UpdateOfferContract>())).Returns(new Offer());

            var exception = Assert.Throws<Model.Exceptions.Offer.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateOfferContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfferContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Offer>(It.IsAny<UpdateOfferContract>()), Times.Never);
            _mockRepositoryOffer.Verify(mrt => mrt.Update(It.IsAny<Offer>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Offers = new List<Offer>() { new Offer() { Id = 1 } }.AsQueryable();
            var readedOfferList = new List<ReadedOfferContract> { new ReadedOfferContract { Id = 1 } };
            _mockRepositoryOffer.Setup(mrt => mrt.QueryEager()).Returns(Offers);
            _mockMapper.Setup(mm => mm.Map<List<ReadedOfferContract>>(It.IsAny<List<Offer>>())).Returns(readedOfferList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryOffer.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedOfferContract>>(It.IsAny<List<Offer>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Offers = new List<Offer>() { new Offer() { Id = 1 } }.AsQueryable();
            var readedOffer = new ReadedOfferContract { Id = 1 };
            _mockRepositoryOffer.Setup(mrt => mrt.QueryEager()).Returns(Offers);
            _mockMapper.Setup(mm => mm.Map<ReadedOfferContract>(It.IsAny<Offer>())).Returns(readedOffer);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedOffer, actualResult);
            _mockRepositoryOffer.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedOfferContract>(It.IsAny<Offer>()), Times.Once);
        }
    }
}