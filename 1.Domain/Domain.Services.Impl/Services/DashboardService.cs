using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Dashboard;
using Domain.Services.Interfaces.Services;

namespace Domain.Services.Impl.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Dashboard> _DashboardRepository;  
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<DashboardService> _log;

        public DashboardService(
            IMapper mapper,
            IRepository<Dashboard> DashboardRepository,           
            IUnitOfWork unitOfWork,
            ILog<DashboardService> log
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _DashboardRepository = DashboardRepository;
            _log = log;
        }

        public CreatedDashboardContract Create(CreateDashboardContract contract)
        {
            var Dashboard = _mapper.Map<Dashboard>(contract);

            var createdDashboard = _DashboardRepository.Create(Dashboard);
            _log.LogInformation($"Complete for {contract.Name}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Name}");
            return _mapper.Map<CreatedDashboardContract>(createdDashboard);
        }

        public void Update(UpdateDashboardContract contract)
        {
            //_log.LogInformation($"Validating contract {contract.Id}");

            _log.LogInformation($"Mapping contract {contract.Id}");
            var Dashboard = _mapper.Map<Dashboard>(contract);

            var updatedDashboard = _DashboardRepository.Update(Dashboard);
            _log.LogInformation($"Complete for {contract.Id}");
            _unitOfWork.Complete();
        }

        public ReadedDashboardContract Read(int Id)
        {
            var DashboardQuery = _DashboardRepository
                .QueryEager()
                .Where(_ => _.Id == Id);

            var DashboardResult = DashboardQuery.SingleOrDefault();

            return _mapper.Map<ReadedDashboardContract>(DashboardResult);
        }

        public IEnumerable<ReadedDashboardContract> List()
        {
            var DashboardQuery = _DashboardRepository
                .QueryEager();

            var DashboardResult = DashboardQuery.ToList();

            return _mapper.Map<List<ReadedDashboardContract>>(DashboardResult);
        }

        public void Delete(int Id)
        {
            _log.LogInformation($"Searching Dashboard {Id}");
            Dashboard dashboard = _DashboardRepository.Query().Where(_ => _.Id == Id).FirstOrDefault();

            if (dashboard == null)
            {
                throw new Exception("The dashboard doesn't exist");
            }
            _log.LogInformation($"Deleting Dashboard {Id}");
            _DashboardRepository.Delete(dashboard);

            _unitOfWork.Complete();
        }
    }
}
