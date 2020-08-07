// <copyright file="RoomServiceTest.cs" company="Softvision">
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
    using Domain.Services.Contracts.Room;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.Room;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class RoomServiceTest : BaseDomainTest
    {
        private readonly RoomService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Room>> mockRepositoryRoom;
        private readonly Mock<IRepository<Reservation>> mockRepositoryReservation;
        private readonly Mock<IRepository<Office>> mockRepositoryOffice;
        private readonly Mock<ILog<RoomService>> mockLogRoomService;
        private readonly Mock<UpdateRoomContractValidator> mockUpdateRoomContractValidator;
        private readonly Mock<CreateRoomContractValidator> mockCreateRoomContractValidator;

        public RoomServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryRoom = new Mock<IRepository<Room>>();
            this.mockRepositoryReservation = new Mock<IRepository<Reservation>>();
            this.mockRepositoryOffice = new Mock<IRepository<Office>>();
            this.mockLogRoomService = new Mock<ILog<RoomService>>();
            this.mockUpdateRoomContractValidator = new Mock<UpdateRoomContractValidator>();
            this.mockCreateRoomContractValidator = new Mock<CreateRoomContractValidator>();
            this.service = new RoomService(
                this.mockMapper.Object,
                this.mockRepositoryRoom.Object,
                this.mockRepositoryOffice.Object,
                this.MockUnitOfWork.Object,
                this.mockLogRoomService.Object,
                this.mockUpdateRoomContractValidator.Object,
                this.mockCreateRoomContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create RoomService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateRoomService()
        {
            var contract = new CreateRoomContract();
            var expectedRoom = new CreatedRoomContract();
            this.mockCreateRoomContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoomContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Room>(It.IsAny<CreateRoomContract>())).Returns(new Room());
            this.mockRepositoryRoom.Setup(repoCom => repoCom.Create(It.IsAny<Room>())).Returns(new Room());
            this.mockMapper.Setup(mm => mm.Map<CreatedRoomContract>(It.IsAny<Room>())).Returns(expectedRoom);

            var createdRoom = this.service.Create(contract);

            Assert.NotNull(createdRoom);
            Assert.Equal(expectedRoom, createdRoom);
            this.mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateRoomContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoomContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Room>(It.IsAny<CreateRoomContract>()), Times.Once);
            this.mockRepositoryRoom.Verify(mrt => mrt.Create(It.IsAny<Room>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedRoomContract>(It.IsAny<Room>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateRoomContract();
            var expectedRoom = new CreatedRoomContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateRoomContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoomContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedRoomContract>(It.IsAny<Room>())).Returns(expectedRoom);

            var exception = Assert.Throws<Model.Exceptions.Room.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateRoomContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoomContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Room>(It.IsAny<CreateRoomContract>()), Times.Never);
            this.mockRepositoryRoom.Verify(mrt => mrt.Create(It.IsAny<Room>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedRoomContract>(It.IsAny<Room>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete RoomService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteRoomService()
        {
            var rooms = new List<Room>() { new Room() { Id = 1 } }.AsQueryable();
            this.mockRepositoryRoom.Setup(mrt => mrt.Query()).Returns(rooms);

            this.service.Delete(1);

            this.mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryRoom.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryRoom.Verify(mrt => mrt.Delete(It.IsAny<Room>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteRoomNotFoundException()
        {
            var expectedErrorMEssage = $"Room not found for the Room Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.Room.DeleteRoomNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryRoom.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryRoom.Verify(mrt => mrt.Delete(It.IsAny<Room>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update RoomService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateRoomContract();
            this.mockUpdateRoomContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoomContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Room>(It.IsAny<UpdateRoomContract>())).Returns(new Room());

            this.service.Update(contract);

            this.mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateRoomContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoomContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Room>(It.IsAny<UpdateRoomContract>()), Times.Once);
            this.mockRepositoryRoom.Verify(mrt => mrt.Update(It.IsAny<Room>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateRoomContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateRoomContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoomContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<Room>(It.IsAny<UpdateRoomContract>())).Returns(new Room());

            var exception = Assert.Throws<Model.Exceptions.Room.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogRoomService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateRoomContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoomContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Room>(It.IsAny<UpdateRoomContract>()), Times.Never);
            this.mockRepositoryRoom.Verify(mrt => mrt.Update(It.IsAny<Room>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var rooms = new List<Room>() { new Room() { Id = 1 } }.AsQueryable();
            var readedRoomList = new List<ReadedRoomContract> { new ReadedRoomContract { Id = 1 } };
            this.mockRepositoryRoom.Setup(mrt => mrt.QueryEager()).Returns(rooms);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedRoomContract>>(It.IsAny<List<Room>>())).Returns(readedRoomList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryRoom.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedRoomContract>>(It.IsAny<List<Room>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var rooms = new List<Room>() { new Room() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedRoom = new ReadedRoomContract { Id = 1, Name = "Name" };
            this.mockRepositoryRoom.Setup(mrt => mrt.QueryEager()).Returns(rooms);
            this.mockMapper.Setup(mm => mm.Map<ReadedRoomContract>(It.IsAny<Room>())).Returns(readedRoom);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            this.mockRepositoryRoom.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedRoomContract>(It.IsAny<Room>()), Times.Once);
        }
    }
}