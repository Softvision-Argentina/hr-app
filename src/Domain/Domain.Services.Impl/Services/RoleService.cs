// <copyright file="RoleService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System.Collections.Generic;
    using System.Linq;
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

    public class RoleService : IRoleService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Role> roleRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<SkillTypeService> log;
        private readonly UpdateRoleContractValidator updateRoleContractValidator;
        private readonly CreateRoleContractValidator createRoleContractValidator;

        public RoleService(
            IMapper mapper,
            IRepository<Role> roleRepository,
            IUnitOfWork unitOfWork,
            ILog<SkillTypeService> log,
            UpdateRoleContractValidator updateRoleContractValidator,
            CreateRoleContractValidator createRoleContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.roleRepository = roleRepository;
            this.log = log;
            this.updateRoleContractValidator = updateRoleContractValidator;
            this.createRoleContractValidator = createRoleContractValidator;
        }

        public CreatedRoleContract Create(CreateRoleContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var role = this.mapper.Map<Role>(contract);

            var createdRole = this.roleRepository.Create(role);

            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();

            this.log.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedRoleContract>(createdRole);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching role {id}");
            Role role = this.roleRepository.Query().Where(r => r.Id == id).FirstOrDefault();

            if (role == null)
            {
                throw new DeleteRoleNotFoundException(id);
            }

            this.log.LogInformation($"Deleting role {id}");
            this.roleRepository.Delete(role);

            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedRoleContract> List()
        {
            var roleQuery = this.roleRepository.QueryEager();
            var roleList = roleQuery.ToList();
            return this.mapper.Map<List<ReadedRoleContract>>(roleList);
        }

        public ReadedRoleContract Read(int id)
        {
            var roleQuery = this.roleRepository.QueryEager().Where(_ => _.Id == id);

            var roleResult = roleQuery.SingleOrDefault();

            return this.mapper.Map<ReadedRoleContract>(roleResult);
        }

        public void Update(UpdateRoleContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var role = this.mapper.Map<Role>(contract);

            var updatedOffice = this.roleRepository.Update(role);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        private void ValidateContract(CreateRoleContract contract)
        {
            try
            {
                this.createRoleContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
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
                this.updateRoleContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETDEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
