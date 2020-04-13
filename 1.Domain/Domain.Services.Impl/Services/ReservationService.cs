using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Reservation;
using Domain.Services.Contracts.Reservation;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Reservation;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class ReservationService: IReservationService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Reservation> _ReservationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Room> _RoomRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<ReservationService> _log;
        private readonly IGoogleCalendarService _googleCalendarService;
        private readonly UpdateReservationContractValidator _updateReservationContractValidator;
        private readonly CreateReservationContractValidator _createReservationContractValidator;

        public ReservationService(
            IMapper mapper,
            IRepository<Reservation> ReservationRepository,
            IRepository<User> UserRepository,
            IRepository<Room> RoomRepository,
            IUnitOfWork unitOfWork,
            ILog<ReservationService> log,
            IGoogleCalendarService googleCalendarService,
            UpdateReservationContractValidator updateReservationContractValidator,
            CreateReservationContractValidator createReservationContractValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _ReservationRepository = ReservationRepository;
            _log = log;
            _updateReservationContractValidator = updateReservationContractValidator;
            _createReservationContractValidator = createReservationContractValidator;
            _RoomRepository = RoomRepository;
            _userRepository = UserRepository;
            _googleCalendarService = googleCalendarService;
        }

        public CreatedReservationContract Create(CreateReservationContract contract)
        {
            contract.SinceReservation = contract.SinceReservation.ToLocalTime();
            contract.UntilReservation = contract.UntilReservation.ToLocalTime();
            _log.LogInformation($"Validating contract {contract.Description}");
            ValidateContract(contract);

            _log.LogInformation($"Mapping contract {contract.Description}");
            var Reservation = _mapper.Map<Reservation>(contract);

            ValidateSchedule(Reservation);
            CheckOverlap(Reservation);

            Reservation.User = _userRepository.Query().Where(x => x.Id == contract.User).FirstOrDefault();
            Reservation.Room = _RoomRepository.Query().Where(x => x.Id == Reservation.RoomId).FirstOrDefault();

            var createdReservation = _ReservationRepository.Create(Reservation);

            AddModelToGoogleCalendar(Reservation);

            _log.LogInformation($"Complete for {contract.Description}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Description}");
            return _mapper.Map<CreatedReservationContract>(createdReservation);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching Reservation {id}");
            Reservation Reservation = _ReservationRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (Reservation == null)
            {
                throw new DeleteReservationNotFoundException(id);
            }
            _log.LogInformation($"Deleting Reservation {id}");
            _ReservationRepository.Delete(Reservation);

            _unitOfWork.Complete();
        }

        public void Update(UpdateReservationContract contract)
        {
            var reservationWithoutChanges = _ReservationRepository.Query().Where(r => r.Id == contract.Id).FirstOrDefault();
            if (reservationWithoutChanges.SinceReservation != contract.SinceReservation)
                contract.SinceReservation = contract.SinceReservation.ToLocalTime();
            if (reservationWithoutChanges.UntilReservation != contract.UntilReservation)
                contract.UntilReservation = contract.UntilReservation.ToLocalTime();

            _log.LogInformation($"Validating contract {contract.Description}");
            ValidateContract(contract);

            _log.LogInformation($"Mapping contract {contract.Description}");
            var Reservation = _mapper.Map<Reservation>(contract);

            ValidateSchedule(Reservation);
            CheckOverlap(Reservation);
            Reservation.User = _userRepository.Query().Where(x => x.Id == contract.User).FirstOrDefault();

            _ReservationRepository.Update(Reservation);
            _log.LogInformation($"Complete for {contract.Description}");
            _unitOfWork.Complete();
        }
        
        public ReadedReservationContract Read(int id)
        {
            var ReservationQuery = _ReservationRepository
                .Query()
                .Where(_ => _.Id == id)
                .OrderBy(_ => _.SinceReservation);

            var ReservationResult = ReservationQuery.SingleOrDefault();

            return _mapper.Map<ReadedReservationContract>(ReservationResult);
        }
        
        public IEnumerable<ReadedReservationContract> List()
        {
            var ReservationQuery = _ReservationRepository
                .Query()
                .OrderBy(_ => _.SinceReservation);

            var ReservationResult = ReservationQuery.ToList();

            return _mapper.Map<List<ReadedReservationContract>>(ReservationResult);
        }

        private void ValidateContract(CreateReservationContract contract)
        {
            try
            {
                _createReservationContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateReservationContract contract)
        {
            try
            {
                _updateReservationContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        public void ValidateSchedule(Reservation reservation)
        {
            DateTime reservationSince = DateTime.Parse(reservation.SinceReservation.ToString("g"));
            DateTime reservationUntil = DateTime.Parse(reservation.UntilReservation.ToString("g"));
            if(reservationSince > reservationUntil)
            {
                throw new InvalidReservationException("The selected schedule is invalid");
            }
        }

        private void CheckOverlap(Reservation newReservation)
        {
            DateTime currentReservationSince;
            DateTime currentReservationUntil;
            DateTime newReservationSince = DateTime.Parse(newReservation.SinceReservation.ToString("g"));
            DateTime newReservationUntil = DateTime.Parse(newReservation.UntilReservation.ToString("g"));
            List<Reservation> reservationsList = _ReservationRepository.Query().ToList();
            foreach (Reservation currentReservation in reservationsList)
            {
                currentReservationSince = DateTime.Parse(currentReservation.SinceReservation.ToString("g"));
                currentReservationUntil = DateTime.Parse(currentReservation.UntilReservation.ToString("g"));
                if (
                    newReservation.Id != currentReservation.Id &&
                    newReservation.RoomId == currentReservation.Room.Id &&
                    (
                    ((newReservationSince >= currentReservationSince && newReservationSince < currentReservationUntil) ||
                    (newReservationUntil > currentReservationSince && newReservationUntil <= currentReservationUntil)) ||
                    ((currentReservationSince >= newReservationSince && currentReservationSince < newReservationUntil) ||
                    (currentReservationUntil > newReservationSince && currentReservationUntil <= newReservationUntil))
                    )
                    )
                {
                    throw new InvalidReservationException("There is already a reservation for this moment.");
                }
            }
        }

        public void AddModelToGoogleCalendar(Reservation reservation)
        {
            Event newEvent = new Event
            {
                //Summary = reservation.Type,
                Start = new EventDateTime()
                {
                    DateTime = new System.DateTime(reservation.SinceReservation.Date.Year, reservation.SinceReservation.Date.Month, reservation.SinceReservation.Date.Day, reservation.SinceReservation.Date.Hour, reservation.SinceReservation.Date.Minute, 0)
                },
                End = new EventDateTime()
                {
                    DateTime = new System.DateTime(reservation.UntilReservation.Date.Year, reservation.UntilReservation.Date.Month, reservation.UntilReservation.Date.Day, reservation.UntilReservation.Date.Hour, reservation.UntilReservation.Date.Minute, 0)
                }
            };
            _googleCalendarService.CreateEvent(newEvent);
        }
    }
}
