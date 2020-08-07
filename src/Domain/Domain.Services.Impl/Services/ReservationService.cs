// <copyright file="ReservationService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

    public class ReservationService : IReservationService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Reservation> reservationRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Room> roomRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<ReservationService> log;
        private readonly IGoogleCalendarService googleCalendarService;
        private readonly UpdateReservationContractValidator updateReservationContractValidator;
        private readonly CreateReservationContractValidator createReservationContractValidator;

        public ReservationService(
            IMapper mapper,
            IRepository<Reservation> reservationRepository,
            IRepository<User> userRepository,
            IRepository<Room> roomRepository,
            IUnitOfWork unitOfWork,
            ILog<ReservationService> log,
            IGoogleCalendarService googleCalendarService,
            UpdateReservationContractValidator updateReservationContractValidator,
            CreateReservationContractValidator createReservationContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.reservationRepository = reservationRepository;
            this.log = log;
            this.updateReservationContractValidator = updateReservationContractValidator;
            this.createReservationContractValidator = createReservationContractValidator;
            this.roomRepository = roomRepository;
            this.userRepository = userRepository;
            this.googleCalendarService = googleCalendarService;
        }

        public CreatedReservationContract Create(CreateReservationContract contract)
        {
            contract.SinceReservation = contract.SinceReservation.ToLocalTime();
            contract.UntilReservation = contract.UntilReservation.ToLocalTime();
            this.log.LogInformation($"Validating contract {contract.Description}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Description}");
            var reservation = this.mapper.Map<Reservation>(contract);

            this.ValidateSchedule(reservation);
            this.CheckOverlap(reservation);

            reservation.User = this.userRepository.Query().Where(x => x.Id == contract.User).FirstOrDefault();
            reservation.Room = this.roomRepository.Query().Where(x => x.Id == reservation.RoomId).FirstOrDefault();

            var createdReservation = this.reservationRepository.Create(reservation);

            this.AddModelToGoogleCalendar(reservation);

            this.log.LogInformation($"Complete for {contract.Description}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Description}");
            return this.mapper.Map<CreatedReservationContract>(createdReservation);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching Reservation {id}");
            Reservation reservation = this.reservationRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (reservation == null)
            {
                throw new DeleteReservationNotFoundException(id);
            }

            this.log.LogInformation($"Deleting Reservation {id}");
            this.reservationRepository.Delete(reservation);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateReservationContract contract)
        {
            var reservationWithoutChanges = this.reservationRepository.Query().Where(r => r.Id == contract.Id).FirstOrDefault();
            if (reservationWithoutChanges.SinceReservation != contract.SinceReservation)
            {
                contract.SinceReservation = contract.SinceReservation.ToLocalTime();
            }

            if (reservationWithoutChanges.UntilReservation != contract.UntilReservation)
            {
                contract.UntilReservation = contract.UntilReservation.ToLocalTime();
            }

            this.log.LogInformation($"Validating contract {contract.Description}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Description}");
            var reservation = this.mapper.Map<Reservation>(contract);

            this.ValidateSchedule(reservation);
            this.CheckOverlap(reservation);
            reservation.User = this.userRepository.Query().Where(x => x.Id == contract.User).FirstOrDefault();

            this.reservationRepository.Update(reservation);
            this.log.LogInformation($"Complete for {contract.Description}");
            this.unitOfWork.Complete();
        }

        public ReadedReservationContract Read(int id)
        {
            var reservationQuery = this.reservationRepository
                .Query()
                .Where(_ => _.Id == id)
                .OrderBy(_ => _.SinceReservation);

            var reservationResult = reservationQuery.SingleOrDefault();

            return this.mapper.Map<ReadedReservationContract>(reservationResult);
        }

        public IEnumerable<ReadedReservationContract> List()
        {
            var reservationQuery = this.reservationRepository
                .Query()
                .OrderBy(_ => _.SinceReservation);

            var reservationResult = reservationQuery.ToList();

            return this.mapper.Map<List<ReadedReservationContract>>(reservationResult);
        }

        private void ValidateContract(CreateReservationContract contract)
        {
            try
            {
                this.createReservationContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
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
                this.updateReservationContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETDEFAULT}");
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
            if (reservationSince > reservationUntil)
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
            List<Reservation> reservationsList = this.reservationRepository.Query().ToList();
            foreach (Reservation currentReservation in reservationsList)
            {
                currentReservationSince = DateTime.Parse(currentReservation.SinceReservation.ToString("g"));
                currentReservationUntil = DateTime.Parse(currentReservation.UntilReservation.ToString("g"));
                if (
                    newReservation.Id != currentReservation.Id &&
                    newReservation.RoomId == currentReservation.Room.Id &&
                    (
                    (newReservationSince >= currentReservationSince && newReservationSince < currentReservationUntil) ||
                    (newReservationUntil > currentReservationSince && newReservationUntil <= currentReservationUntil) ||
                    (currentReservationSince >= newReservationSince && currentReservationSince < newReservationUntil) ||
                    (currentReservationUntil > newReservationSince && currentReservationUntil <= newReservationUntil)))
                {
                    throw new InvalidReservationException("There is already a reservation for this moment.");
                }
            }
        }

        public void AddModelToGoogleCalendar(Reservation reservation)
        {
            Event newEvent = new Event
            {
                // Summary = reservation.Type,
                Start = new EventDateTime()
                {
                    DateTime = new System.DateTime(reservation.SinceReservation.Date.Year, reservation.SinceReservation.Date.Month, reservation.SinceReservation.Date.Day, reservation.SinceReservation.Date.Hour, reservation.SinceReservation.Date.Minute, 0),
                },
                End = new EventDateTime()
                {
                    DateTime = new System.DateTime(reservation.UntilReservation.Date.Year, reservation.UntilReservation.Date.Month, reservation.UntilReservation.Date.Day, reservation.UntilReservation.Date.Hour, reservation.UntilReservation.Date.Minute, 0),
                },
            };
            this.googleCalendarService.CreateEvent(newEvent);
        }
    }
}
