using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Room;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.Room;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class RoomServiceTest : BaseDomainTest
    {
        private readonly RoomService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Room>> _mockRepositoryRoom;
        private readonly Mock<IRepository<Reservation>> _mockRepositoryReservation;        
        private readonly Mock<IRepository<Office>> _mockRepositoryOffice;
        private readonly Mock<ILog<RoomService>> _mockLogRoomService;
        private readonly Mock<UpdateRoomContractValidator> _mockUpdateRoomContractValidator;
        private readonly Mock<CreateRoomContractValidator> _mockCreateRoomContractValidator;

        public RoomServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryRoom = new Mock<IRepository<Room>>();
            _mockRepositoryReservation = new Mock<IRepository<Reservation>>();            
            _mockRepositoryOffice = new Mock<IRepository<Office>>();
            _mockLogRoomService = new Mock<ILog<RoomService>>();
            _mockUpdateRoomContractValidator = new Mock<UpdateRoomContractValidator>();
            _mockCreateRoomContractValidator = new Mock<CreateRoomContractValidator>();
            _service = new RoomService(
                _mockMapper.Object,
                _mockRepositoryRoom.Object,
                _mockRepositoryReservation.Object,
                _mockRepositoryOffice.Object,
                MockUnitOfWork.Object,
                _mockLogRoomService.Object,
                _mockUpdateRoomContractValidator.Object,
                _mockCreateRoomContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create RoomService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateRoomService()
        {
            var contract = new CreateRoomContract();
            var expectedRoom = new CreatedRoomContract();
            _mockCreateRoomContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoomContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Room>(It.IsAny<CreateRoomContract>())).Returns(new Room());
            _mockRepositoryRoom.Setup(repoCom => repoCom.Create(It.IsAny<Room>())).Returns(new Room());
            _mockMapper.Setup(mm => mm.Map<CreatedRoomContract>(It.IsAny<Room>())).Returns(expectedRoom);

            var createdRoom = _service.Create(contract);

            Assert.NotNull(createdRoom);
            Assert.Equal(expectedRoom, createdRoom);
            _mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateRoomContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoomContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Room>(It.IsAny<CreateRoomContract>()), Times.Once);
            _mockRepositoryRoom.Verify(mrt => mrt.Create(It.IsAny<Room>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedRoomContract>(It.IsAny<Room>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateRoomContract();
            var expectedRoom = new CreatedRoomContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateRoomContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoomContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedRoomContract>(It.IsAny<Room>())).Returns(expectedRoom);

            var exception = Assert.Throws<Model.Exceptions.Room.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateRoomContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoomContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Room>(It.IsAny<CreateRoomContract>()), Times.Never);
            _mockRepositoryRoom.Verify(mrt => mrt.Create(It.IsAny<Room>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedRoomContract>(It.IsAny<Room>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete RoomService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteRoomService()
        {
            var Rooms = new List<Room>() { new Room() { Id = 1 } }.AsQueryable();
            _mockRepositoryRoom.Setup(mrt => mrt.Query()).Returns(Rooms);

            _service.Delete(1);

            _mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryRoom.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryRoom.Verify(mrt => mrt.Delete(It.IsAny<Room>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteRoomNotFoundException()
        {
            var expectedErrorMEssage = $"Room not found for the Room Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.Room.DeleteRoomNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryRoom.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryRoom.Verify(mrt => mrt.Delete(It.IsAny<Room>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update RoomService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateRoomContract();
            _mockUpdateRoomContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoomContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Room>(It.IsAny<UpdateRoomContract>())).Returns(new Room());

            _service.Update(contract);

            _mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateRoomContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoomContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Room>(It.IsAny<UpdateRoomContract>()), Times.Once);
            _mockRepositoryRoom.Verify(mrt => mrt.Update(It.IsAny<Room>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateRoomContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateRoomContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoomContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<Room>(It.IsAny<UpdateRoomContract>())).Returns(new Room());

            var exception = Assert.Throws<Model.Exceptions.Room.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateRoomContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoomContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Room>(It.IsAny<UpdateRoomContract>()), Times.Never);
            _mockRepositoryRoom.Verify(mrt => mrt.Update(It.IsAny<Room>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Rooms = new List<Room>() { new Room() { Id = 1 } }.AsQueryable();
            var readedRoomList = new List<ReadedRoomContract> { new ReadedRoomContract { Id = 1 } };
            _mockRepositoryRoom.Setup(mrt => mrt.QueryEager()).Returns(Rooms);
            _mockMapper.Setup(mm => mm.Map<List<ReadedRoomContract>>(It.IsAny<List<Room>>())).Returns(readedRoomList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryRoom.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedRoomContract>>(It.IsAny<List<Room>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Rooms = new List<Room>() { new Room() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedRoom = new ReadedRoomContract { Id = 1, Name = "Name" };
            _mockRepositoryRoom.Setup(mrt => mrt.QueryEager()).Returns(Rooms);
            _mockMapper.Setup(mm => mm.Map<ReadedRoomContract>(It.IsAny<Room>())).Returns(readedRoom);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            _mockRepositoryRoom.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedRoomContract>(It.IsAny<Room>()), Times.Once);
        }
    }
}