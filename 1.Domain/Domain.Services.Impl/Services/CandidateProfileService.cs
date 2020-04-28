using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.CandidateProfile;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.CandidateProfile;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class CandidateProfileService : ICandidateProfileService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CandidateProfile> _candidateProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<CandidateProfileService> _log;
        private readonly UpdateCandidateProfileContractValidator _updateCandidateProfileContractValidator;
        private readonly CreateCandidateProfileContractValidator _createCandidateProfileContractValidator;

        public CandidateProfileService(
            IMapper mapper,
            IRepository<CandidateProfile> candidateProfileRepository,
            IUnitOfWork unitOfWork,
            ILog<CandidateProfileService> log,
            UpdateCandidateProfileContractValidator updateCandidateProfileContractValidator,
            CreateCandidateProfileContractValidator createCandidateProfileContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _candidateProfileRepository = candidateProfileRepository;
            _log = log;
            _updateCandidateProfileContractValidator = updateCandidateProfileContractValidator;
            _createCandidateProfileContractValidator = createCandidateProfileContractValidator;
        }

        public CreatedCandidateProfileContract Create(CreateCandidateProfileContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(0, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var candidateProfile = _mapper.Map<CandidateProfile>(contract);

            var createdCandidateProfile = _candidateProfileRepository.Create(candidateProfile);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedCandidateProfileContract>(createdCandidateProfile);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching Candidate Profile {id}");
            var candidateProfile = _candidateProfileRepository.Query().FirstOrDefault(_ => _.Id == id);

            if (candidateProfile == null)
            {
                throw new DeleteCandidateProfileNotFoundException(id);
            }
            _log.LogInformation($"Deleting Candidate Profile {id}");
            _candidateProfileRepository.Delete(candidateProfile);

            _unitOfWork.Complete();
        }

        public void Update(UpdateCandidateProfileContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(contract.Id, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var candidateProfile = _mapper.Map<CandidateProfile>(contract);

            _candidateProfileRepository.Update(candidateProfile);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }


        public IEnumerable<ReadedCandidateProfileContract> List()
        {
            var candidateProfileQuery = _candidateProfileRepository
                .QueryEager();                 
                
            var candidateProfileResult = candidateProfileQuery.ToList();

            return _mapper.Map<List<ReadedCandidateProfileContract>>(candidateProfileResult);
        }

        public ReadedCandidateProfileContract Read(int id)
        {
            var candidateProfileQuery = _candidateProfileRepository
                .QueryEager()                
                .Where(_ => _.Id == id);

            var candidateProfileResult = candidateProfileQuery.SingleOrDefault();

            return _mapper.Map<ReadedCandidateProfileContract>(candidateProfileResult);
        }

        private void ValidateContract(CreateCandidateProfileContract contract)
        {
            try
            {
                _createCandidateProfileContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateCandidateProfileContract contract)
        {
            try
            {
                _updateCandidateProfileContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateExistence(int id, string name)
        {
            try
            {
                var candidateProfile = _candidateProfileRepository.Query().AsNoTracking().FirstOrDefault(_ => _.Name == name && _.Id != id);
                if (candidateProfile != null) throw new InvalidCandidateProfileException("The Profile already exists .");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
