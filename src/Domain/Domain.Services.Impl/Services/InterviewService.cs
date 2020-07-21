
using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Interview;
using Domain.Services.Impl.Validators;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Interview> _interviewRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InterviewService(IMapper mapper,
        IRepository<Interview> interviewRepository,
        IUnitOfWork unitOfWork
        )
        {
            _mapper = mapper;
            _interviewRepository = interviewRepository;
            _unitOfWork = unitOfWork;
        }

        public CreatedInterviewContract Create(CreateInterviewContract contract)
        {
            var interview = _mapper.Map<Interview>(contract);
            var createdInterview = _interviewRepository.Create(interview);
            _unitOfWork.Complete();

            return _mapper.Map<CreatedInterviewContract>(createdInterview);
        }
        public void Delete(int id)
        {
            Interview interview = _interviewRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            _interviewRepository.Delete(interview);
            _unitOfWork.Complete();

        }
        public void Update(UpdateInterviewContract contract)
        {

            var interview = _mapper.Map<Interview>(contract);

            var updatedInterview = _interviewRepository.Update(interview);
            _unitOfWork.Complete();

        }

        public ReadedInterviewContract Read(int id)
        {
            var interviewQuery = _interviewRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var interviewResult = interviewQuery.SingleOrDefault();
            _unitOfWork.Complete();

            return _mapper.Map<ReadedInterviewContract>(interviewResult);
        }

        public IEnumerable<ReadedInterviewContract> List()
        {
            var interviewQuery = _interviewRepository
                .QueryEager();

            var interviewResult = interviewQuery.ToList();
            _unitOfWork.Complete();

            return _mapper.Map<List<ReadedInterviewContract>>(interviewResult);
        }

    }
}