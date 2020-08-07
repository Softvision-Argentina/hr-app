// <copyright file="PreOfferServiceTest.cs" company="Softvision">
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
    using Domain.Services.Contracts.PreOffer;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.PreOffer;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class PreOfferServiceTest : BaseDomainTest
    {
        private readonly PreOfferService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<PreOffer>> mockRepositoryPreOffer;
        private readonly Mock<ILog<PreOfferService>> mockLogPreOfferService;
        private readonly Mock<UpdatePreOfferContractValidator> mockUpdatePreOfferContractValidator;
        private readonly Mock<CreatePreOfferContractValidator> mockCreatePreOfferContractValidator;

        public PreOfferServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryPreOffer = new Mock<IRepository<PreOffer>>();
            this.mockLogPreOfferService = new Mock<ILog<PreOfferService>>();
            this.mockUpdatePreOfferContractValidator = new Mock<UpdatePreOfferContractValidator>();
            this.mockCreatePreOfferContractValidator = new Mock<CreatePreOfferContractValidator>();
            this.service = new PreOfferService(
                this.mockMapper.Object,
                this.mockRepositoryPreOffer.Object,
                this.MockUnitOfWork.Object,
                this.mockLogPreOfferService.Object,
                this.mockUpdatePreOfferContractValidator.Object,
                this.mockCreatePreOfferContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create PreOfferService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreatePreOfferService()
        {
            var contract = new CreatePreOfferContract();
            var expectedPreOffer = new CreatedPreOfferContract();
            this.mockCreatePreOfferContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreatePreOfferContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<PreOffer>(It.IsAny<CreatePreOfferContract>())).Returns(new PreOffer());
            this.mockRepositoryPreOffer.Setup(repoCom => repoCom.Create(It.IsAny<PreOffer>())).Returns(new PreOffer());
            this.mockMapper.Setup(mm => mm.Map<CreatedPreOfferContract>(It.IsAny<PreOffer>())).Returns(expectedPreOffer);

            var createdPreOffer = this.service.Create(contract);

            Assert.NotNull(createdPreOffer);
            Assert.Equal(expectedPreOffer, createdPreOffer);
            this.mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreatePreOfferContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreatePreOfferContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<PreOffer>(It.IsAny<CreatePreOfferContract>()), Times.Once);
            this.mockRepositoryPreOffer.Verify(mrt => mrt.Create(It.IsAny<PreOffer>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedPreOfferContract>(It.IsAny<PreOffer>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreatePreOfferContract();
            var expectedPreOffer = new CreatedPreOfferContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreatePreOfferContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreatePreOfferContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedPreOfferContract>(It.IsAny<PreOffer>())).Returns(expectedPreOffer);

            var exception = Assert.Throws<Model.Exceptions.PreOffer.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreatePreOfferContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreatePreOfferContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<PreOffer>(It.IsAny<CreatePreOfferContract>()), Times.Never);
            this.mockRepositoryPreOffer.Verify(mrt => mrt.Create(It.IsAny<PreOffer>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedPreOfferContract>(It.IsAny<PreOffer>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete PreOfferService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeletePreOfferService()
        {
            var preOffers = new List<PreOffer>() { new PreOffer() { Id = 1 } }.AsQueryable();
            this.mockRepositoryPreOffer.Setup(mrt => mrt.Query()).Returns(preOffers);

            this.service.Delete(1);

            this.mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryPreOffer.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryPreOffer.Verify(mrt => mrt.Delete(It.IsAny<PreOffer>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeletePreOfferNotFoundException()
        {
            var expectedErrorMEssage = $"PreOffer not found for the preOfferId: {0}";

            var exception = Assert.Throws<Model.Exceptions.PreOffer.DeletePreOfferNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryPreOffer.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryPreOffer.Verify(mrt => mrt.Delete(It.IsAny<PreOffer>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update PreOfferService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdatePreOfferContract();
            this.mockUpdatePreOfferContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdatePreOfferContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<PreOffer>(It.IsAny<UpdatePreOfferContract>())).Returns(new PreOffer());

            this.service.Update(contract);

            this.mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdatePreOfferContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdatePreOfferContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<PreOffer>(It.IsAny<UpdatePreOfferContract>()), Times.Once);
            this.mockRepositoryPreOffer.Verify(mrt => mrt.Update(It.IsAny<PreOffer>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdatePreOfferContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdatePreOfferContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdatePreOfferContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<PreOffer>(It.IsAny<UpdatePreOfferContract>())).Returns(new PreOffer());

            var exception = Assert.Throws<Model.Exceptions.PreOffer.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogPreOfferService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdatePreOfferContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdatePreOfferContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<PreOffer>(It.IsAny<UpdatePreOfferContract>()), Times.Never);
            this.mockRepositoryPreOffer.Verify(mrt => mrt.Update(It.IsAny<PreOffer>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var preOffers = new List<PreOffer>() { new PreOffer() { Id = 1 } }.AsQueryable();
            var readedPreOfferList = new List<ReadedPreOfferContract> { new ReadedPreOfferContract { Id = 1 } };
            this.mockRepositoryPreOffer.Setup(mrt => mrt.QueryEager()).Returns(preOffers);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedPreOfferContract>>(It.IsAny<List<PreOffer>>())).Returns(readedPreOfferList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryPreOffer.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedPreOfferContract>>(It.IsAny<List<PreOffer>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var preOffers = new List<PreOffer>() { new PreOffer() { Id = 1 } }.AsQueryable();
            var readedPreOffer = new ReadedPreOfferContract { Id = 1 };
            this.mockRepositoryPreOffer.Setup(mrt => mrt.QueryEager()).Returns(preOffers);
            this.mockMapper.Setup(mm => mm.Map<ReadedPreOfferContract>(It.IsAny<PreOffer>())).Returns(readedPreOffer);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedPreOffer, actualResult);
            this.mockRepositoryPreOffer.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedPreOfferContract>(It.IsAny<PreOffer>()), Times.Once);
        }
    }
}