// <copyright file="RoomService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Model.Exceptions.Room;
    using Domain.Services.Contracts.Room;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.Room;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class RoomService : IRoomService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Room> roomRepository;
        private readonly IRepository<Office> officeRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<RoomService> log;
        private readonly UpdateRoomContractValidator updateRoomContractValidator;
        private readonly CreateRoomContractValidator createRoomContractValidator;

        public RoomService(
            IMapper mapper,
            IRepository<Room> roomRepository,
            IRepository<Office> officeItemRepository,
            IUnitOfWork unitOfWork,
            ILog<RoomService> log,
            UpdateRoomContractValidator updateRoomContractValidator,
            CreateRoomContractValidator createRoomContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.roomRepository = roomRepository;
            this.officeRepository = officeItemRepository;
            this.log = log;
            this.updateRoomContractValidator = updateRoomContractValidator;
            this.createRoomContractValidator = createRoomContractValidator;
        }

        public CreatedRoomContract Create(CreateRoomContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(0, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var room = this.mapper.Map<Room>(contract);

            room.Office = this.officeRepository.Query().Where(x => x.Id == room.OfficeId).FirstOrDefault();

            var createdRoom = this.roomRepository.Create(room);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedRoomContract>(createdRoom);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching Candidate Profile {id}");
            Room room = this.roomRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (room == null)
            {
                throw new DeleteRoomNotFoundException(id);
            }

            this.log.LogInformation($"Deleting Candidate Profile {id}");
            this.roomRepository.Delete(room);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateRoomContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(contract.Id, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var room = this.mapper.Map<Room>(contract);

            this.roomRepository.Update(room);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedRoomContract> List()
        {
            var roomQuery = this.roomRepository
                .QueryEager();

            var roomResult = roomQuery.ToList();

            return this.mapper.Map<List<ReadedRoomContract>>(roomResult);
        }

        public ReadedRoomContract Read(int id)
        {
            var roomQuery = this.roomRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var roomResult = roomQuery.SingleOrDefault();

            return this.mapper.Map<ReadedRoomContract>(roomResult);
        }

        private void ValidateContract(CreateRoomContract contract)
        {
            try
            {
                this.createRoomContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateRoomContract contract)
        {
            try
            {
                this.updateRoomContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETDEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateExistence(int id, string name)
        {
            try
            {
                Room room = this.roomRepository.Query().Where(_ => _.Name == name && _.Id != id).FirstOrDefault();
                if (room != null)
                {
                    throw new InvalidRoomException("The Room already exists .");
                }
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
