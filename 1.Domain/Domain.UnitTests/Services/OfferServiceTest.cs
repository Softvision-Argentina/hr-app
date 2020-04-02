using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Offer;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.Offer;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class OfferServiceTest : BaseDomainTest
    {
        private readonly OfferService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Offer>> mockRepositoryOffer;
        private readonly Mock<IRepository<Model.Offer>> mockRepositoryModelOffer;
        private readonly Mock<ILog<OfferService>> mockLogOfferService;
        private readonly Mock<UpdateOfferContractValidator> mockUpdateOfferContractValidator;
        private readonly Mock<CreateOfferContractValidator> mockCreateOfferContractValidator;

        public OfferServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryOffer = new Mock<IRepository<Offer>>();
            mockRepositoryModelOffer = new Mock<IRepository<Model.Offer>>();
            mockLogOfferService = new Mock<ILog<OfferService>>();
            mockUpdateOfferContractValidator = new Mock<UpdateOfferContractValidator>();
            mockCreateOfferContractValidator = new Mock<CreateOfferContractValidator>();
            service = new OfferService(
                mockMapper.Object,
                mockRepositoryOffer.Object,
                MockUnitOfWork.Object,
                mockLogOfferService.Object,
                mockUpdateOfferContractValidator.Object,
                mockCreateOfferContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create OfferService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateOfferService()
        {
            var contract = new CreateOfferContract();
            var expectedOffer = new CreatedOfferContract();
            mockCreateOfferContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfferContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Offer>(It.IsAny<CreateOfferContract>())).Returns(new Offer());
            mockRepositoryOffer.Setup(repoCom => repoCom.Create(It.IsAny<Offer>())).Returns(new Offer());
            mockMapper.Setup(mm => mm.Map<CreatedOfferContract>(It.IsAny<Offer>())).Returns(expectedOffer);

            var createdOffer = service.Create(contract);

            Assert.NotNull(createdOffer);
            Assert.Equal(expectedOffer, createdOffer);
            mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateOfferContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfferContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Offer>(It.IsAny<CreateOfferContract>()), Times.Once);
            mockRepositoryOffer.Verify(mrt => mrt.Create(It.IsAny<Offer>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedOfferContract>(It.IsAny<Offer>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateOfferContract();
            var expectedOffer = new CreatedOfferContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateOfferContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfferContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CreatedOfferContract>(It.IsAny<Offer>())).Returns(expectedOffer);

            var exception = Assert.Throws<Model.Exceptions.Offer.CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateOfferContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfferContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Offer>(It.IsAny<CreateOfferContract>()), Times.Never);
            mockRepositoryOffer.Verify(mrt => mrt.Create(It.IsAny<Offer>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            mockMapper.Verify(mm => mm.Map<CreatedOfferContract>(It.IsAny<Offer>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete OfferService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteOfferService()
        {
            var Offers = new List<Offer>() { new Offer() { Id = 1 } }.AsQueryable();
            mockRepositoryOffer.Setup(mrt => mrt.Query()).Returns(Offers);

            service.Delete(1);

            mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryOffer.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryOffer.Verify(mrt => mrt.Delete(It.IsAny<Offer>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteOfferNotFoundException()
        {
            var expectedErrorMEssage = $"Offer not found for the offerId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Offer.DeleteOfferNotFoundException>(() => service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositoryOffer.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryOffer.Verify(mrt => mrt.Delete(It.IsAny<Offer>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update OfferService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateOfferContract();
            mockUpdateOfferContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfferContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Offer>(It.IsAny<UpdateOfferContract>())).Returns(new Offer());

            service.Update(contract);

            mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateOfferContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfferContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Offer>(It.IsAny<UpdateOfferContract>()), Times.Once);
            mockRepositoryOffer.Verify(mrt => mrt.Update(It.IsAny<Offer>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateOfferContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateOfferContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfferContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<Offer>(It.IsAny<UpdateOfferContract>())).Returns(new Offer());

            var exception = Assert.Throws<Model.Exceptions.Offer.CreateContractInvalidException>(() => service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateOfferContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfferContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Offer>(It.IsAny<UpdateOfferContract>()), Times.Never);
            mockRepositoryOffer.Verify(mrt => mrt.Update(It.IsAny<Offer>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Offers = new List<Offer>() { new Offer() { Id = 1 } }.AsQueryable();
            var readedOfferList = new List<ReadedOfferContract> { new ReadedOfferContract { Id = 1 } };
            mockRepositoryOffer.Setup(mrt => mrt.QueryEager()).Returns(Offers);
            mockMapper.Setup(mm => mm.Map<List<ReadedOfferContract>>(It.IsAny<List<Offer>>())).Returns(readedOfferList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryOffer.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedOfferContract>>(It.IsAny<List<Offer>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Offers = new List<Offer>() { new Offer() { Id = 1 } }.AsQueryable();
            var readedOffer = new ReadedOfferContract { Id = 1 };
            mockRepositoryOffer.Setup(mrt => mrt.QueryEager()).Returns(Offers);
            mockMapper.Setup(mm => mm.Map<ReadedOfferContract>(It.IsAny<Offer>())).Returns(readedOffer);

            var actualResult = service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedOffer, actualResult);
            mockRepositoryOffer.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedOfferContract>(It.IsAny<Offer>()), Times.Once);
        }
    }
}