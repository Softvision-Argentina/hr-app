using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.HireProjection;
using Domain.Services.Contracts.HireProjection;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.HireProjection;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class HireProjectionService : IHireProjectionService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<HireProjection> _hireProjectionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<HireProjectionService> _log;
        private readonly UpdateHireProjectionContractValidator _updateHireProjectionContractValidator;
        private readonly CreateHireProjectionContractValidator _createHireProjectionContractValidator;

        public HireProjectionService(
            IMapper mapper,
            IRepository<HireProjection> hireProjectionRepository,
            IUnitOfWork unitOfWork,
            ILog<HireProjectionService> log,
            UpdateHireProjectionContractValidator updateHireProjectionContractValidator,
            CreateHireProjectionContractValidator createHireProjectionContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _hireProjectionRepository = hireProjectionRepository;
            _log = log;
            _updateHireProjectionContractValidator = updateHireProjectionContractValidator;
            _createHireProjectionContractValidator = createHireProjectionContractValidator;
        }

        public IEnumerable<ReadedHireProjectionContract> List()
        {
            var hireProjectionQuery = _hireProjectionRepository.QueryEager();

            var hireProjections = hireProjectionQuery.ToList();

            return _mapper.Map<List<ReadedHireProjectionContract>>(hireProjections);
        }

        public CreatedHireProjectionContract Create(CreateHireProjectionContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Month} + {contract.Year}");
            ValidateContract(contract);
            ValidateExistence(0, contract.Month, contract.Year);

            _log.LogInformation($"Mapping contract {contract.Month} + {contract.Year}");
            var hireProjection = _mapper.Map<HireProjection>(contract);

            var createdHireProjection = _hireProjectionRepository.Create(hireProjection);
            _log.LogInformation($"Complete for {contract.Month} + {contract.Year}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Month} + {contract.Year}");
            return _mapper.Map<CreatedHireProjectionContract>(createdHireProjection);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching projection {id}");
            HireProjection hireProjection = _hireProjectionRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (hireProjection == null)
            {
                throw new DeleteHireProjectionNotFoundException(id);
            }
            _log.LogInformation($"Deleting hireProjection {id}");
            _hireProjectionRepository.Delete(hireProjection);

            _unitOfWork.Complete();
        }

        public void Update(UpdateHireProjectionContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Month} + {contract.Year}");
            ValidateContract(contract);
            ValidateExistence(contract.Id, contract.Month, contract.Year);

            _log.LogInformation($"Mapping contract {contract.Month} + {contract.Year}");
            var hireProjection = _mapper.Map<HireProjection>(contract);

            var updatedHireProjection = _hireProjectionRepository.Update(hireProjection);
            _log.LogInformation($"Complete for {contract.Month} + {contract.Year}");
            _unitOfWork.Complete();
        }

        public ReadedHireProjectionContract Read(int id)
        {
            var hireProjectionQuery = _hireProjectionRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var hireProjectionResult = hireProjectionQuery.SingleOrDefault();

            return _mapper.Map<ReadedHireProjectionContract>(hireProjectionResult);
        }

        private void ValidateContract(CreateHireProjectionContract contract)
        {
            try
            {
                _createHireProjectionContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateHireProjectionContract contract)
        {
            try
            {
                _updateHireProjectionContractValidator.ValidateAndThrow(contract,
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
                HireProjection hireProjection = _hireProjectionRepository.Query().Where(_ => _.Month == month && _.Year == year && _.Id != id).FirstOrDefault();
                if (hireProjection != null) throw new InvalidHireProjectionException("The HireProjection already exists .");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
