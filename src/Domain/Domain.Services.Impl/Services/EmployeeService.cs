// <copyright file="EmployeeService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.Employee;
    using Domain.Services.Contracts.Employee;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.Employee;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Employee> employeeRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<EmployeeService> logger;
        private readonly UpdateEmployeeContractValidator updateEmployeeContractValidator;
        private readonly CreateEmployeeContractValidator createEmployeeContractValidator;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Role> roleRepository;

        public EmployeeService(
            IMapper mapper,
            IRepository<Employee> employeeRepository,
            IUnitOfWork unitOfWork,
            ILog<EmployeeService> log,
            UpdateEmployeeContractValidator updateEmployeeContractValidator,
            CreateEmployeeContractValidator createEmployeeContractValidator,
            IRepository<User> userRepository,
            IRepository<Role> roleRepository)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.employeeRepository = employeeRepository;
            this.logger = log;
            this.updateEmployeeContractValidator = updateEmployeeContractValidator;
            this.createEmployeeContractValidator = createEmployeeContractValidator;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }

        public IEnumerable<ReadedEmployeeContract> List()
        {
            var employeeQuery = this.employeeRepository.QueryEager();

            var employeeResult = employeeQuery.ToList();

            return this.mapper.Map<List<ReadedEmployeeContract>>(employeeResult);
        }

        public void Delete(int id)
        {
            this.logger.LogInformation($"Searching employee {id}");
            Employee employee = this.employeeRepository.Query().Where(emp => emp.Id == id).FirstOrDefault();

            if (employee == null)
            {
                throw new DeleteEmployeeNotFoundException(id);
            }

            this.logger.LogInformation($"Deleting employee {id}");
            this.employeeRepository.Delete(employee);

            this.unitOfWork.Complete();
        }

        public Employee GetByDNI(int dni)
        {
            Employee employee = this.employeeRepository.Query().Where(emp => emp.DNI == dni).FirstOrDefault();
            return this.mapper.Map<Employee>(employee);
        }

        public Employee GetByEmail(string email)
        {
            Employee employee = this.employeeRepository.Query().Where(emp => emp.EmailAddress == email).FirstOrDefault();
            return this.mapper.Map<Employee>(employee);
        }

        public CreatedEmployeeContract Create(CreateEmployeeContract contract)
        {
            this.logger.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateEmailExistence(0, contract.EmailAddress);
            this.ValidateDniExistence(0, contract.DNI);

            this.logger.LogInformation($"Mapping contract {contract.Name}");
            var employee = this.mapper.Map<Employee>(contract);

            this.AddUserToEmployee(employee, contract.UserId);
            this.AddRoleToEmployee(employee, contract.RoleId);
            if (contract.ReviewerId != null)
            {
                this.AddReviewerToEmployee(employee, contract.ReviewerId);
            }

            var createdEmployee = this.employeeRepository.Create(employee);
            this.logger.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.logger.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedEmployeeContract>(createdEmployee);
        }

        public void UpdateEmployee(UpdateEmployeeContract contract)
        {
            this.logger.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateEmailExistence(contract.Id, contract.EmailAddress);
            this.ValidateDniExistence(contract.Id, contract.DNI);

            this.logger.LogInformation($"Mapping contract {contract.Name}");
            var employee = this.mapper.Map<Employee>(contract);

            this.AddUserToEmployee(employee, contract.UserId);
            this.AddRoleToEmployee(employee, contract.RoleId);
            if (contract.ReviewerId != null)
            {
                this.AddReviewerToEmployee(employee, contract.ReviewerId);
            }

            this.employeeRepository.Update(employee);

            this.logger.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        private void AddUserToEmployee(Employee employee, int userId)
        {
            var user = this.userRepository.Query().Where(usr => usr.Id == userId).FirstOrDefault();
            employee.User = user ?? throw new Domain.Model.Exceptions.User.UserNotFoundException(userId);
        }

        private void AddRoleToEmployee(Employee employee, int roleId)
        {
            var role = this.roleRepository.Query().Where(r => r.Id == roleId).FirstOrDefault();
            employee.Role = role ?? throw new Domain.Model.Exceptions.Role.RoleNotFoundException(roleId);
        }

        private void AddReviewerToEmployee(Employee employee, int? reviewerId)
        {
            var reviewer = this.employeeRepository.Query().Where(e => e.Id == reviewerId).FirstOrDefault();
            if (reviewer != null)
            {
                employee.Reviewer = reviewer;
            }
            else
            {
                throw new EmployeeNotFoundException(reviewer.Id);
            }
        }

        private void ValidateContract(CreateEmployeeContract contract)
        {
            try
            {
                this.createEmployeeContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateEmployeeContract contract)
        {
            try
            {
                this.updateEmployeeContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETUPDATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateEmailExistence(int id, string email)
        {
            try
            {
                Employee employee = this.employeeRepository.Query().Where(emp => emp.EmailAddress == email && emp.Id != id).FirstOrDefault();
                if (employee != null)
                {
                    throw new InvalidEmployeeException("The email already exists.");
                }
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateDniExistence(int id, int dni)
        {
            try
            {
                Employee employee = this.employeeRepository.Query().Where(emp => emp.DNI == dni && emp.Id != id).FirstOrDefault();
                if (employee != null)
                {
                    throw new InvalidEmployeeException("The DNI already exists.");
                }
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        public Employee GetById(int id)
        {
            Employee employee = this.employeeRepository.Query().Where(emp => emp.Id == id).FirstOrDefault();
            return this.mapper.Map<Employee>(employee);
        }
    }
}
