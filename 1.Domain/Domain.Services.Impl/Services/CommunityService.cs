using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Community;
using Domain.Services.Contracts.Community;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Community;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class CommunityService: ICommunityService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Community> _CommunityRepository;
        private readonly IRepository<CandidateProfile> _CandidateProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<CommunityService> _log;
        private readonly UpdateCommunityContractValidator _updateCommunityContractValidator;
        private readonly CreateCommunityContractValidator _createCommunityContractValidator;

        public CommunityService(IMapper mapper,
            IRepository<Community> CommunityRepository,
            IRepository<CandidateProfile> CandidateProfileRepository,
            IUnitOfWork unitOfWork,
            ILog<CommunityService> log,
            UpdateCommunityContractValidator updateCommunityContractValidator,
            CreateCommunityContractValidator createCommunityContractValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _CommunityRepository = CommunityRepository;
            _log = log;
            _updateCommunityContractValidator = updateCommunityContractValidator;
            _createCommunityContractValidator = createCommunityContractValidator;
            _CandidateProfileRepository = CandidateProfileRepository;
        }

        public CreatedCommunityContract Create(CreateCommunityContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var Community = _mapper.Map<Community>(contract);

            Community.Profile = _CandidateProfileRepository.Query().Where(x => x.Id == Community.ProfileId).FirstOrDefault();

            var createdCommunity = _CommunityRepository.Create(Community);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedCommunityContract>(createdCommunity);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching Community {id}");
            Community Community = _CommunityRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (Community == null)
            {
                throw new DeleteCommunityNotFoundException(id);
            }
            _log.LogInformation($"Deleting Community {id}");
            _CommunityRepository.Delete(Community);

            _unitOfWork.Complete();
        }

        public void Update(UpdateCommunityContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);


            _log.LogInformation($"Mapping contract {contract.Name}");
            var Community = _mapper.Map<Community>(contract);

            var updatedCommunity = _CommunityRepository.Update(Community);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }
        
        public ReadedCommunityContract Read(int id)
        {
            var CommunityQuery = _CommunityRepository
                .Query()
                .Where(_ => _.Id == id)
                .OrderBy(_ => _.Name);

            var CommunityResult = CommunityQuery.SingleOrDefault();

            return _mapper.Map<ReadedCommunityContract>(CommunityResult);
        }
        
        public IEnumerable<ReadedCommunityContract> List()
        {
            var CommunityQuery = _CommunityRepository
                .Query()
                .OrderBy(_ => _.Name);

            var CommunityResult = CommunityQuery.ToList();

            return _mapper.Map<List<ReadedCommunityContract>>(CommunityResult);
        }

        private void ValidateContract(CreateCommunityContract contract)
        {
            try
            {
                _createCommunityContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateCommunityContract contract)
        {
            try
            {
                _updateCommunityContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
