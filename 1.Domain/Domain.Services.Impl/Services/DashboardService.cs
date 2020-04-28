using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Dashboard;
using Domain.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Dashboard> _dashboardRepository;  
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<DashboardService> _log;

        public DashboardService(
            IMapper mapper,
            IRepository<Dashboard> dashboardRepository,           
            IUnitOfWork unitOfWork,
            ILog<DashboardService> log
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _dashboardRepository = dashboardRepository;
            _log = log;
        }

        public CreatedDashboardContract Create(CreateDashboardContract contract)
        {
            var dashboard = _mapper.Map<Dashboard>(contract);

            var createdDashboard = _dashboardRepository.Create(dashboard);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedDashboardContract>(createdDashboard);
        }

        public void Update(UpdateDashboardContract contract)
        {
            _log.LogInformation($"Mapping contract {contract.Id}");
            var dashboard = _mapper.Map<Dashboard>(contract);

            var updatedDashboard = _dashboardRepository.Update(dashboard);
            _log.LogInformation($"Complete for {contract.Id}");
            _unitOfWork.Complete();
        }

        public ReadedDashboardContract Read(int id)
        {
            var dashboardQuery = _dashboardRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var dashboardResult = dashboardQuery.SingleOrDefault();

            return _mapper.Map<ReadedDashboardContract>(dashboardResult);
        }

        public IEnumerable<ReadedDashboardContract> List()
        {
            var dashboardQuery = _dashboardRepository
                .QueryEager();

            var dashboardResult = dashboardQuery.ToList();

            return _mapper.Map<List<ReadedDashboardContract>>(dashboardResult);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching Dashboard {id}");
            Dashboard dashboard = _dashboardRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (dashboard == null)
            {
                throw new Exception("The dashboard doesn't exist");
            }
            _log.LogInformation($"Deleting Dashboard {id}");
            _dashboardRepository.Delete(dashboard);

            _unitOfWork.Complete();
        }
    }
}
