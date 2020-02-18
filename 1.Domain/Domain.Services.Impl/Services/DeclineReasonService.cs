using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Skill;
using Domain.Model.Exceptions;
using Domain.Services.Contracts;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Services.Impl.Services
{
    public class DeclineReasonService : IDeclineReasonService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<DeclineReason> _declineReasonRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<DeclineReasonService> _log;
        private readonly UpdateDeclineReasonContractValidator _updateSkillContractValidator;
        private readonly CreateDeclineReasonContractValidator _createSkillContractValidator;

        public DeclineReasonService(
            IMapper mapper,
            IRepository<DeclineReason> declineReasonRepository,
            IUnitOfWork unitOfWork,
            ILog<DeclineReasonService> log,
            UpdateDeclineReasonContractValidator updateSkillContractValidator,
            CreateDeclineReasonContractValidator createSkillContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _declineReasonRepository = declineReasonRepository;
            _log = log;
            _updateSkillContractValidator = updateSkillContractValidator;
            _createSkillContractValidator = createSkillContractValidator;
        }

        public CreatedDeclineReasonContract Create(CreateDeclineReasonContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(0, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var declineReason = _mapper.Map<DeclineReason>(contract);

            var createdDeclineReason = _declineReasonRepository.Create(declineReason);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedDeclineReasonContract>(createdDeclineReason);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching skill {id}");
            DeclineReason declineReason = _declineReasonRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (declineReason == null)
            {
                throw new DeleteSkillNotFoundException(id);
            }
            _log.LogInformation($"Deleting skill {id}");
            _declineReasonRepository.Delete(declineReason);

            _unitOfWork.Complete();
        }

        public void Update(UpdateDeclineReasonContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(contract.Id, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var declineReason = _mapper.Map<DeclineReason>(contract);

            var updatedSkill = _declineReasonRepository.Update(declineReason);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }

        public IEnumerable<ReadedDeclineReasonContract> List()
        {
            var declineReasonQuery = _declineReasonRepository
                .QueryEager();

            var declineReasonResult = declineReasonQuery.ToList();

            return _mapper.Map<List<ReadedDeclineReasonContract>>(declineReasonResult);
        }

        public IEnumerable<ReadedDeclineReasonContract> ListNamed()
        {
            var declineReasonQuery = _declineReasonRepository
                .QueryEager();

            var declineReasonResult = declineReasonQuery.Where(d => !d.Name.Equals("Other"))
                .ToList();

            return _mapper.Map<List<ReadedDeclineReasonContract>>(declineReasonResult);
        }

        public ReadedDeclineReasonContract Read(int id)
        {
            var declineReasonQuery = _declineReasonRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var skillResult = declineReasonQuery.SingleOrDefault();

            return _mapper.Map<ReadedDeclineReasonContract>(skillResult);
        }

        private void ValidateContract(CreateDeclineReasonContract contract)
        {
            try
            {
                _createSkillContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateDeclineReasonContract contract)
        {
            try
            {
                _updateSkillContractValidator.ValidateAndThrow(contract,
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
                DeclineReason declineReason = _declineReasonRepository.Query().Where(_ => _.Name == name && _.Id != id).FirstOrDefault();
                if (declineReason != null) throw new InvalidDeclineReasonException("The DeclineReason already exists .");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
