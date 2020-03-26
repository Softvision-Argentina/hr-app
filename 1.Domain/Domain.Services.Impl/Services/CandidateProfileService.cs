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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Domain.Services.Impl.Services
{
    public class CandidateProfileService : ICandidateProfileService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CandidateProfile> _CandidateProfileRepository;
        private readonly IRepository<Model.Community> _communityItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<CandidateProfileService> _log;
        private readonly UpdateCandidateProfileContractValidator _updateCandidateProfileContractValidator;
        private readonly CreateCandidateProfileContractValidator _createCandidateProfileContractValidator;

        public CandidateProfileService(
            IMapper mapper,
            IRepository<CandidateProfile> CandidateProfileRepository,
            IRepository<Model.Community> communityItemRepository,
            IUnitOfWork unitOfWork,
            ILog<CandidateProfileService> log,
            UpdateCandidateProfileContractValidator updateCandidateProfileContractValidator,
            CreateCandidateProfileContractValidator createCandidateProfileContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _CandidateProfileRepository = CandidateProfileRepository;
            _communityItemRepository = communityItemRepository;
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
            var CandidateProfile = _mapper.Map<CandidateProfile>(contract);

            var createdCandidateProfile = _CandidateProfileRepository.Create(CandidateProfile);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedCandidateProfileContract>(createdCandidateProfile);
        }

        public void Delete(int Id)
        {
            _log.LogInformation($"Searching Candidate Profile {Id}");
            CandidateProfile CandidateProfile = _CandidateProfileRepository.Query().Where(_ => _.Id == Id).FirstOrDefault();

            if (CandidateProfile == null)
            {
                throw new DeleteCandidateProfileNotFoundException(Id);
            }
            _log.LogInformation($"Deleting Candidate Profile {Id}");
            _CandidateProfileRepository.Delete(CandidateProfile);

            _unitOfWork.Complete();
        }

        public void Update(UpdateCandidateProfileContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(contract.Id, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var CandidateProfile = _mapper.Map<CandidateProfile>(contract);


            var updatedCandidateProfile = _CandidateProfileRepository.Update(CandidateProfile);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }


        public IEnumerable<ReadedCandidateProfileContract> List()
        {
            var CandidateProfileQuery = _CandidateProfileRepository
                .QueryEager();                 
                
            var CandidateProfileResult = CandidateProfileQuery.ToList();

            return _mapper.Map<List<ReadedCandidateProfileContract>>(CandidateProfileResult);
        }

        public ReadedCandidateProfileContract Read(int Id)
        {
            var CandidateProfileQuery = _CandidateProfileRepository
                .QueryEager()                
                .Where(_ => _.Id == Id);

            var CandidateProfileResult = CandidateProfileQuery.SingleOrDefault();

            return _mapper.Map<ReadedCandidateProfileContract>(CandidateProfileResult);
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

        private void ValidateExistence(int Id, string name)
        {
            try
            {
                CandidateProfile CandidateProfile = _CandidateProfileRepository.Query().Where(_ => _.Name == name && _.Id != Id).FirstOrDefault();
                if (CandidateProfile != null) throw new InvalidCandidateProfileException("The Profile already exists .");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
