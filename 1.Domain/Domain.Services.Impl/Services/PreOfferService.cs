using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.PreOffer;
using Domain.Services.Contracts.PreOffer;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.PreOffer;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class PreOfferService : IPreOfferService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<PreOffer> _preOfferRepository;        
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<PreOfferService> _log;
        private readonly UpdatePreOfferContractValidator _updatePreOfferContractValidator;
        private readonly CreatePreOfferContractValidator _createPreOfferContractValidator;

        public PreOfferService(
            IMapper mapper,
            IRepository<PreOffer> preOfferRepository,            
            IUnitOfWork unitOfWork,
            ILog<PreOfferService> log,
            UpdatePreOfferContractValidator updatePreOfferContractValidator,
            CreatePreOfferContractValidator createPreOfferContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _preOfferRepository = preOfferRepository;            
            _log = log;
            _updatePreOfferContractValidator = updatePreOfferContractValidator;
            _createPreOfferContractValidator = createPreOfferContractValidator;
        }

        public CreatedPreOfferContract Create(CreatePreOfferContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Status}");
            ValidateContract(contract);            

            _log.LogInformation($"Mapping contract {contract.Status}");
            var preOffer = _mapper.Map<PreOffer>(contract);            

            var createdPreOffer = _preOfferRepository.Create(preOffer);
            _log.LogInformation($"Complete for {contract.Status}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Status}");
            return _mapper.Map<CreatedPreOfferContract>(createdPreOffer);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching pre-offer {id}");
            PreOffer preOffer = _preOfferRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (preOffer == null)
            {
                throw new DeletePreOfferNotFoundException(id);
            }
            _log.LogInformation($"Deleting pre-offer {id}");
            _preOfferRepository.Delete(preOffer);

            _unitOfWork.Complete();
        }

        public void Update(UpdatePreOfferContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Status}");
            ValidateContract(contract);            

            _log.LogInformation($"Mapping contract {contract.Status}");
            var preOffer = _mapper.Map<PreOffer>(contract);            

            var updatedPreOffer = _preOfferRepository.Update(preOffer);
            _log.LogInformation($"Complete for {contract.Status}");
            _unitOfWork.Complete();
        }

        public IEnumerable<ReadedPreOfferContract> List()
        {
            var preOfferQuery = _preOfferRepository
                .QueryEager();

            var preOfferResult = preOfferQuery.ToList();

            return _mapper.Map<List<ReadedPreOfferContract>>(preOfferResult);
        }

        public ReadedPreOfferContract Read(int id)
        {
            var preOfferQuery = _preOfferRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var preOfferResult = preOfferQuery.SingleOrDefault();

            return _mapper.Map<ReadedPreOfferContract>(preOfferResult);
        }

        private void ValidateContract(CreatePreOfferContract contract)
        {
            try
            {
                _createPreOfferContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdatePreOfferContract contract)
        {
            try
            {
                _updatePreOfferContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
