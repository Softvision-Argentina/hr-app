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
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<Office> _officeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<RoomService> _log;
        private readonly UpdateRoomContractValidator _updateRoomContractValidator;
        private readonly CreateRoomContractValidator _createRoomContractValidator;

        public RoomService(
            IMapper mapper,
            IRepository<Room> roomRepository,
            IRepository<Office> officeItemRepository,
            IUnitOfWork unitOfWork,
            ILog<RoomService> log,
            UpdateRoomContractValidator updateRoomContractValidator,
            CreateRoomContractValidator createRoomContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roomRepository = roomRepository;
            _officeRepository = officeItemRepository;
            _log = log;
            _updateRoomContractValidator = updateRoomContractValidator;
            _createRoomContractValidator = createRoomContractValidator;
        }

        public CreatedRoomContract Create(CreateRoomContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(0, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var room = _mapper.Map<Room>(contract);

            room.Office = _officeRepository.Query().Where(x => x.Id == room.OfficeId).FirstOrDefault();

            var createdRoom = _roomRepository.Create(room);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedRoomContract>(createdRoom);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching Candidate Profile {id}");
            Room room = _roomRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (room == null)
            {
                throw new DeleteRoomNotFoundException(id);
            }
            _log.LogInformation($"Deleting Candidate Profile {id}");
            _roomRepository.Delete(room);

            _unitOfWork.Complete();
        }

        public void Update(UpdateRoomContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(contract.Id, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var Room = _mapper.Map<Room>(contract);

            _roomRepository.Update(Room);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }


        public IEnumerable<ReadedRoomContract> List()
        {
            var roomQuery = _roomRepository
                .QueryEager();
                
            var roomResult = roomQuery.ToList();

            return _mapper.Map<List<ReadedRoomContract>>(roomResult);
        }

        public ReadedRoomContract Read(int id)
        {
            var roomQuery = _roomRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var roomResult = roomQuery.SingleOrDefault();

            return _mapper.Map<ReadedRoomContract>(roomResult);
        }

        private void ValidateContract(CreateRoomContract contract)
        {
            try
            {
                _createRoomContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
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
                _updateRoomContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
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
                Room room = _roomRepository.Query().Where(_ => _.Name == name && _.Id != id).FirstOrDefault();
                if (room != null) throw new InvalidRoomException("The Room already exists .");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
