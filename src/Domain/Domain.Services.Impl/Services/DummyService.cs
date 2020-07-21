using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model.Seed;
using Domain.Model.Seed.Exceptions;
using Domain.Services.Contracts.Seed;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Seed;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class DummyService : IDummyService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Dummy> _dummyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<DummyService> _log;
        private readonly UpdateDummyContractValidator _updateDummyContractValidator;
        private readonly CreateDummyContractValidator _createDummyContractValidator;

        public DummyService(
            IMapper mapper,
            IRepository<Dummy> dummyRepository,
            IUnitOfWork unitOfWork,
            ILog<DummyService> log,
            UpdateDummyContractValidator updateDummyContractValidator,
            CreateDummyContractValidator createDummyContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _dummyRepository = dummyRepository;
            _log = log;
            _updateDummyContractValidator = updateDummyContractValidator;
            _createDummyContractValidator = createDummyContractValidator;
        }

        public CreatedDummyContract Create(CreateDummyContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var dummy = _mapper.Map<Dummy>(contract);

            var createdDummy = _dummyRepository.Create(dummy);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedDummyContract>(createdDummy);
        }

        public void Delete(Guid id)
        {
            _log.LogInformation($"Searching dummy {id}");
            Dummy dummy = _dummyRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (dummy == null)
            {
                throw new DeleteDummyNotFoundException(id);
            }
            _log.LogInformation($"Deleting dummy {id}");
            _dummyRepository.Delete(dummy);

            _unitOfWork.Complete();
        }

        public void Update(UpdateDummyContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var dummy = _mapper.Map<Dummy>(contract);

            var updatedDummy = _dummyRepository.Update(dummy);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }

        public IEnumerable<ReadedDummyContract> List()
        {
            var dummyQuery = _dummyRepository
                .QueryEager();

            var dummyResult = dummyQuery.ToList();

            return _mapper.Map<List<ReadedDummyContract>>(dummyResult);
        }

        public ReadedDummyContract Read(Guid id)
        {
            var dummyQuery = _dummyRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var dummyResult = dummyQuery.SingleOrDefault();

            return _mapper.Map<ReadedDummyContract>(dummyResult);
        }

        private void ValidateContract(CreateDummyContract contract)
        {
            try
            {
                _createDummyContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateDummyContract contract)
        {
            try
            {
                _updateDummyContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

    }
}
