using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Skill;
using Domain.Model.Exceptions.SkillType;
using Domain.Services.Contracts.SkillType;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.SkillType;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class SkillTypeService: ISkillTypeService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<SkillType> _skillTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<SkillTypeService> _log;
        private readonly UpdateSkillTypeContractValidator _updateSkillContractValidator;
        private readonly CreateSkillTypeContractValidator _createSkillContractValidator;

        public SkillTypeService(
            IMapper mapper,
            IRepository<SkillType> skillTypeRepository,
            IUnitOfWork unitOfWork,
            ILog<SkillTypeService> log,
            UpdateSkillTypeContractValidator updateSkillContractValidator,
            CreateSkillTypeContractValidator createSkillContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _skillTypeRepository = skillTypeRepository;
            _log = log;
            _updateSkillContractValidator = updateSkillContractValidator;
            _createSkillContractValidator = createSkillContractValidator;
        }

        public CreatedSkillTypeContract Create(CreateSkillTypeContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(0, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var skillType = _mapper.Map<SkillType>(contract);

            var createdSkillType = _skillTypeRepository.Create(skillType);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedSkillTypeContract>(createdSkillType);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching skill {id}");
            SkillType skillType = _skillTypeRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (skillType == null)
            {
                throw new DeleteSkillNotFoundException(id);
            }
            _log.LogInformation($"Deleting skill {id}");
            _skillTypeRepository.Delete(skillType);

            _unitOfWork.Complete();
        }

        public void Update(UpdateSkillTypeContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(contract.Id, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var skillType = _mapper.Map<SkillType>(contract);

            _skillTypeRepository.Update(skillType);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }

        public IEnumerable<ReadedSkillTypeContract> List()
        {
            var skillTypeQuery = _skillTypeRepository
                .QueryEager();

            var skillTypeResult = skillTypeQuery.ToList();

            return _mapper.Map<List<ReadedSkillTypeContract>>(skillTypeResult);
        }

        public ReadedSkillTypeContract Read(int id)
        {
            var skillTypeQuery = _skillTypeRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var skillResult = skillTypeQuery.SingleOrDefault();

            return _mapper.Map<ReadedSkillTypeContract>(skillResult);
        }

        private void ValidateContract(CreateSkillTypeContract contract)
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

        private void ValidateContract(UpdateSkillTypeContract contract)
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
                SkillType skillType = _skillTypeRepository.Query().Where(_ => _.Name == name && _.Id != id).FirstOrDefault();
                if (skillType != null) throw new InvalidSkillTypeException("The SkillType already exists .");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
