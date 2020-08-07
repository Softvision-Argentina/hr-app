// <copyright file="CompanyCalendarService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.CompanyCalendar;
    using Domain.Services.Contracts.CompanyCalendar;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.CompanyCalendar;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;
    using Google.Apis.Calendar.v3.Data;

    public class CompanyCalendarService : ICompanyCalendarService
    {
        private readonly IMapper mapper;
        private readonly IRepository<CompanyCalendar> companyCalendarRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<CompanyCalendarService> log;
        private readonly IGoogleCalendarService googleCalendarService;
        private readonly UpdateCompanyCalendarContractValidator updateCompanyCalendarContractValidator;
        private readonly CreateCompanyCalendarContractValidator createCompanyCalendarContractValidator;

        public CompanyCalendarService(
            IMapper mapper,
            IRepository<CompanyCalendar> companyCalendarRepository,
            IUnitOfWork unitOfWork,
            ILog<CompanyCalendarService> log,
            IGoogleCalendarService googleCalendarService,
            UpdateCompanyCalendarContractValidator updateCompanyCalendarContractValidator,
            CreateCompanyCalendarContractValidator createCompanyCalendarContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.companyCalendarRepository = companyCalendarRepository;
            this.log = log;
            this.updateCompanyCalendarContractValidator = updateCompanyCalendarContractValidator;
            this.createCompanyCalendarContractValidator = createCompanyCalendarContractValidator;
            this.googleCalendarService = googleCalendarService;
        }

        public IEnumerable<ReadedCompanyCalendarContract> List()
        {
            var companyCalendarQuery = this.companyCalendarRepository.QueryEager();
            var companyCalendars = companyCalendarQuery.ToList();
            return this.mapper.Map<List<ReadedCompanyCalendarContract>>(companyCalendars);
        }

        public CreatedCompanyCalendarContract Create(CreateCompanyCalendarContract contract)
        {
            this.ValidateContract(contract);

            var companyCalendar = this.mapper.Map<CompanyCalendar>(contract);
            var createdCompanyCalendar = this.companyCalendarRepository.Create(companyCalendar);
            this.AddModelToGoogleCalendar(companyCalendar);

            this.unitOfWork.Complete();
            return this.mapper.Map<CreatedCompanyCalendarContract>(createdCompanyCalendar);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching company calendar {id}");
            CompanyCalendar companyCalendar = this.companyCalendarRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (companyCalendar == null)
            {
                throw new DeleteCompanyCalendarNotFoundException(id);
            }

            this.log.LogInformation($"Deleting company calendar {id}");
            this.companyCalendarRepository.Delete(companyCalendar);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateCompanyCalendarContract contract)
        {
            this.ValidateContract(contract);

            var companyCalendar = this.mapper.Map<CompanyCalendar>(contract);
            this.companyCalendarRepository.Update(companyCalendar);
            this.AddModelToGoogleCalendar(companyCalendar);

            this.unitOfWork.Complete();
        }

        public ReadedCompanyCalendarContract Read(int id)
        {
            var companyCalendarQuery = this.companyCalendarRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var companyCalendarResult = companyCalendarQuery.SingleOrDefault();

            return this.mapper.Map<ReadedCompanyCalendarContract>(companyCalendarResult);
        }

        private void ValidateContract(CreateCompanyCalendarContract contract)
        {
            try
            {
                this.createCompanyCalendarContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
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
                this.updateCompanyCalendarContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETDEFAULT}");
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
                    DateTime = new System.DateTime(companyCalendar.Date.Date.Year, companyCalendar.Date.Date.Month, companyCalendar.Date.Date.Day, 8, 0, 0),
                },
                End = new EventDateTime()
                {
                    DateTime = new System.DateTime(companyCalendar.Date.Date.Year, companyCalendar.Date.Date.Month, companyCalendar.Date.Date.Day, 19, 0, 0),
                },
            };
            this.googleCalendarService.CreateEvent(newEvent);
        }
    }
}
