using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Offer;
using Domain.Services.Contracts.Offer;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Offer;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class OfferService : IOfferService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Offer> _offerRepository;        
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<OfferService> _log;
        private readonly UpdateOfferContractValidator _updateOfferContractValidator;
        private readonly CreateOfferContractValidator _createOfferContractValidator;

        public OfferService(
            IMapper mapper,
            IRepository<Offer> offerRepository,            
            IUnitOfWork unitOfWork,
            ILog<OfferService> log,
            UpdateOfferContractValidator updateOfferContractValidator,
            CreateOfferContractValidator createOfferContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _offerRepository = offerRepository;            
            _log = log;
            _updateOfferContractValidator = updateOfferContractValidator;
            _createOfferContractValidator = createOfferContractValidator;
        }

        public CreatedOfferContract Create(CreateOfferContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Status}");
            ValidateContract(contract);            

            _log.LogInformation($"Mapping contract {contract.Status}");
            var offer = _mapper.Map<Offer>(contract);            

            var createdOffer = _offerRepository.Create(offer);
            _log.LogInformation($"Complete for {contract.Status}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Status}");
            return _mapper.Map<CreatedOfferContract>(createdOffer);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching offer {id}");
            Offer offer = _offerRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (offer == null)
            {
                throw new DeleteOfferNotFoundException(id);
            }
            _log.LogInformation($"Deleting offer {id}");
            _offerRepository.Delete(offer);

            _unitOfWork.Complete();
        }

        public void Update(UpdateOfferContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Status}");
            ValidateContract(contract);            

            _log.LogInformation($"Mapping contract {contract.Status}");
            var offer = _mapper.Map<Offer>(contract);            

            var updatedOffer = _offerRepository.Update(offer);
            _log.LogInformation($"Complete for {contract.Status}");
            _unitOfWork.Complete();
        }

        public IEnumerable<ReadedOfferContract> List()
        {
            var offerQuery = _offerRepository
                .QueryEager();

            var offerResult = offerQuery.ToList();

            return _mapper.Map<List<ReadedOfferContract>>(offerResult);
        }

        public ReadedOfferContract Read(int id)
        {
            var offerQuery = _offerRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var offerResult = offerQuery.SingleOrDefault();

            return _mapper.Map<ReadedOfferContract>(offerResult);
        }

        private void ValidateContract(CreateOfferContract contract)
        {
            try
            {
                _createOfferContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateOfferContract contract)
        {
            try
            {
                _updateOfferContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
