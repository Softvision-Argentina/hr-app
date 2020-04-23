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
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class ReservationServiceTest : BaseDomainTest
    {
        private readonly ReservationService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Reservation>> _mockRepositoryReservation;
        private readonly Mock<IRepository<User>> _mockRepositoryUser;
        private readonly Mock<IRepository<Room>> _mockRepositoryRoom;
        private readonly Mock<ILog<ReservationService>> _mockLogReservationService;
        private readonly Mock<IGoogleCalendarService> _mockCalendarService;
        private readonly Mock<UpdateReservationContractValidator> _mockUpdateReservationContractValidator;
        private readonly Mock<CreateReservationContractValidator> _mockCreateReservationContractValidator;

        public ReservationServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryReservation = new Mock<IRepository<Reservation>>();
            _mockRepositoryUser = new Mock<IRepository<User>>();
            _mockRepositoryRoom = new Mock<IRepository<Room>>();
            _mockLogReservationService = new Mock<ILog<ReservationService>>();
            _mockCalendarService = new Mock<IGoogleCalendarService>();
            _mockUpdateReservationContractValidator = new Mock<UpdateReservationContractValidator>();
            _mockCreateReservationContractValidator = new Mock<CreateReservationContractValidator>();
            _service = new ReservationService(
                _mockMapper.Object,
                _mockRepositoryReservation.Object,
                _mockRepositoryUser.Object,
                _mockRepositoryRoom.Object,
                MockUnitOfWork.Object,
                _mockLogReservationService.Object,
                _mockCalendarService.Object,
                _mockUpdateReservationContractValidator.Object,
                _mockCreateReservationContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create ReservationService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateReservationService()
        {
            var contract = new CreateReservationContract();
            var expectedReservation = new CreatedReservationContract();
            _mockCreateReservationContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateReservationContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Reservation>(It.IsAny<CreateReservationContract>())).Returns(new Reservation());
            _mockRepositoryReservation.Setup(repoCom => repoCom.Create(It.IsAny<Reservation>())).Returns(new Reservation());
            _mockMapper.Setup(mm => mm.Map<CreatedReservationContract>(It.IsAny<Reservation>())).Returns(expectedReservation);

            var createdReservation = _service.Create(contract);

            Assert.NotNull(createdReservation);
            Assert.Equal(expectedReservation, createdReservation);
            _mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateReservationContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateReservationContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Reservation>(It.IsAny<CreateReservationContract>()), Times.Once);
            _mockRepositoryReservation.Verify(mrt => mrt.Create(It.IsAny<Reservation>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedReservationContract>(It.IsAny<Reservation>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateReservationContract();
            var expectedReservation = new CreatedReservationContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateReservationContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateReservationContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedReservationContract>(It.IsAny<Reservation>())).Returns(expectedReservation);

            var exception = Assert.Throws<Model.Exceptions.Reservation.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateReservationContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateReservationContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Reservation>(It.IsAny<CreateReservationContract>()), Times.Never);
            _mockRepositoryReservation.Verify(mrt => mrt.Create(It.IsAny<Reservation>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedReservationContract>(It.IsAny<Reservation>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete ReservationService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteReservationService()
        {
            var Reservations = new List<Reservation>() { new Reservation() { Id = 1 } }.AsQueryable();
            _mockRepositoryReservation.Setup(mrt => mrt.Query()).Returns(Reservations);

            _service.Delete(1);

            _mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryReservation.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryReservation.Verify(mrt => mrt.Delete(It.IsAny<Reservation>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteReservationNotFoundException()
        {
            var expectedErrorMEssage = $"Reservation not found for the ReservationId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Reservation.DeleteReservationNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryReservation.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryReservation.Verify(mrt => mrt.Delete(It.IsAny<Reservation>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update ReservationService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var reservations = new List<Reservation> { new Reservation { Id= 1, SinceReservation = DateTime.Now, UntilReservation = DateTime.Now, Room = new Room() } }.AsQueryable();
            _mockRepositoryReservation.Setup(x => x.Query()).Returns(reservations);
            var contract = new UpdateReservationContract { Id = 1};
            _mockUpdateReservationContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateReservationContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Reservation>(It.IsAny<UpdateReservationContract>())).Returns(new Reservation ());

            _service.Update(contract);

            _mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateReservationContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateReservationContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Reservation>(It.IsAny<UpdateReservationContract>()), Times.Once);
            _mockRepositoryReservation.Verify(mrt => mrt.Update(It.IsAny<Reservation>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var reservations = new List<Reservation> { new Reservation { Id = 1, SinceReservation = DateTime.Now, UntilReservation = DateTime.Now, Room = new Room() } }.AsQueryable();
            _mockRepositoryReservation.Setup(x => x.Query()).Returns(reservations);
            var contract = new UpdateReservationContract { Id = 1 };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateReservationContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateReservationContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<Reservation>(It.IsAny<UpdateReservationContract>())).Returns(new Reservation());

            var exception = Assert.Throws<Model.Exceptions.Reservation.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogReservationService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateReservationContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateReservationContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Reservation>(It.IsAny<UpdateReservationContract>()), Times.Never);
            _mockRepositoryReservation.Verify(mrt => mrt.Update(It.IsAny<Reservation>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Reservations = new List<Reservation>() { new Reservation() { Id = 1 } }.AsQueryable();
            var readedReservationList = new List<ReadedReservationContract> { new ReadedReservationContract { Id = 1 } };
            _mockRepositoryReservation.Setup(mrt => mrt.Query()).Returns(Reservations);
            _mockMapper.Setup(mm => mm.Map<List<ReadedReservationContract>>(It.IsAny<List<Reservation>>())).Returns(readedReservationList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryReservation.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedReservationContract>>(It.IsAny<List<Reservation>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Reservations = new List<Reservation>() { new Reservation() { Id = 1 } }.AsQueryable();
            var readedReservation = new ReadedReservationContract { Id = 1 };
            _mockRepositoryReservation.Setup(mrt => mrt.Query()).Returns(Reservations);
            _mockMapper.Setup(mm => mm.Map<ReadedReservationContract>(It.IsAny<Reservation>())).Returns(readedReservation);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);            
            _mockRepositoryReservation.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedReservationContract>(It.IsAny<Reservation>()), Times.Once);
        }
    }
}