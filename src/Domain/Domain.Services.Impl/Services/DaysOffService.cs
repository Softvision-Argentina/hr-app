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
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class DaysOffService : IDaysOffService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<DaysOff> _daysOffRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<DaysOffService> _log;
        private readonly IGoogleCalendarService _googleCalendarService;
        private readonly UpdateDaysOffContractValidator _updateDaysOffContractValidator;
        private readonly CreateDaysOffContractValidator _createDaysOffContractValidator;

        public DaysOffService(
            IMapper mapper,
            IRepository<DaysOff> daysOffRepository,
            IUnitOfWork unitOfWork,
            ILog<DaysOffService> log,
            IGoogleCalendarService googleCalendarService,
            UpdateDaysOffContractValidator updateDaysOffContractValidator,
            CreateDaysOffContractValidator createDaysOffContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _daysOffRepository = daysOffRepository;
            _log = log;
            _updateDaysOffContractValidator = updateDaysOffContractValidator;
            _createDaysOffContractValidator = createDaysOffContractValidator;
            _googleCalendarService = googleCalendarService;
        }

        public IEnumerable<ReadedDaysOffContract> List()
        {
            var daysOffQuery = _daysOffRepository.QueryEager();

            var daysOffs = daysOffQuery.ToList();

            return _mapper.Map<List<ReadedDaysOffContract>>(daysOffs);
        }

        public CreatedDaysOffContract Create(CreateDaysOffContract contract)
        {
            ValidateContract(contract);

            var daysOff = _mapper.Map<DaysOff>(contract);

            var createdDaysOff = _daysOffRepository.Create(daysOff);

            if (daysOff.Status == Model.Enum.DaysOffStatus.Accepted)
            {
                var googleCalendarEventId = this.AddModelToGoogleCalendar(daysOff);

                createdDaysOff.GoogleCalendarEventId = googleCalendarEventId;
            }

            _unitOfWork.Complete();
            return _mapper.Map<CreatedDaysOffContract>(createdDaysOff);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching days Off {id}");
            DaysOff daysOff = _daysOffRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (daysOff == null)
            {
                throw new DeleteDaysOffNotFoundException(id);
            }
            _log.LogInformation($"Deleting days Off {id}");
            _daysOffRepository.Delete(daysOff);

            if (string.IsNullOrEmpty(daysOff.GoogleCalendarEventId) && !DeleteEventInGoogleCalendar(daysOff))
            {
                _log.LogInformation($"Could not delete google calendar event for days off {id}");
            }

            _unitOfWork.Complete();
        }

        public void Update(UpdateDaysOffContract contract)
        {
            ValidateContract(contract);

            var daysOff = _mapper.Map<DaysOff>(contract);

            var updatedDaysOff = _daysOffRepository.Update(daysOff);

            if (daysOff.Status == Model.Enum.DaysOffStatus.Accepted)
            {
                var googleCalendarEventId = this.AddModelToGoogleCalendar(daysOff);

                updatedDaysOff.GoogleCalendarEventId = googleCalendarEventId;
            }

            _unitOfWork.Complete();
        }

        public void AcceptPetition(int id)
        {
            var daysOff = _daysOffRepository.Query().FirstOrDefault(_ => _.Id == id);

            if (daysOff == null)
            {
                throw new UpdateDaysOffNotFoundException(id, new System.Guid());
            }

            daysOff.Status = DaysOffStatus.Accepted;
            var updatedDaysOff = _daysOffRepository.Update(daysOff);
            var googleCalendarEventId = this.AddModelToGoogleCalendar(daysOff);

            updatedDaysOff.GoogleCalendarEventId = googleCalendarEventId;

            _unitOfWork.Complete();
        }

        public ReadedDaysOffContract Read(int id)
        {
            var daysOffQuery = _daysOffRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var daysOffResult = daysOffQuery.SingleOrDefault();

            return _mapper.Map<ReadedDaysOffContract>(daysOffResult);
        }

        public IEnumerable<ReadedDaysOffContract> ReadByDni(int dni)
        {
            var daysOffQuery = _daysOffRepository
                .QueryEager()
                .Where(_ => _.Employee.DNI == dni).ToList();

            return _mapper.Map<List<ReadedDaysOffContract>>(daysOffQuery);
        }

        private void ValidateContract(CreateDaysOffContract contract)
        {
            try
            {
                _createDaysOffContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
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
                _updateDaysOffContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
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
                    DateTime = new System.DateTime(daysOff.Date.Date.Year, daysOff.Date.Date.Month, daysOff.Date.Date.Day, 8, 0, 0)
                },
                End = new EventDateTime()
                {
                    DateTime = new System.DateTime(daysOff.EndDate.Date.Year, daysOff.EndDate.Date.Month, daysOff.EndDate.Date.Day, 8, 0, 0)
                },
                Attendees = new List<EventAttendee>()
            };

            newEvent.Attendees.Add(new EventAttendee() { Email = daysOff.Employee.EmailAddress });            

            return _googleCalendarService.CreateEvent(newEvent);
        }

        public bool DeleteEventInGoogleCalendar(DaysOff daysOff)
        {
            return _googleCalendarService.DeleteEvent(daysOff.GoogleCalendarEventId);
        }
    }
}
