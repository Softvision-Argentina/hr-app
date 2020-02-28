using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Candidate;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Candidate;
using Domain.Services.Interfaces.Services;
using Domain.Services.Repositories.EF;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Services.Impl.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Process> _processRepository;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IRepository<Consultant> _consultantRepository;
        private readonly IRepository<Office> _officeRepository;
        private readonly IRepository<Community> _communityRepository;
        private readonly IRepository<CandidateProfile> _candidateProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<CandidateService> _log;
        private readonly UpdateCandidateContractValidator _updateCandidateContractValidator;
        private readonly CreateCandidateContractValidator _createCandidateContractValidator;
        private readonly DataBaseContext _context;

        public CandidateService(IMapper mapper,
            IRepository<Candidate> candidateRepository,
            IRepository<Community> communityRepository,
            IRepository<CandidateProfile> candidateProfileRepository,
            IRepository<Consultant> consultantRepository,
            IRepository<Office> officeRepository,
            IRepository<Process> processRepository,
            IUnitOfWork unitOfWork,
            ILog<CandidateService> log,
            UpdateCandidateContractValidator updateCandidateContractValidator,
            CreateCandidateContractValidator createCandidateContractValidator,
             DataBaseContext context)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _processRepository = processRepository;
            _candidateRepository = candidateRepository;
            _consultantRepository = consultantRepository;
            _officeRepository = officeRepository;
            _communityRepository = communityRepository;
            _candidateProfileRepository = candidateProfileRepository;
            _log = log;
            _updateCandidateContractValidator = updateCandidateContractValidator;
            _createCandidateContractValidator = createCandidateContractValidator;
            _context = context;
        }

        public CreatedCandidateContract Create(CreateCandidateContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(0, contract.EmailAddress, contract.LinkedInProfile);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var candidate = _mapper.Map<Candidate>(contract);

            this.AddRecruiterToCandidate(candidate, contract.Recruiter.Id);
            this.AddCommunityToCandidate(candidate, contract.Community.Id);
            this.AddCandidateProfileToCandidate(candidate, contract.Profile.Id);
            //this.AddOfficeToCandidate(candidate, contract.PreferredOfficeId);

            var createdCandidate = _candidateRepository.Create(candidate);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedCandidateContract>(createdCandidate);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching candidate {id}");
            Candidate candidate = _candidateRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

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

            this.AddRecruiterToCandidate(candidate, contract.Recruiter.Id);
            this.AddOfficeToCandidate(candidate, contract.PreferredOfficeId);
            this.AddCommunityToCandidate(candidate, contract.Community.Id);
            this.AddCandidateProfileToCandidate(candidate, contract.Profile.Id);

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

        //public ReadedCandidateContract Exists(int dni)
        //{
        //    var candidateQuery = _candidateRepository
        //        .QueryEager()
        //        .Where(_ => _.DNI == dni);

        //    var candidateResult = candidateQuery.SingleOrDefault();

        //    return _mapper.Map<ReadedCandidateContract>(candidateResult);
        //}
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
            var users = _context.Candidates.Find(id);

            return users;
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

        private void ValidateExistence(int id, string email, string linkedInProfile)
        {
            try
            {
                Candidate candidate = _candidateRepository.Query().Where(_ => !string.IsNullOrEmpty(email) && _.EmailAddress == email && _.Id != id).FirstOrDefault();
                if (candidate != null) throw new InvalidCandidateException("The Email already exists .");
            }
            catch (ValidationException ex)
            {
                    throw new CreateContractInvalidException(ex.ToListOfMessages());
            }

            try
            {
                if(linkedInProfile != null)
                {
                    Candidate candidate = _candidateRepository.Query().Where(_ => linkedInProfile != "N/A" && _.LinkedInProfile == linkedInProfile && _.Id != id).FirstOrDefault();
                    if (candidate != null) throw new InvalidCandidateException("The LinkedIn Profile already exists in our database.");
                }
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

        private void AddRecruiterToCandidate(Candidate candidate, int recruiterID)
        {
            var recruiter = _consultantRepository.Query().Where(_ => _.Id == recruiterID).FirstOrDefault();
            if (recruiter == null)
                throw new Domain.Model.Exceptions.Consultant.ConsultantNotFoundException(recruiterID);

            candidate.Recruiter = recruiter;
        }
        private void AddCommunityToCandidate(Candidate candidate, int communityID)
        {
            var community = _communityRepository.Query().Where(_ => _.Id == communityID).FirstOrDefault();
            if (community == null)
                throw new Domain.Model.Exceptions.Community.CommunityNotFoundException(communityID);

            candidate.Community = community;
        }
        private void AddCandidateProfileToCandidate(Candidate candidate, int profileID)
        {
            var profile = _candidateProfileRepository.Query().Where(_ => _.Id == profileID).FirstOrDefault();
            if (profile == null)
                throw new Domain.Model.Exceptions.CandidateProfile.CandidateProfileNotFoundException(profileID);

            candidate.Profile = profile;
        }
        private void AddOfficeToCandidate(Candidate candidate, int officeId)
        {
            var office = _officeRepository.Query().Where(_ => _.Id == officeId).FirstOrDefault();
            if (office == null)
                throw new Domain.Model.Exceptions.Office.OfficeNotFoundException(officeId);

            candidate.PreferredOffice = office;
        }

       
    }
}
