using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Office;
using Domain.Services.Contracts.Office;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Office;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Office> _OfficeRepository;
        private readonly IRepository<Model.Room> _RoomItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<OfficeService> _log;
        private readonly UpdateOfficeContractValidator _updateOfficeContractValidator;
        private readonly CreateOfficeContractValidator _createOfficeContractValidator;

        public OfficeService(
            IMapper mapper,
            IRepository<Office> OfficeRepository,
            IRepository<Model.Room> RoomItemRepository,
            IUnitOfWork unitOfWork,
            ILog<OfficeService> log,
            UpdateOfficeContractValidator updateOfficeContractValidator,
            CreateOfficeContractValidator createOfficeContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _OfficeRepository = OfficeRepository;
            _RoomItemRepository = RoomItemRepository;
            _log = log;
            _updateOfficeContractValidator = updateOfficeContractValidator;
            _createOfficeContractValidator = createOfficeContractValidator;
        }

        public CreatedOfficeContract Create(CreateOfficeContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(0, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var Office = _mapper.Map<Office>(contract);

            var createdOffice = _OfficeRepository.Create(Office);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedOfficeContract>(createdOffice);
        }

        public void Delete(int Id)
        {
            _log.LogInformation($"Searching Candidate Profile {Id}");
            Office Office = _OfficeRepository.Query().Where(_ => _.Id == Id).FirstOrDefault();

            if (Office == null)
            {
                throw new DeleteOfficeNotFoundException(Id);
            }
            _log.LogInformation($"Deleting Candidate Profile {Id}");
            _OfficeRepository.Delete(Office);

            _unitOfWork.Complete();
        }

        public void Update(UpdateOfficeContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Name}");
            ValidateContract(contract);
            ValidateExistence(contract.Id, contract.Name);

            _log.LogInformation($"Mapping contract {contract.Name}");
            var Office = _mapper.Map<Office>(contract);


            var updatedOffice = _OfficeRepository.Update(Office);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
        }


        public IEnumerable<ReadedOfficeContract> List()
        {
            var OfficeQuery = _OfficeRepository
                .QueryEager();                 
                
            var OfficeResult = OfficeQuery.ToList();

            return _mapper.Map<List<ReadedOfficeContract>>(OfficeResult);
        }

        public ReadedOfficeContract Read(int Id)
        {
            var OfficeQuery = _OfficeRepository
                .QueryEager()                
                .Where(_ => _.Id == Id);

            var OfficeResult = OfficeQuery.SingleOrDefault();

            return _mapper.Map<ReadedOfficeContract>(OfficeResult);
        }

        private void ValidateContract(CreateOfficeContract contract)
        {
            try
            {
                _createOfficeContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateOfficeContract contract)
        {
            try
            {
                _updateOfficeContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateExistence(int Id, string name)
        {
            try
            {
                Office Office = _OfficeRepository.Query().Where(_ => _.Name == name && _.Id != Id).FirstOrDefault();
                if (Office != null) throw new InvalidOfficeException("The Office already exists .");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
