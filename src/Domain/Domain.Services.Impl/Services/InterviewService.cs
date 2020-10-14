// <copyright file="InterviewService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Contracts.Interview;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class InterviewService : IInterviewService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Interview> interviewRepository;
        private readonly IUnitOfWork unitOfWork;

        public InterviewService(
            IMapper mapper,
            IRepository<Interview> interviewRepository,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.interviewRepository = interviewRepository;
            this.unitOfWork = unitOfWork;
        }

        public CreatedInterviewContract Create(CreateInterviewContract contract)
        {
            var interview = this.mapper.Map<Interview>(contract);
            var createdInterview = this.interviewRepository.Create(interview);
            this.unitOfWork.Complete();

            return this.mapper.Map<CreatedInterviewContract>(createdInterview);
        }

        public void Delete(int id)
        {
            Interview interview = this.interviewRepository.Query().Where(_ => _.Id == id).FirstOrDefault();
            this.interviewRepository.Delete(interview);
            this.unitOfWork.Complete();
        }

        public void Update(UpdateInterviewContract contract)
        {
            var interview = this.mapper.Map<Interview>(contract);

            var updatedInterview = this.interviewRepository.Update(interview);
            this.unitOfWork.Complete();
        }

        public void UpdateMany(int clientStageId, List<CreateInterviewContract> contracts)
        {
            foreach (var interview in this.interviewRepository.Query().Where(_ => _.ClientStageId == clientStageId))
            {
                this.interviewRepository.Delete(interview);
            }

            foreach (var contract in contracts)
            {
                var interview = this.mapper.Map<Interview>(contract);
                this.interviewRepository.Create(interview);
            }

            this.unitOfWork.Complete();
        }

        public ReadedInterviewContract Read(int id)
        {
            var interviewQuery = this.interviewRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var interviewResult = interviewQuery.SingleOrDefault();
            this.unitOfWork.Complete();

            return this.mapper.Map<ReadedInterviewContract>(interviewResult);
        }

        public IEnumerable<ReadedInterviewContract> List()
        {
            var interviewQuery = this.interviewRepository
                .QueryEager();

            var interviewResult = interviewQuery.ToList();
            this.unitOfWork.Complete();

            return this.mapper.Map<List<ReadedInterviewContract>>(interviewResult);
        }
    }
}