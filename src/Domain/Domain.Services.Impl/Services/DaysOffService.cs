// <copyright file="DaysOffService.cs" company="Softvision">
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
    using Domain.Model.Enum;
    using Domain.Model.Exceptions.DaysOff;
    using Domain.Services.Contracts.DaysOff;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.DaysOff;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;
    using Google.Apis.Calendar.v3.Data;

    public class DaysOffService : IDaysOffService
    {
        private readonly IMapper mapper;
        private readonly IRepository<DaysOff> daysOffRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<DaysOffService> log;
        private readonly IGoogleCalendarService googleCalendarService;
        private readonly UpdateDaysOffContractValidator updateDaysOffContractValidator;
        private readonly CreateDaysOffContractValidator createDaysOffContractValidator;

        public DaysOffService(
            IMapper mapper,
            IRepository<DaysOff> daysOffRepository,
            IUnitOfWork unitOfWork,
            ILog<DaysOffService> log,
            IGoogleCalendarService googleCalendarService,
            UpdateDaysOffContractValidator updateDaysOffContractValidator,
            CreateDaysOffContractValidator createDaysOffContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.daysOffRepository = daysOffRepository;
            this.log = log;
            this.updateDaysOffContractValidator = updateDaysOffContractValidator;
            this.createDaysOffContractValidator = createDaysOffContractValidator;
            this.googleCalendarService = googleCalendarService;
        }

        public IEnumerable<ReadedDaysOffContract> List()
        {
            var daysOffQuery = this.daysOffRepository.QueryEager();

            var daysOffs = daysOffQuery.ToList();

            return this.mapper.Map<List<ReadedDaysOffContract>>(daysOffs);
        }

        public CreatedDaysOffContract Create(CreateDaysOffContract contract)
        {
            this.ValidateContract(contract);

            var daysOff = this.mapper.Map<DaysOff>(contract);

            var createdDaysOff = this.daysOffRepository.Create(daysOff);

            if (daysOff.Status == Model.Enum.DaysOffStatus.Accepted)
            {
                var googleCalendarEventId = this.AddModelToGoogleCalendar(daysOff);

                createdDaysOff.GoogleCalendarEventId = googleCalendarEventId;
            }

            this.unitOfWork.Complete();
            return this.mapper.Map<CreatedDaysOffContract>(createdDaysOff);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching days Off {id}");
            DaysOff daysOff = this.daysOffRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (daysOff == null)
            {
                throw new DeleteDaysOffNotFoundException(id);
            }

            this.log.LogInformation($"Deleting days Off {id}");
            this.daysOffRepository.Delete(daysOff);

            if (string.IsNullOrEmpty(daysOff.GoogleCalendarEventId) && !this.DeleteEventInGoogleCalendar(daysOff))
            {
                this.log.LogInformation($"Could not delete google calendar event for days off {id}");
            }

            this.unitOfWork.Complete();
        }

        public void Update(UpdateDaysOffContract contract)
        {
            this.ValidateContract(contract);

            var daysOff = this.mapper.Map<DaysOff>(contract);

            var updatedDaysOff = this.daysOffRepository.Update(daysOff);

            if (daysOff.Status == Model.Enum.DaysOffStatus.Accepted)
            {
                var googleCalendarEventId = this.AddModelToGoogleCalendar(daysOff);

                updatedDaysOff.GoogleCalendarEventId = googleCalendarEventId;
            }

            this.unitOfWork.Complete();
        }

        public void AcceptPetition(int id)
        {
            var daysOff = this.daysOffRepository.Query().FirstOrDefault(_ => _.Id == id);

            if (daysOff == null)
            {
                throw new UpdateDaysOffNotFoundException(id, new System.Guid());
            }

            daysOff.Status = DaysOffStatus.Accepted;
            var updatedDaysOff = this.daysOffRepository.Update(daysOff);
            var googleCalendarEventId = this.AddModelToGoogleCalendar(daysOff);

            updatedDaysOff.GoogleCalendarEventId = googleCalendarEventId;

            this.unitOfWork.Complete();
        }

        public ReadedDaysOffContract Read(int id)
        {
            var daysOffQuery = this.daysOffRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var daysOffResult = daysOffQuery.SingleOrDefault();

            return this.mapper.Map<ReadedDaysOffContract>(daysOffResult);
        }

        public IEnumerable<ReadedDaysOffContract> ReadByDni(int dni)
        {
            var daysOffQuery = this.daysOffRepository
                .QueryEager()
                .Where(_ => _.Employee.DNI == dni).ToList();

            return this.mapper.Map<List<ReadedDaysOffContract>>(daysOffQuery);
        }

        private void ValidateContract(CreateDaysOffContract contract)
        {
            try
            {
                this.createDaysOffContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateDaysOffContract contract)
        {
            try
            {
                this.updateDaysOffContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETDEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        public string AddModelToGoogleCalendar(DaysOff daysOff)
        {
            Event newEvent = new Event
            {
                Summary = ((DaysOffType)daysOff.Type).ToString(),
                Start = new EventDateTime()
                {
                    DateTime = new System.DateTime(daysOff.Date.Date.Year, daysOff.Date.Date.Month, daysOff.Date.Date.Day, 8, 0, 0),
                },
                End = new EventDateTime()
                {
                    DateTime = new System.DateTime(daysOff.EndDate.Date.Year, daysOff.EndDate.Date.Month, daysOff.EndDate.Date.Day, 8, 0, 0),
                },
                Attendees = new List<EventAttendee>(),
            };

            newEvent.Attendees.Add(new EventAttendee() { Email = daysOff.Employee.EmailAddress });

            return this.googleCalendarService.CreateEvent(newEvent);
        }

        public bool DeleteEventInGoogleCalendar(DaysOff daysOff)
        {
            return this.googleCalendarService.DeleteEvent(daysOff.GoogleCalendarEventId);
        }
    }
}
