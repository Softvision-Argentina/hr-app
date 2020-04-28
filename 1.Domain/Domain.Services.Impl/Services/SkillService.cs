using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Skill;
using Domain.Model.Exceptions.SkillType;
using Domain.Services.Contracts.Skill;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Skill;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class SkillService: ISkillService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Skill> _skillRepository;
        private readonly IRepository<SkillType> _skillTypesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<SkillService> _log;
        private readonly UpdateSkillContractValidator _updateSkillContractValidator;
        private readonly CreateSkillContractValidator _createSkillContractValidator;

        public SkillService(
            IMapper mapper,
            IRepository<Skill> skillRepository,
            IRepository<SkillType> skillTypesRepository,
            IUnitOfWork unitOfWork,
            ILog<SkillService> log,
            UpdateSkillContractValidator updateSkillContractValidator,
            CreateSkillContractValidator createSkillContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _skillRepository = skillRepository;
            _skillTypesRepository = skillTypesRepository;
            _log = log;
            _updateSkillContractValidator = updateSkillContractValidator;
            _createSkillContractValidator = createSkillContractValidator;
        }

        public CreatedSkillContract Create(CreateSkillContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(0, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var skill = _mapper.Map<Skill>(contract);

            this.AddTypeToSkill(skill, contract.Type);

            var createdSkill = _skillRepository.Create(skill);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedSkillContract>(createdSkill);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching skill {id}");
            Skill skill = _skillRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (skill == null)
            {
                throw new DeleteSkillNotFoundException(id);
            }
            _log.LogInformation($"Deleting skill {id}");
            _skillRepository.Delete(skill);

            _unitOfWork.Complete();
        }

        public void Update(UpdateSkillContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(contract.Id, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var skill = _mapper.Map<Skill>(contract);

            this.AddTypeToSkill(skill, contract.Type);

            _skillRepository.Update(skill);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }

        private void AddTypeToSkill(Skill skill, int typeId)
        {
            var type = _skillTypesRepository.Query().Where(_ => _.Id == typeId).FirstOrDefault();
            if (type == null)
                throw new SkillTypeNotFoundException(typeId);

            skill.Type = type;
        }

        public IEnumerable<ReadedSkillContract> List()
        {
            var skillQuery = _skillRepository
                .QueryEager();

            var skillResult = skillQuery.ToList();

            return _mapper.Map<List<ReadedSkillContract>>(skillResult);
        }

        public ReadedSkillContract Read(int id)
        {
            var skillQuery = _skillRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var skillResult = skillQuery.SingleOrDefault();

            return _mapper.Map<ReadedSkillContract>(skillResult);
        }

        private void ValidateContract(CreateSkillContract contract)
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

        private void ValidateContract(UpdateSkillContract contract)
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
                Skill skill = _skillRepository.Query().Where(_ => _.Name == name && _.Id != id).FirstOrDefault();
                if (skill != null) throw new InvalidSkillException("The skill already exists .");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
