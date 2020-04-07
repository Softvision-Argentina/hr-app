using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Role;
using Domain.Services.Contracts.Role;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Role;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class RoleService: IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<SkillTypeService> _log;
        private readonly UpdateRoleContractValidator _updateRoleContractValidator;
        private readonly CreateRoleContractValidator _createRoleContractValidator;

        public RoleService(
            IMapper mapper,
            IRepository<Role> roleRepository,
            IUnitOfWork unitOfWork,
            ILog<SkillTypeService> log,
            UpdateRoleContractValidator updateRoleContractValidator,
            CreateRoleContractValidator createRoleContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _log = log;
            _updateRoleContractValidator = updateRoleContractValidator;
            _createRoleContractValidator = createRoleContractValidator;
        }

        public CreatedRoleContract Create(CreateRoleContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var role = _mapper.Map<Role>(contract);

            var createdRole = _roleRepository.Create(role);

            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();

            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedRoleContract>(createdRole);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching role {id}");
            Role role = _roleRepository.Query().Where(r => r.Id == id).FirstOrDefault();

            if (role == null)
            {
                throw new DeleteRoleNotFoundException(id);
            }
            _log.LogInformation($"Deleting role {id}");
            _roleRepository.Delete(role);

            _unitOfWork.Complete();
        }

        public IEnumerable<ReadedRoleContract> List()
        {
            var RoleQuery = _roleRepository.QueryEager();
            var RoleList = RoleQuery.ToList();
            return _mapper.Map<List<ReadedRoleContract>>(RoleList);
        }

        public ReadedRoleContract Read(int Id)
        {
            var roleQuery = _roleRepository.QueryEager().Where(_ => _.Id == Id);

            var roleResult = roleQuery.SingleOrDefault();

            return _mapper.Map<ReadedRoleContract>(roleResult);
        }

        public void Update(UpdateRoleContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var role = _mapper.Map<Role>(contract);


            var updatedOffice = _roleRepository.Update(role);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }

        private void ValidateContract(CreateRoleContract contract)
        {
            try
            {
                _createRoleContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateRoleContract contract)
        {
            try
            {
                _updateRoleContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
