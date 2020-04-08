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
        private readonly IRepository<Room> _RoomRepository;
        private readonly IRepository<Office> _OfficeRepository;
        private readonly IRepository<Reservation> _ReservationItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<RoomService> _log;
        private readonly UpdateRoomContractValidator _updateRoomContractValidator;
        private readonly CreateRoomContractValidator _createRoomContractValidator;

        public RoomService(
            IMapper mapper,
            IRepository<Room> RoomRepository,
            IRepository<Reservation> ReservationItemRepository,
            IRepository<Office> OfficeItemRepository,
            IUnitOfWork unitOfWork,
            ILog<RoomService> log,
            UpdateRoomContractValidator updateRoomContractValidator,
            CreateRoomContractValidator createRoomContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _RoomRepository = RoomRepository;
            _ReservationItemRepository = ReservationItemRepository;
            _OfficeRepository = OfficeItemRepository;
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
            var Room = _mapper.Map<Room>(contract);

            Room.Office = _OfficeRepository.Query().Where(x => x.Id == Room.OfficeId).FirstOrDefault();

            var createdRoom = _RoomRepository.Create(Room);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedRoomContract>(createdRoom);
        }

        public void Delete(int Id)
        {
            _log.LogInformation($"Searching Candidate Profile {Id}");
            Room Room = _RoomRepository.Query().Where(_ => _.Id == Id).FirstOrDefault();

            if (Room == null)
            {
                throw new DeleteRoomNotFoundException(Id);
            }
            _log.LogInformation($"Deleting Candidate Profile {Id}");
            _RoomRepository.Delete(Room);

            _unitOfWork.Complete();
        }

        public void Update(UpdateRoomContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(contract.Id, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var Room = _mapper.Map<Room>(contract);


            var updatedRoom = _RoomRepository.Update(Room);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }


        public IEnumerable<ReadedRoomContract> List()
        {
            var RoomQuery = _RoomRepository
                .QueryEager(); 
                //.Query();
                
            var RoomResult = RoomQuery.ToList();

            return _mapper.Map<List<ReadedRoomContract>>(RoomResult);
        }

        public ReadedRoomContract Read(int Id)
        {
            var RoomQuery = _RoomRepository
                .QueryEager()
                // .Query()
                .Where(_ => _.Id == Id);

            var RoomResult = RoomQuery.SingleOrDefault();

            return _mapper.Map<ReadedRoomContract>(RoomResult);
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

        private void ValidateExistence(int Id, string name)
        {
            try
            {
                Room Room = _RoomRepository.Query().Where(_ => _.Name == name && _.Id != Id).FirstOrDefault();
                if (Room != null) throw new InvalidRoomException("The Room already exists .");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
