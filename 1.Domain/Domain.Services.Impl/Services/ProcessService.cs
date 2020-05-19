using AutoMapper;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Process;
using Domain.Services.Interfaces.Repositories;
using Domain.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Domain.Services.Impl.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IMapper _mapper;
        private readonly IProcessRepository _processRepository;
        private readonly IProcessStageRepository _processStageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Community> _communityRepository;
        private readonly IRepository<CandidateProfile> _candidateProfileRepository;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IRepository<Office> _officeRepository;
        private readonly IRepository<DeclineReason> _declineReasonRepository;
        private readonly IHrStageRepository _hrStageRepository;
        private readonly ITechnicalStageRepository _technicalStageRepository;
        private readonly IClientStageRepository _clientStageRepository;
        private readonly IOfferStageRepository _offerStageRepository;
        private readonly IPreOfferStageRepository _preOfferStageRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContext;

        public ProcessService(
            IMapper mapper,
            IRepository<User> userRepository,
            IRepository<Candidate> candidateRepository,
            IRepository<CandidateProfile> candidateProfileRepository,
            IRepository<Community> communityRepository,
            IRepository<Office> officeRepository,
            IRepository<DeclineReason> declineReasonRepository,
            IProcessRepository processRepository,
            IProcessStageRepository processStageRepository,
            IHrStageRepository hrStageRepository,
            ITechnicalStageRepository technicalStageRepository,
            IClientStageRepository clientStageRepository,
            IOfferStageRepository offerStageRepository,
            IUnitOfWork unitOfWork,
            INotificationRepository notificationRepository,
            IConfiguration config,
            IHttpContextAccessor httpContext,
            IPreOfferStageRepository preOfferStageRepository)
            
        {            
            _candidateRepository = candidateRepository;
            _candidateProfileRepository = candidateProfileRepository;
            _communityRepository = communityRepository;
            _officeRepository = officeRepository;
            _declineReasonRepository = declineReasonRepository;
            _mapper = mapper;
            _processRepository = processRepository;
            _processStageRepository = processStageRepository;
            _hrStageRepository = hrStageRepository;
            _technicalStageRepository = technicalStageRepository;
            _clientStageRepository = clientStageRepository;
            _offerStageRepository = offerStageRepository;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _config = config;
            _httpContext = httpContext;
            _preOfferStageRepository = preOfferStageRepository;
        }

        public ReadedProcessContract Read(int id)
        {
            var process = _processRepository
                .QueryEager().SingleOrDefault(_ => _.Id == id);

            return _mapper.Map<ReadedProcessContract>(process);
        }
        public IEnumerable<ReadedProcessContract> GetProcessesByCommunity(string community)
        {
            var candidateQuery = _processRepository
                .QueryEager().Where(pro => pro.Candidate.Community.Name.Equals(community));

            var candidateResult = candidateQuery.ToList();

            return _mapper.Map<List<ReadedProcessContract>>(candidateResult);
        }

        public void Delete(int id)
        {
            var process = _processRepository.QueryEager().FirstOrDefault(p => p.Id == id);

            process.Candidate.Status = SetCandidateStatus(ProcessStatus.Rejected);

            _processRepository.Delete(process);

            _unitOfWork.Complete();
        }

        public IEnumerable<ReadedProcessContract> List()
        {
            var candidateQuery = _processRepository
                .QueryEager().ToList();

            var candidateResult = candidateQuery.OrderByDescending(x => x.StartDate).ToList();

            return _mapper.Map<List<ReadedProcessContract>>(candidateResult);
        }

        public IEnumerable<ReadedProcessContract> GetActiveByCandidateId(int candidateId)
        {
            var process = _processRepository
                .QueryEager().Where(_ => _.CandidateId == candidateId && (_.Status == ProcessStatus.InProgress || _.Status == ProcessStatus.Accepted || _.Status == ProcessStatus.Recall ));

            return _mapper.Map<IEnumerable<ReadedProcessContract>>(process);
        }

        public CreatedProcessContract Create(CreateProcessContract createProcessContract)
        {
            var process = _mapper.Map<Process>(createProcessContract);

            this.AddOfficeToCandidate(process.Candidate, createProcessContract.Candidate.PreferredOfficeId);

            process.Candidate.Status = CandidateStatus.InProgress;

            _candidateRepository.Update(process.Candidate);

            process.CurrentStage = SetProcessCurrentStage(process);

            var userId = GetUser();

            process.UserOwnerId = userId;

            var createdProcess = _processRepository.Create(process);

            process.HrStage.UserOwnerId = process.UserOwnerId;
            process.TechnicalStage.UserOwnerId = process.UserOwnerId;
            process.ClientStage.UserOwnerId = process.UserOwnerId;
            process.PreOfferStage.UserOwnerId = process.UserOwnerId;
            process.OfferStage.UserOwnerId = process.UserOwnerId;

            _unitOfWork.Complete();

            var createdProcessContract = _mapper.Map<CreatedProcessContract>(createdProcess);

            var status = process.Status;

            if (process.Candidate.ReferredBy != null && process.Status == ProcessStatus.InProgress)
            {
                var notification = new Notification
                {
                    Text = $"Your referral's {process.Candidate.Name} {process.Candidate.LastName} process status is {status}"
                };

                _notificationRepository.Create(notification, process.Candidate.Id);

                SendEmailNotification(process, status);
            }

            return createdProcessContract;
        }

        private int GetUser()
        {
            var getUser = _httpContext.HttpContext.User.Identity.Name;
            var userId = int.Parse(getUser);
            return userId;
        }

        private void SendEmailNotification(Process process, ProcessStatus status)
        {
            var email = GetUserMail(process.Candidate.ReferredBy);

            using (var client = new SmtpClient(_config.GetValue<string>("smtpClient"), _config.GetValue<int>("smtpClientPort")))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_config.GetValue<string>("networkCredentialMail"), _config.GetValue<string>("networkCredentialPass"));
                var message = new MailMessage(_config.GetValue<string>("email"), email, "Referral's status", $"Your referral's {process.Candidate.Name} {process.Candidate.LastName} process status is {status}");
                client.Send(message);
            }
        }

        private string GetUserMail(string referredBy)
        {
            var referred = referredBy.Split(" ");
            var userName = _userRepository.Query().FirstOrDefault(x => x.FirstName == referred[0] && x.LastName == referred[1]);
            var mail = userName.Username;

            return mail;
        }

        private void AddOfficeToCandidate(Candidate candidate, int officeId)
        {
            var office = _officeRepository.Query().Where(_ => _.Id == officeId).FirstOrDefault();
            if (office == null)
                throw new Domain.Model.Exceptions.Office.OfficeNotFoundException(officeId);

            candidate.PreferredOffice = office;
        }

        public void Update(UpdateProcessContract updateProcessContract)
        {
            var process = _mapper.Map<Process>(updateProcessContract);
            process.Status = SetProcessStatus(process);
            process.CurrentStage = SetProcessCurrentStage(process);

            var candidate = _candidateRepository.QueryEager().FirstOrDefault(c => c.Id == process.Candidate.Id);
            candidate.EnglishLevel = process.HrStage.EnglishLevel;            
            candidate.Status = SetCandidateStatus(process.Status);
            process.Candidate = candidate;
            candidate.DNI = process.PreOfferStage.DNI;
            process.HrStage.UserOwnerId = process.UserOwnerId;
            process.TechnicalStage.UserOwnerId = process.UserOwnerId;
            process.ClientStage.UserOwnerId = process.UserOwnerId;
            process.PreOfferStage.UserOwnerId = process.UserOwnerId;
            process.OfferStage.UserOwnerId = process.UserOwnerId;

            _candidateRepository.Update(candidate);
            _hrStageRepository.Update(process.HrStage);
            _technicalStageRepository.Update(process.TechnicalStage);
            _clientStageRepository.Update(process.ClientStage);
            _preOfferStageRepository.Update(process.PreOfferStage);
            _offerStageRepository.Update(process.OfferStage);

            if(process.DeclineReasonId != null)
            {
                if (process.DeclineReason.Id == -1)
                {
                    process.DeclineReason = _declineReasonRepository.Create(new DeclineReason
                    {
                        Name = "Other",
                        Description = updateProcessContract.DeclineReason.Description
                    });
                    process.DeclineReasonId = process.DeclineReason.Id;
                }
                else
                {
                    process.DeclineReason = _declineReasonRepository.Get(process.DeclineReason.Id);
                }
            }

            var updatedProcess = _processRepository.Update(process);
            var status = process.Status;

            if (process.Candidate.ReferredBy != null && (process.Status == ProcessStatus.Hired || process.Status == ProcessStatus.InProgress 
                || process.Status == ProcessStatus.Accepted || process.Status == ProcessStatus.Recall))
            {
                var notification = new Notification
                {
                    Text = $"Your referral's {process.Candidate.Name} {process.Candidate.LastName} process status is {status}"
                };
                _notificationRepository.Create(notification, process.Candidate.Id);
                SendEmailNotification(process, status);
            }

            _unitOfWork.Complete();
        }

        public void Approve(int processId)
        {
            _processRepository.Approve(processId);
            var process = _processRepository.QueryEager().FirstOrDefault(p => p.Id == processId);
            process.Candidate.Status = SetCandidateStatus(process.Status);
            _unitOfWork.Complete();
        }
         
        public void Reject(int id, string rejectionReason)
        {
            _processRepository.Reject(id, rejectionReason);
            var process = _processRepository.QueryEager().FirstOrDefault(p => p.Id == id);
            process.Candidate.Status = SetCandidateStatus(process.Status);
            var status = process.Status;

            if (process.Candidate.ReferredBy != null && process.Status == ProcessStatus.Rejected || process.Status == ProcessStatus.Declined)
            {
                var notification = new Notification
                {
                    Text = $"Your referral's {process.Candidate.Name} {process.Candidate.LastName} process status is {status}"
                };
                _notificationRepository.Create(notification, process.Candidate.Id);
                SendEmailNotification(process, status);
            }

            _unitOfWork.Complete();
        }

        private ProcessStatus SetProcessStatus(Process process)
        {
            if (process.OfferStage.Status != StageStatus.NA)
            {
                return (ProcessStatus)process.OfferStage.Status;
            }
            else if (process.PreOfferStage.Status != StageStatus.NA)
            {
                return (ProcessStatus)process.PreOfferStage.Status;
            }
            else if (process.ClientStage.Status != StageStatus.NA)
            {
                return (ProcessStatus)process.ClientStage.Status;
            }
            else if (process.TechnicalStage.Status != StageStatus.NA)
            {
                return (ProcessStatus)process.TechnicalStage.Status;
            }

            return (ProcessStatus)process.HrStage.Status;
        }

        private CandidateStatus SetCandidateStatus(ProcessStatus processStatus)
        {
            return (CandidateStatus)processStatus;
        }

        private ProcessCurrentStage SetProcessCurrentStage(Process process)
        {
            if (process.OfferStage.Status != StageStatus.NA && process.OfferStage.Status != StageStatus.Declined && process.OfferStage.Status != StageStatus.Rejected)
            {
                return ProcessCurrentStage.OfferStage;
            }
            else if (process.PreOfferStage.Status != StageStatus.NA && process.PreOfferStage.Status != StageStatus.Declined && process.PreOfferStage.Status != StageStatus.Rejected)
            {
                return ProcessCurrentStage.PreOfferStage;
            }
            else if (process.ClientStage.Status != StageStatus.NA && process.ClientStage.Status != StageStatus.Declined && process.ClientStage.Status != StageStatus.Rejected)
            {
                return ProcessCurrentStage.ClientStage;
            }
            else if (process.TechnicalStage.Status != StageStatus.NA && process.TechnicalStage.Status != StageStatus.Declined && process.TechnicalStage.Status != StageStatus.Rejected)
            {
                return ProcessCurrentStage.TechnicalStage;
            }

            return ProcessCurrentStage.HrStage;
        }
    }
}
