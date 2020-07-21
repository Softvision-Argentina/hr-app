using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.EmployeeCasualty;
using Domain.Services.Contracts.EmployeeCasualty;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.EmployeeCasualty;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class EmployeeCasualtyService : IEmployeeCasualtyService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<EmployeeCasualty> _employeeCasualtyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<EmployeeCasualtyService> _log;
        private readonly UpdateEmployeeCasualtyContractValidator _updateEmployeeCasualtyContractValidator;
        private readonly CreateEmployeeCasualtyContractValidator _createEmployeeCasualtyContractValidator;

        public EmployeeCasualtyService(
            IMapper mapper,
            IRepository<EmployeeCasualty> employeeCasualtyRepository,
            IUnitOfWork unitOfWork,
            ILog<EmployeeCasualtyService> log,
            UpdateEmployeeCasualtyContractValidator updateEmployeeCasualtyContractValidator,
            CreateEmployeeCasualtyContractValidator createEmployeeCasualtyContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _employeeCasualtyRepository = employeeCasualtyRepository;
            _log = log;
            _updateEmployeeCasualtyContractValidator = updateEmployeeCasualtyContractValidator;
            _createEmployeeCasualtyContractValidator = createEmployeeCasualtyContractValidator;
        }

        public IEnumerable<ReadedEmployeeCasualtyContract> List()
        {
            var employeeCasualtyQuery = _employeeCasualtyRepository.QueryEager();

            var employeeCasualties = employeeCasualtyQuery.ToList();

            return _mapper.Map<List<ReadedEmployeeCasualtyContract>>(employeeCasualties);
        }

        public CreatedEmployeeCasualtyContract Create(CreateEmployeeCasualtyContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Month} + {contract.Year}");
            ValidateContract(contract);
            ValidateExistence(0, contract.Month, contract.Year);

            _log.LogInformation($"Mapping contract {contract.Month} + {contract.Year}");
            var employeeCasualty = _mapper.Map<EmployeeCasualty>(contract);

            var createdEmployeeCasualty = _employeeCasualtyRepository.Create(employeeCasualty);
            _log.LogInformation($"Complete for {contract.Month} + {contract.Year}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Month} + {contract.Year}");
            return _mapper.Map<CreatedEmployeeCasualtyContract>(createdEmployeeCasualty);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching casualty {id}");
            EmployeeCasualty employeeCasualty = _employeeCasualtyRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (employeeCasualty == null)
            {
                throw new DeleteEmployeeCasualtyNotFoundException(id);
            }
            _log.LogInformation($"Deleting employeeCasualty {id}");
            _employeeCasualtyRepository.Delete(employeeCasualty);

            _unitOfWork.Complete();
        }

        public void Update(UpdateEmployeeCasualtyContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Month} + {contract.Year}");
            ValidateContract(contract);
            ValidateExistence(contract.Id, contract.Month, contract.Year);

            _log.LogInformation($"Mapping contract {contract.Month} + {contract.Year}");
            var employeeCasualty = _mapper.Map<EmployeeCasualty>(contract);

            var updatedEmployeeCasualty = _employeeCasualtyRepository.Update(employeeCasualty);
            _log.LogInformation($"Complete for {contract.Month} + {contract.Year}");
            _unitOfWork.Complete();
        }

        public ReadedEmployeeCasualtyContract Read(int id)
        {
            var employeeCasualtyQuery = _employeeCasualtyRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var employeeCasualtyResult = employeeCasualtyQuery.SingleOrDefault();

            return _mapper.Map<ReadedEmployeeCasualtyContract>(employeeCasualtyResult);
        }

        private void ValidateContract(CreateEmployeeCasualtyContract contract)
        {
            try
            {
                _createEmployeeCasualtyContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateEmployeeCasualtyContract contract)
        {
            try
            {
                _updateEmployeeCasualtyContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateExistence(int id, int month, int year)
        {
            try
            {
                EmployeeCasualty employeeCasualty = _employeeCasualtyRepository.Query().Where(_ => _.Month == month && _.Year == year && _.Id != id).FirstOrDefault();
                if (employeeCasualty != null) throw new InvalidEmployeeCasualtyException("The EmployeeCasualty already exists .");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
