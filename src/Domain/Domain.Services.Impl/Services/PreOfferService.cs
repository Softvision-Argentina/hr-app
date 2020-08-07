// <copyright file="PreOfferService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.PreOffer;
    using Domain.Services.Contracts.PreOffer;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.PreOffer;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class PreOfferService : IPreOfferService
    {
        private readonly IMapper mapper;
        private readonly IRepository<PreOffer> preOfferRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<PreOfferService> log;
        private readonly UpdatePreOfferContractValidator updatePreOfferContractValidator;
        private readonly CreatePreOfferContractValidator createPreOfferContractValidator;

        public PreOfferService(
            IMapper mapper,
            IRepository<PreOffer> preOfferRepository,
            IUnitOfWork unitOfWork,
            ILog<PreOfferService> log,
            UpdatePreOfferContractValidator updatePreOfferContractValidator,
            CreatePreOfferContractValidator createPreOfferContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.preOfferRepository = preOfferRepository;
            this.log = log;
            this.updatePreOfferContractValidator = updatePreOfferContractValidator;
            this.createPreOfferContractValidator = createPreOfferContractValidator;
        }

        public CreatedPreOfferContract Create(CreatePreOfferContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Status}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Status}");
            var preOffer = this.mapper.Map<PreOffer>(contract);

            var createdPreOffer = this.preOfferRepository.Create(preOffer);
            this.log.LogInformation($"Complete for {contract.Status}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Status}");
            return this.mapper.Map<CreatedPreOfferContract>(createdPreOffer);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching pre-offer {id}");
            PreOffer preOffer = this.preOfferRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (preOffer == null)
            {
                throw new DeletePreOfferNotFoundException(id);
            }

            this.log.LogInformation($"Deleting pre-offer {id}");
            this.preOfferRepository.Delete(preOffer);

            this.unitOfWork.Complete();
        }

        public void Update(UpdatePreOfferContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Status}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Status}");
            var preOffer = this.mapper.Map<PreOffer>(contract);

            var updatedPreOffer = this.preOfferRepository.Update(preOffer);
            this.log.LogInformation($"Complete for {contract.Status}");
            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedPreOfferContract> List()
        {
            var preOfferQuery = this.preOfferRepository
                .QueryEager();

            var preOfferResult = preOfferQuery.ToList();

            return this.mapper.Map<List<ReadedPreOfferContract>>(preOfferResult);
        }

        public ReadedPreOfferContract Read(int id)
        {
            var preOfferQuery = this.preOfferRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var preOfferResult = preOfferQuery.SingleOrDefault();

            return this.mapper.Map<ReadedPreOfferContract>(preOfferResult);
        }

        public IEnumerable<ReadedPreOfferContract> GetByProcessId(int id)
        {
            var preOfferQuery = this.preOfferRepository
                .QueryEager().Where(_ => _.ProcessId == id);

            var preOfferResult = preOfferQuery.ToList();

            return this.mapper.Map<List<ReadedPreOfferContract>>(preOfferResult);
        }

        private void ValidateContract(CreatePreOfferContract contract)
        {
            try
            {
                this.createPreOfferContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
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
                this.updatePreOfferContractValidator.ValidateAndThrow(
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
