using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Employee;
using Domain.Model.Exceptions.Employee;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Employee;
using Domain.Services.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace Domain.Services.Impl.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<EmployeeService> _logger;
        private readonly UpdateEmployeeContractValidator _updateEmployeeContractValidator;
        private readonly CreateEmployeeContractValidator _createEmployeeContractValidator;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public EmployeeService(IMapper mapper,
            IRepository<Employee> employeeRepository,
            IUnitOfWork unitOfWork,
            ILog<EmployeeService> log,
            UpdateEmployeeContractValidator updateEmployeeContractValidator,
            CreateEmployeeContractValidator createEmployeeContractValidator,
            IRepository<User> userRepository,
            IRepository<Role> roleRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
            _logger = log;
            _updateEmployeeContractValidator = updateEmployeeContractValidator;
            _createEmployeeContractValidator = createEmployeeContractValidator;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public IEnumerable<ReadedEmployeeContract> List()
        {
            var employeeQuery = _employeeRepository.QueryEager();

            var employeeResult = employeeQuery.ToList();

            return _mapper.Map<List<ReadedEmployeeContract>>(employeeResult);
        }

        public void Delete(int id)
        {
            _logger.LogInformation($"Searching employee {id}");
            Employee employee= _employeeRepository.Query().Where(emp => emp.Id == id).FirstOrDefault();

            if (employee == null)
            {
                throw new DeleteEmployeeNotFoundException(id);
            }
            _logger.LogInformation($"Deleting employee {id}");
            _employeeRepository.Delete(employee);

            _unitOfWork.Complete();
        }

        public Employee getByDNI(int dni)
        {
            Employee employee = _employeeRepository.Query().Where(emp => emp.DNI == dni).FirstOrDefault();
            return _mapper.Map<Employee>(employee);
        }

        public Employee GetByEmail(string email)
        {
            Employee employee = _employeeRepository.Query().Where(emp => emp.EmailAddress == email).FirstOrDefault();
            return _mapper.Map<Employee>(employee);
        }

        public CreatedEmployeeContract Create(CreateEmployeeContract contract)
        {
            _logger.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateEmailExistence(0, contract.EmailAddress);
            ValidateDniExistence(0, contract.DNI);
            
            _logger.LogInformation($"Mapping contract {contract.Name}");
            var employee = _mapper.Map<Employee>(contract);

            this.AddUserToEmployee(employee, contract.UserId);
            this.AddRoleToEmployee(employee, contract.RoleId);
            if(contract.ReviewerId != null)
                this.AddReviewerToEmployee(employee, contract.ReviewerId);

            var createdEmployee = _employeeRepository.Create(employee);
            _logger.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _logger.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedEmployeeContract>(createdEmployee);
        }

        public void UpdateEmployee(UpdateEmployeeContract contract)
        {
            _logger.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateEmailExistence(contract.Id, contract.EmailAddress);
            ValidateDniExistence(contract.Id, contract.DNI);

            _logger.LogInformation($"Mapping contract {contract.Name}");
            var employee = _mapper.Map<Employee>(contract);

            this.AddUserToEmployee(employee, contract.UserId);
            this.AddRoleToEmployee(employee, contract.RoleId);
            if (contract.ReviewerId != null)
                this.AddReviewerToEmployee(employee, contract.ReviewerId);

            _employeeRepository.Update(employee);

            _logger.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }

        private void AddUserToEmployee(Employee employee, int userID)
        {

            var user = _userRepository.Query().Where(usr => usr.Id == userID).FirstOrDefault();
            //if user == null => throw
            employee.User = user ?? throw new Domain.Model.Exceptions.User.UserNotFoundException(userID);
        }

        private void AddRoleToEmployee(Employee employee, int roleId)
        {

            var role = _roleRepository.Query().Where(r => r.Id == roleId).FirstOrDefault();
            //if user == null => throw
            employee.Role = role ?? throw new Domain.Model.Exceptions.Role.RoleNotFoundException(roleId);
        }

        private void AddReviewerToEmployee(Employee employee, int? ReviewerId)
        {

            var Reviewer = _employeeRepository.Query().Where(e => e.Id == ReviewerId).FirstOrDefault();
            //if recruiter == null => throw
            if (Reviewer != null)
            {
                employee.Reviewer = Reviewer;
            }
             else throw new EmployeeNotFoundException(Reviewer.Id);
        }

        private void ValidateContract(CreateEmployeeContract contract)
        {
            try
            {
                _createEmployeeContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
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
                _updateEmployeeContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_UPDATE}");
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
                Employee employee = _employeeRepository.Query().Where(emp => emp.EmailAddress == email && emp.Id != id).FirstOrDefault();
                if (employee != null) throw new InvalidEmployeeException("The email already exists.");
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
                Employee employee = _employeeRepository.Query().Where(emp => emp.DNI == dni && emp.Id != id).FirstOrDefault();
                if (employee != null) throw new InvalidEmployeeException("The DNI already exists.");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        public Employee getById(int id)
        {
            Employee employee = _employeeRepository.Query().Where(emp => emp.Id == id).FirstOrDefault();
            return _mapper.Map<Employee>(employee);
        }
    }
}
