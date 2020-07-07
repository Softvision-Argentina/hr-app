using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Candidate;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Candidate;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Process> _processRepository;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Office> _officeRepository;
        private readonly IRepository<Community> _communityRepository;
        private readonly IRepository<CandidateProfile> _candidateProfileRepository;
        private readonly IRepository<OpenPosition> _openPositionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<CandidateService> _log;
        private readonly UpdateCandidateContractValidator _updateCandidateContractValidator;
        private readonly CreateCandidateContractValidator _createCandidateContractValidator;

        public CandidateService(IMapper mapper,
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
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _processRepository = processRepository;
            _candidateRepository = candidateRepository;
            _userRepository = userRepository;
            _officeRepository = officeRepository;
            _communityRepository = communityRepository;
            _candidateProfileRepository = candidateProfileRepository;
            _openPositionRepository = openPositionRepository;
            _log = log;
            _updateCandidateContractValidator = updateCandidateContractValidator;
            _createCandidateContractValidator = createCandidateContractValidator;
        }

        public CreatedCandidateContract Create(CreateCandidateContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(contract.EmailAddress, contract.PhoneNumber);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var candidate = _mapper.Map<Candidate>(contract);

            if (contract.User != null)
            {
                this.AddUserToCandidate(candidate, contract.User.Id);
            }
            this.AddCommunityToCandidate(candidate, contract.Community.Id);

            if (contract.Profile != null)
            {
                this.AddCandidateProfileToCandidate(candidate, contract.Profile.Id);
            }
            if(contract.OpenPosition != null)
            {
                this.AddOpenPositionForCandidate(candidate, contract.OpenPosition.Id);
            }

            
            var createdCandidate = _candidateRepository.Create(candidate);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            var date = DateTime.UtcNow;
            createdCandidate.CreatedDate = date;
            return _mapper.Map<CreatedCandidateContract>(createdCandidate);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching candidate {id}");
            var candidate = _candidateRepository.Query().FirstOrDefault(_ => _.Id == id);

            if (candidate == null)
            {
                throw new DeleteCandidateNotFoundException(id);
            }
            _log.LogInformation($"Deleting candidate {id}");
            _candidateRepository.Delete(candidate);

            _unitOfWork.Complete();
        }

        public void Update(UpdateCandidateContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var candidate = _mapper.Map<Candidate>(contract);

            var currentProcesses = _processRepository.Query().Where(p => p.CandidateId == candidate.Id && p.Status != Model.Enum.ProcessStatus.Hired);

            foreach (var process in currentProcesses)
            {
                process.Status = Model.Enum.ProcessStatus.Recall;
                _processRepository.Update(process);
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

            var updatedCandidate = _candidateRepository.Update(candidate);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }

        public ReadedCandidateContract Read(int id)
        {
            var candidateQuery = _candidateRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var candidateResult = candidateQuery.SingleOrDefault();

            return _mapper.Map<ReadedCandidateContract>(candidateResult);
        }

        public IEnumerable<ReadedCandidateContract> Read(Func<Candidate, bool> filterRule)
        {

            var candidateQuery = _candidateRepository
                .QueryEager()
                .Where(filterRule);


            var candidateResult = candidateQuery.ToList();

            return _mapper.Map<List<ReadedCandidateContract>>(candidateResult);
        }

        public ReadedCandidateContract Exists(int id)
        {
            var candidateQuery = _candidateRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var candidateResult = candidateQuery.SingleOrDefault();

            return _mapper.Map<ReadedCandidateContract>(candidateResult);
        }

        public IEnumerable<ReadedCandidateContract> List()
        {
            var candidateQuery = _candidateRepository
                .QueryEager();

            var candidateResult = candidateQuery.ToList();

            return _mapper.Map<List<ReadedCandidateContract>>(candidateResult);
        }

        public IEnumerable<ReadedCandidateAppContract> ListApp()
        {
            var candidateQuery = _candidateRepository
                .QueryEager();

            var candidateResult = candidateQuery.ToList();
            return _mapper.Map<List<ReadedCandidateAppContract>>(candidateResult);
        }

        public Candidate GetCandidate(int id)
        {
            var candidateQuery = _candidateRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var candidateResult = candidateQuery.SingleOrDefault();

            return candidateResult;
        }

        private void ValidateContract(CreateCandidateContract contract)
        {
            try
            {
                _createCandidateContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
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
                candidate = _candidateRepository.Query()
                    .Where(_ => email != null)
                    .FirstOrDefault(_ => _.EmailAddress == email);

                if (candidate != null)
                    throw new InvalidCandidateException("Email address already exists");
            }

            void ExistPhoneNumber()
            {
                candidate = _candidateRepository.Query()
                    .Where(_ => phoneNumber != null)
                    .FirstOrDefault(_ => _.PhoneNumber == phoneNumber);

                if (candidate != null && candidate.PhoneNumber != "(+54)")
                    throw new InvalidCandidateException("Phone number already exists");
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
                _updateCandidateContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void AddUserToCandidate(Candidate candidate, int userId)
        {
            var user = _userRepository.Query().Where(_ => _.Id == userId).FirstOrDefault();
            if (user == null)
                throw new Domain.Model.Exceptions.User.UserNotFoundException(userId);

            candidate.User = user;
        }

        private void AddCommunityToCandidate(Candidate candidate, int communityId)
        {
            var community = _communityRepository.Query().Where(_ => _.Id == communityId).FirstOrDefault();
            if (community == null)
                throw new Domain.Model.Exceptions.Community.CommunityNotFoundException(communityId);

            candidate.Community = community;
        }

        private void AddCandidateProfileToCandidate(Candidate candidate, int profileId)
        {
            var profile = _candidateProfileRepository.Query().Where(_ => _.Id == profileId).FirstOrDefault();
            if (profile == null)
                throw new Domain.Model.Exceptions.CandidateProfile.CandidateProfileNotFoundException(profileId);

            candidate.Profile = profile;
        }

        private void AddOfficeToCandidate(Candidate candidate, int officeId)
        {
            var office = _officeRepository.Query().Where(_ => _.Id == officeId).FirstOrDefault();
            if (office == null)
                throw new Domain.Model.Exceptions.Office.OfficeNotFoundException(officeId);

            candidate.PreferredOffice = office;
        }

        private void AddOpenPositionForCandidate(Candidate candidate, int id)
        {
            var position = _openPositionRepository.Query().Where(_ => _.Id == id).FirstOrDefault();
            if (position == null)
                throw new Exception("Postion for the Candidate not found");

            candidate.OpenPosition = position;
            candidate.PositionTitle = position.Title;
        }
    }
}
