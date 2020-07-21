using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Community;
using Domain.Model.Exceptions.Office;
using Domain.Services.Contracts.OpenPositions;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.OpenPosition;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class OpenPositionService : IOpenPositionService
    {
        private readonly IRepository<OpenPosition> _openPositionRepository;
        private readonly IRepository<Community> _communityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILog<OpenPosition> _log;
        private readonly UpdateOpenPositionContractValidator _updateValidator;
        private readonly CreateOpenPositionContractValidator _createValidator;

        public OpenPositionService(
            IRepository<OpenPosition> openPositionRepository,
            IRepository<Community> communityRepository,
            IUnitOfWork unitOfWork,
            ILog<OpenPosition> log,
            IMapper mapper,
            UpdateOpenPositionContractValidator updateValidator,
            CreateOpenPositionContractValidator createValidator
            )
        {
            _openPositionRepository = openPositionRepository;
            _communityRepository = communityRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _log = log;
            _updateValidator = updateValidator;
            _createValidator = createValidator;
        }

        public CreatedOpenPositionContract Create(CreateOpenPositionContract openPosition)
        {
            _log.LogInformation($"Validating contract {openPosition.Title}");
            ValidateContract(openPosition);

            _log.LogInformation($"Mapping contract {openPosition.Title}");
            var position = _mapper.Map<OpenPosition>(openPosition);

            AddCommunityToPosition(position, openPosition.Community.Id);

            var createdPosition = _openPositionRepository.Create(position);
            _log.LogInformation($"Complete for {openPosition.Title}");

            _unitOfWork.Complete();

            _log.LogInformation($"Return {openPosition.Title}");

            return _mapper.Map<CreatedOpenPositionContract>(createdPosition);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching candidate {id}");
            var position = _openPositionRepository.Query().FirstOrDefault(p => p.Id == id);

            if (position == null)
            {
                throw new Exception("Position not found");
            }

            _log.LogInformation($"Deleting position {id}");
            _openPositionRepository.Delete(position);

            _unitOfWork.Complete();
        }

        public IEnumerable<ReadedOpenPositionContract> Get()
        {
            var positions = _openPositionRepository.QueryEager().ToList();

            return _mapper.Map<List<ReadedOpenPositionContract>>(positions);
        }

        public ReadedOpenPositionContract GetById(int id)
        {
            var position = _openPositionRepository.QueryEager().Where(c => c.Id == id).FirstOrDefault();

            return _mapper.Map<ReadedOpenPositionContract>(position);
        }

        public void Update(UpdateOpenPositionContract openPosition)
        {
            _log.LogInformation($"Validating contract {openPosition.Title}");
            ValidateContract(openPosition);

            _log.LogInformation($"Mapping contract {openPosition.Title}");
            var position = _mapper.Map<OpenPosition>(openPosition);

            AddCommunityToPosition(position, openPosition.Community.Id);

            _openPositionRepository.Update(position);
            _log.LogInformation($"Complete for {openPosition.Title}");

            _unitOfWork.Complete();
        }

        private void ValidateContract(CreateOpenPositionContract openPosition)
        {
            try
            {
                _createValidator.ValidateAndThrow(openPosition,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void ValidateContract(UpdateOpenPositionContract openPosition)
        {
            try
            {
                _updateValidator.ValidateAndThrow(openPosition,
                    $"{ValidatorConstants.RULESET_UPDATE}");
            }
            catch (ValidationException ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void AddCommunityToPosition(OpenPosition position, int communityId)
        {
            var community = _communityRepository.Query().Where(c => c.Id == communityId).FirstOrDefault();

            if (community == null)
                throw new CommunityNotFoundException(communityId);

            position.Community = community;
        }
    }
}
