// <copyright file="DashboardService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Contracts.Dashboard;
    using Domain.Services.Interfaces.Services;

    public class DashboardService : IDashboardService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Dashboard> dashboardRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<DashboardService> log;

        public DashboardService(
            IMapper mapper,
            IRepository<Dashboard> dashboardRepository,
            IUnitOfWork unitOfWork,
            ILog<DashboardService> log)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.dashboardRepository = dashboardRepository;
            this.log = log;
        }

        public CreatedDashboardContract Create(CreateDashboardContract contract)
        {
            var dashboard = this.mapper.Map<Dashboard>(contract);

            var createdDashboard = this.dashboardRepository.Create(dashboard);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedDashboardContract>(createdDashboard);
        }

        public void Update(UpdateDashboardContract contract)
        {
            this.log.LogInformation($"Mapping contract {contract.Id}");
            var dashboard = this.mapper.Map<Dashboard>(contract);

            var updatedDashboard = this.dashboardRepository.Update(dashboard);
            this.log.LogInformation($"Complete for {contract.Id}");
            this.unitOfWork.Complete();
        }

        public ReadedDashboardContract Read(int id)
        {
            var dashboardQuery = this.dashboardRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var dashboardResult = dashboardQuery.SingleOrDefault();

            return this.mapper.Map<ReadedDashboardContract>(dashboardResult);
        }

        public IEnumerable<ReadedDashboardContract> List()
        {
            var dashboardQuery = this.dashboardRepository
                .QueryEager();

            var dashboardResult = dashboardQuery.ToList();

            return this.mapper.Map<List<ReadedDashboardContract>>(dashboardResult);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching Dashboard {id}");
            Dashboard dashboard = this.dashboardRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (dashboard == null)
            {
                throw new Exception("The dashboard doesn't exist");
            }

            this.log.LogInformation($"Deleting Dashboard {id}");
            this.dashboardRepository.Delete(dashboard);

            this.unitOfWork.Complete();
        }
    }
}
