﻿// <copyright file="CandidateService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Core.ExtensionHelpers;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Model.Exceptions.Candidate;
    using Domain.Services.Contracts.Candidate;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.Candidate;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class CandidateService : ICandidateService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Process> processRepository;
        private readonly IRepository<Candidate> candidateRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Office> officeRepository;
        private readonly IRepository<Community> communityRepository;
        private readonly IRepository<CandidateProfile> candidateProfileRepository;
        private readonly IRepository<OpenPosition> openPositionRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<CandidateService> log;
        private readonly UpdateCandidateContractValidator updateCandidateContractValidator;
        private readonly CreateCandidateContractValidator createCandidateContractValidator;

        public CandidateService(
            IMapper mapper,
            IRepository<Candidate> candidateRepository,
            IRepository<Community> communityRepository,
            IRepository<CandidateProfile> candidateProfileRepository,
            IRepository<User> userRepository,
            IRepository<Office> officeRepository,
            IRepository<Process> processRepository,
            IRepository<OpenPosition> openPositionRepository,
            IUnitOfWork unitOfWork,
            ILog<CandidateService> log,
            UpdateCandidateContractValidator updateCandidateContractValidator,
            CreateCandidateContractValidator createCandidateContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.processRepository = processRepository;
            this.candidateRepository = candidateRepository;
            this.userRepository = userRepository;
            this.officeRepository = officeRepository;
            this.communityRepository = communityRepository;
            this.candidateProfileRepository = candidateProfileRepository;
            this.openPositionRepository = openPositionRepository;
            this.log = log;
            this.updateCandidateContractValidator = updateCandidateContractValidator;
            this.createCandidateContractValidator = createCandidateContractValidator;
        }

        public CreatedCandidateContract Create(CreateCandidateContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(contract.EmailAddress, contract.PhoneNumber);

            this.log.LogInformation($"Mapping contract {contract.Name}");

            var candidate = this.mapper.Map<Candidate>(contract);

            if (candidate.LinkedInProfile != null)
            {
                candidate.LinkedInProfile = RegexExtensions.GetLinkedInUsername(candidate.LinkedInProfile);
            }

            if (contract.User != null)
            {
                this.AddUserToCandidate(candidate, contract.User.Id);
            }

            this.AddCommunityToCandidate(candidate, contract.Community.Id);

            if (contract.Profile != null)
            {
                this.AddCandidateProfileToCandidate(candidate, contract.Profile.Id);
            }

            if (contract.OpenPosition != null)
            {
                this.AddOpenPositionForCandidate(candidate, contract.OpenPosition.Id);
            }

            var createdCandidate = this.candidateRepository.Create(candidate);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Name}");
            var date = DateTime.UtcNow;
            createdCandidate.CreatedDate = date;
            return this.mapper.Map<CreatedCandidateContract>(createdCandidate);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching candidate {id}");
            var candidate = this.candidateRepository.Query().FirstOrDefault(_ => _.Id == id);

            if (candidate == null)
            {
                throw new DeleteCandidateNotFoundException(id);
            }

            this.log.LogInformation($"Deleting candidate {id}");
            this.candidateRepository.Delete(candidate);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateCandidateContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Name}");

            var candidate = this.mapper.Map<Candidate>(contract);

            if (candidate.LinkedInProfile != null)
            {
                candidate.LinkedInProfile = RegexExtensions.GetLinkedInUsername(candidate.LinkedInProfile);
            }

            var currentProcesses = this.processRepository.Query().Where(p => p.CandidateId == candidate.Id && p.Status != Model.Enum.ProcessStatus.Hired);

            foreach (var process in currentProcesses)
            {
                process.Status = Model.Enum.ProcessStatus.Recall;
                this.processRepository.Update(process);
            }

            if (contract.User != null)
            {
                this.AddUserToCandidate(candidate, contract.User.Id);
            }

            this.AddOfficeToCandidate(candidate, contract.PreferredOfficeId);
            this.AddCommunityToCandidate(candidate, contract.Community.Id);

            if (contract.Profile != null)
            {
                this.AddCandidateProfileToCandidate(candidate, contract.Profile.Id);
            }

            var updatedCandidate = this.candidateRepository.Update(candidate);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        public ReadedCandidateContract Read(int id)
        {
            var candidateQuery = this.candidateRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var candidateResult = candidateQuery.SingleOrDefault();

            return this.mapper.Map<ReadedCandidateContract>(candidateResult);
        }

        public IEnumerable<ReadedCandidateContract> Read(Func<Candidate, bool> filterRule)
        {
            var candidateQuery = this.candidateRepository
                .QueryEager()
                .Where(filterRule);

            var candidateResult = candidateQuery.ToList();

            return this.mapper.Map<List<ReadedCandidateContract>>(candidateResult);
        }

        public ReadedCandidateContract Exists(int id)
        {
            var candidateQuery = this.candidateRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var candidateResult = candidateQuery.SingleOrDefault();

            return this.mapper.Map<ReadedCandidateContract>(candidateResult);
        }

        public bool Exists(string email)
        {
            if (this.candidateRepository.QueryEager().FirstOrDefault(c => c.EmailAddress == email) == null)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<ReadedCandidateContract> List()
        {
            var candidateQuery = this.candidateRepository
                .QueryEager();

            var candidateResult = candidateQuery.ToList();

            return this.mapper.Map<List<ReadedCandidateContract>>(candidateResult);
        }

        public IEnumerable<ReadedCandidateAppContract> ListApp()
        {
            var candidateQuery = this.candidateRepository
                .QueryEager();

            var candidateResult = candidateQuery.ToList();
            return this.mapper.Map<List<ReadedCandidateAppContract>>(candidateResult);
        }

        public Candidate GetCandidate(int id)
        {
            var candidateQuery = this.candidateRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var candidateResult = candidateQuery.SingleOrDefault();

            return candidateResult;
        }

        private void ValidateContract(CreateCandidateContract contract)
        {
            try
            {
                this.createCandidateContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateExistence(string email, string phoneNumber)
        {
            Candidate candidate;

            void ExistEmail()
            {
                candidate = this.candidateRepository.Query()
                    .Where(_ => email != null)
                    .FirstOrDefault(_ => _.EmailAddress == email);

                if (candidate != null)
                {
                    throw new InvalidCandidateException("Email address already exists");
                }
            }

            void ExistPhoneNumber()
            {
                candidate = this.candidateRepository.Query()
                    .Where(_ => phoneNumber != null)
                    .FirstOrDefault(_ => _.PhoneNumber == phoneNumber);

                if (candidate != null && candidate.PhoneNumber != "(+54)")
                {
                    throw new InvalidCandidateException("Phone number already exists");
                }
            }

            try
            {
                ExistEmail();
                ExistPhoneNumber();
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateCandidateContract contract)
        {
            try
            {
                this.updateCandidateContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETDEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void AddUserToCandidate(Candidate candidate, int userId)
        {
            var user = this.userRepository.Query().Where(_ => _.Id == userId).FirstOrDefault();
            if (user == null)
            {
                throw new Domain.Model.Exceptions.User.UserNotFoundException(userId);
            }

            candidate.User = user;
        }

        private void AddCommunityToCandidate(Candidate candidate, int communityId)
        {
            var community = this.communityRepository.Query().Where(_ => _.Id == communityId).FirstOrDefault();
            if (community == null)
            {
                throw new Domain.Model.Exceptions.Community.CommunityNotFoundException(communityId);
            }

            candidate.Community = community;
        }

        private void AddCandidateProfileToCandidate(Candidate candidate, int profileId)
        {
            var profile = this.candidateProfileRepository.Query().Where(_ => _.Id == profileId).FirstOrDefault();
            if (profile == null)
            {
                throw new Domain.Model.Exceptions.CandidateProfile.CandidateProfileNotFoundException(profileId);
            }

            candidate.Profile = profile;
        }

        private void AddOfficeToCandidate(Candidate candidate, int officeId)
        {
            var office = this.officeRepository.Query().Where(_ => _.Id == officeId).FirstOrDefault();
            if (office == null)
            {
                throw new Domain.Model.Exceptions.Office.OfficeNotFoundException(officeId);
            }

            candidate.PreferredOffice = office;
        }

        private void AddOpenPositionForCandidate(Candidate candidate, int id)
        {
            var position = this.openPositionRepository.Query().Where(_ => _.Id == id).FirstOrDefault();
            if (position == null)
            {
                throw new Exception("Postion for the Candidate not found");
            }

            candidate.OpenPosition = position;
            candidate.PositionTitle = position.Title;
        }
    }
}
