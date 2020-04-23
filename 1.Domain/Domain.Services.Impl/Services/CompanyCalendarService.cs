using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.CompanyCalendar;
using Domain.Services.Contracts.CompanyCalendar;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.CompanyCalendar;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using Google.Apis.Calendar.v3.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class CompanyCalendarService : ICompanyCalendarService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CompanyCalendar> _companyCalendarRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<CompanyCalendarService> _log;
        private readonly IGoogleCalendarService _googleCalendarService;
        private readonly UpdateCompanyCalendarContractValidator _updateCompanyCalendarContractValidator;
        private readonly CreateCompanyCalendarContractValidator _createCompanyCalendarContractValidator;

        public CompanyCalendarService(
            IMapper mapper,
            IRepository<CompanyCalendar> companyCalendarRepository,
            IUnitOfWork unitOfWork,
            ILog<CompanyCalendarService> log,
            IGoogleCalendarService googleCalendarService,
            UpdateCompanyCalendarContractValidator updateCompanyCalendarContractValidator,
            CreateCompanyCalendarContractValidator createCompanyCalendarContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _companyCalendarRepository = companyCalendarRepository;
            _log = log;
            _updateCompanyCalendarContractValidator = updateCompanyCalendarContractValidator;
            _createCompanyCalendarContractValidator = createCompanyCalendarContractValidator;
            _googleCalendarService = googleCalendarService;
        }

        public IEnumerable<ReadedCompanyCalendarContract> List()
        {
            var companyCalendarQuery = _companyCalendarRepository.QueryEager();

            var companyCalendars = companyCalendarQuery.ToList();

            return _mapper.Map<List<ReadedCompanyCalendarContract>>(companyCalendars);
        }

        public CreatedCompanyCalendarContract Create(CreateCompanyCalendarContract contract)
        {
            ValidateContract(contract);

            var companyCalendar = _mapper.Map<CompanyCalendar>(contract);

            var createdCompanyCalendar = _companyCalendarRepository.Create(companyCalendar);

            this.AddModelToGoogleCalendar(companyCalendar);

            _unitOfWork.Complete();
            return _mapper.Map<CreatedCompanyCalendarContract>(createdCompanyCalendar);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching company calendar {id}");
            CompanyCalendar companyCalendar = _companyCalendarRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (companyCalendar == null)
            {
                throw new DeleteCompanyCalendarNotFoundException(id);
            }
            _log.LogInformation($"Deleting company calendar {id}");
            _companyCalendarRepository.Delete(companyCalendar);

            _unitOfWork.Complete();
        }

        public void Update(UpdateCompanyCalendarContract contract)
        {
            ValidateContract(contract);

            var companyCalendar = _mapper.Map<CompanyCalendar>(contract);

            var updatedCompanyCalendar = _companyCalendarRepository.Update(companyCalendar);

            this.AddModelToGoogleCalendar(companyCalendar);

            _unitOfWork.Complete();
        }

        public ReadedCompanyCalendarContract Read(int id)
        {
            var companyCalendarQuery = _companyCalendarRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var companyCalendarResult = companyCalendarQuery.SingleOrDefault();

            return _mapper.Map<ReadedCompanyCalendarContract>(companyCalendarResult);
        }

        private void ValidateContract(CreateCompanyCalendarContract contract)
        {
            try
            {
                _createCompanyCalendarContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateCompanyCalendarContract contract)
        {
            try
            {
                _updateCompanyCalendarContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        public void AddModelToGoogleCalendar(CompanyCalendar companyCalendar)
        {
            Event newEvent = new Event
            {
                Description = companyCalendar.Type,
                Summary = companyCalendar.Comments,
                Start = new EventDateTime()
                {
                    DateTime = new System.DateTime(companyCalendar.Date.Date.Year, companyCalendar.Date.Date.Month, companyCalendar.Date.Date.Day, 8, 0, 0)
                },
                End = new EventDateTime()
                {
                    DateTime = new System.DateTime(companyCalendar.Date.Date.Year, companyCalendar.Date.Date.Month, companyCalendar.Date.Date.Day, 19, 0, 0)
                }
            };
            _googleCalendarService.CreateEvent(newEvent);

        }

    }
}
