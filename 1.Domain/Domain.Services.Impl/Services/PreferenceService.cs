using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Preference;
using Domain.Services.Interfaces.Services;

namespace Domain.Services.Impl.Services
{
    public class PreferenceService : IPreferenceService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Preference> _PreferenceRepository;
        private readonly IRepository<User> _UserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<PreferenceService> _log;

        public PreferenceService(
            IMapper mapper,
            IRepository<Preference> PreferenceRepository,
            IRepository<User> UserRepository,
            IUnitOfWork unitOfWork,
            ILog<PreferenceService> log
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _PreferenceRepository = PreferenceRepository;
            _UserRepository = UserRepository;
            _log = log;
        }
        
        public void Update(UpdatePreferenceContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Id}");

            _log.LogInformation($"Mapping contract {contract.Id}");
            var Preference = _mapper.Map<Preference>(contract);

            var updatedPreference = _PreferenceRepository.Update(Preference);
            _log.LogInformation($"Complete for {contract.Id}");
            _unitOfWork.Complete();
        }

        public ReadedPreferenceContract Read(int Id)
        {
            var PreferenceQuery = _PreferenceRepository
                .QueryEager()
                .Where(_ => _.Id == Id);

            var PreferenceResult = PreferenceQuery.SingleOrDefault();

            return _mapper.Map<ReadedPreferenceContract>(PreferenceResult);
        }

        public IEnumerable<ReadedPreferenceContract> List()
        {
            var preferenceQuery = _PreferenceRepository
                .QueryEager();

            var preferenceResult = preferenceQuery.ToList();

            return _mapper.Map<List<ReadedPreferenceContract>>(preferenceResult);
        }


    }
}
