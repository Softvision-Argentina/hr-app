// <copyright file="ReservationServiceTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Contracts.Reservation;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.Reservation;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class ReservationServiceTest : BaseDomainTest
    {
        private readonly ReservationService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Reservation>> mockRepositoryReservation;
        private readonly Mock<IRepository<User>> mockRepositoryUser;
        private readonly Mock<IRepository<Room>> mockRepositoryRoom;
        private readonly Mock<ILog<ReservationService>> mockLogReservationService;
        private readonly Mock<IGoogleCalendarService> mockCalendarService;
        private readonly Mock<UpdateReservationContractValidator> mockUpdateReservationContractValidator;
        private readonly Mock<CreateReservationContractValidator> mockCreateReservationContractValidator;

        public ReservationServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryReservation = new Mock<IRepository<Reservation>>();
            this.mockRepositoryUser = new Mock<IRepository<User>>();
            this.mockRepositoryRoom = new Mock<IRepository<Room>>();
            this.mockLogReservationService = new Mock<ILog<ReservationService>>();
            this.mockCalendarService = new Mock<IGoogleCalendarService>();
            this.mockUpdateReservationContractValidator = new Mock<UpdateReservationContractValidator>();
            this.mockCreateReservationContractValidator = new Mock<CreateReservationContractValidator>();
            this.service = new ReservationService(
                this.mockMapper.Object,
                this.mockRepositoryReservation.Object,
                this.mockRepositoryUser.Object,
                this.mockRepositoryRoom.Object,
                this.MockUnitOfWork.Object,
                this.mockLogReservationService.Object,
                this.mockCalendarService.Object,
                this.mockUpdateReservationContractValidator.Object,
                this.mockCreateReservationContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create ReservationService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateReservationService()
        {
            var contract = new CreateReservationContract();
            var expectedReservation = new CreatedReservationContract();
            this.mockCreateReservationContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateReservationContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Reservation>(It.IsAny<CreateReservationContract>())).Returns(new Reservation());
            this.mockRepositoryReservation.Setup(repoCom => repoCom.Create(It.IsAny<Reservation>())).Returns(new Reservation());
            this.mockMapper.Setup(mm => mm.Map<CreatedReservationContract>(It.IsAny<Reservation>())).Returns(expectedReservation);

            var createdReservation = this.service.Create(contract);

            Assert.NotNull(createdReservation);
            Assert.Equal(expectedReservation, createdReservation);
            this.mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateReservationContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateReservationContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Reservation>(It.IsAny<CreateReservationContract>()), Times.Once);
            this.mockRepositoryReservation.Verify(mrt => mrt.Create(It.IsAny<Reservation>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedReservationContract>(It.IsAny<Reservation>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateReservationContract();
            var expectedReservation = new CreatedReservationContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateReservationContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateReservationContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedReservationContract>(It.IsAny<Reservation>())).Returns(expectedReservation);

            var exception = Assert.Throws<Model.Exceptions.Reservation.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateReservationContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateReservationContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Reservation>(It.IsAny<CreateReservationContract>()), Times.Never);
            this.mockRepositoryReservation.Verify(mrt => mrt.Create(It.IsAny<Reservation>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedReservationContract>(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete ReservationService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteReservationService()
        {
            var reservations = new List<Reservation>() { new Reservation() { Id = 1 } }.AsQueryable();
            this.mockRepositoryReservation.Setup(mrt => mrt.Query()).Returns(reservations);

            this.service.Delete(1);

            this.mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryReservation.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryReservation.Verify(mrt => mrt.Delete(It.IsAny<Reservation>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteReservationNotFoundException()
        {
            var expectedErrorMEssage = $"Reservation not found for the ReservationId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Reservation.DeleteReservationNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryReservation.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryReservation.Verify(mrt => mrt.Delete(It.IsAny<Reservation>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update ReservationService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var reservations = new List<Reservation> { new Reservation { Id = 1, SinceReservation = DateTime.Now, UntilReservation = DateTime.Now, Room = new Room() } }.AsQueryable();
            this.mockRepositoryReservation.Setup(x => x.Query()).Returns(reservations);
            var contract = new UpdateReservationContract { Id = 1 };
            this.mockUpdateReservationContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateReservationContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Reservation>(It.IsAny<UpdateReservationContract>())).Returns(new Reservation());

            this.service.Update(contract);

            this.mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateReservationContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateReservationContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Reservation>(It.IsAny<UpdateReservationContract>()), Times.Once);
            this.mockRepositoryReservation.Verify(mrt => mrt.Update(It.IsAny<Reservation>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var reservations = new List<Reservation> { new Reservation { Id = 1, SinceReservation = DateTime.Now, UntilReservation = DateTime.Now, Room = new Room() } }.AsQueryable();
            this.mockRepositoryReservation.Setup(x => x.Query()).Returns(reservations);
            var contract = new UpdateReservationContract { Id = 1 };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateReservationContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateReservationContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<Reservation>(It.IsAny<UpdateReservationContract>())).Returns(new Reservation());

            var exception = Assert.Throws<Model.Exceptions.Reservation.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateReservationContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateReservationContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Reservation>(It.IsAny<UpdateReservationContract>()), Times.Never);
            this.mockRepositoryReservation.Verify(mrt => mrt.Update(It.IsAny<Reservation>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var reservations = new List<Reservation>() { new Reservation() { Id = 1 } }.AsQueryable();
            var readedReservationList = new List<ReadedReservationContract> { new ReadedReservationContract { Id = 1 } };
            this.mockRepositoryReservation.Setup(mrt => mrt.Query()).Returns(reservations);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedReservationContract>>(It.IsAny<List<Reservation>>())).Returns(readedReservationList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryReservation.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedReservationContract>>(It.IsAny<List<Reservation>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var reservations = new List<Reservation>() { new Reservation() { Id = 1 } }.AsQueryable();
            var readedReservation = new ReadedReservationContract { Id = 1 };
            this.mockRepositoryReservation.Setup(mrt => mrt.Query()).Returns(reservations);
            this.mockMapper.Setup(mm => mm.Map<ReadedReservationContract>(It.IsAny<Reservation>())).Returns(readedReservation);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            this.mockRepositoryReservation.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedReservationContract>(It.IsAny<Reservation>()), Times.Once);
        }
    }
}