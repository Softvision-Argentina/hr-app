using AutoMapper;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Process;
using Domain.Services.Interfaces.Repositories;
using Domain.Services.Interfaces.Services;
using Microsoft.Extensions.Configuration;
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
        private readonly INotificationRepository _notificationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _config;

        public ProcessService(IMapper mapper,
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
            IConfiguration config)
        {
            _userRepository = userRepository;
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
                .QueryEager();

            var candidateResult = candidateQuery.OrderByDescending(x => x.StartDate).ToList();

            return _mapper.Map<List<ReadedProcessContract>>(candidateResult);
        }

        public IEnumerable<ReadedProcessContract> GetActiveByCandidateId(int candidateId)
        {
            var process = _processRepository
                .QueryEager().Where(_ => _.CandidateId == candidateId && (_.Status == ProcessStatus.InProgress || _.Status == ProcessStatus.OfferAccepted || _.Status == ProcessStatus.Recall ));

            return _mapper.Map<IEnumerable<ReadedProcessContract>>(process);
        }

        public CreatedProcessContract Create(CreateProcessContract createProcessContract)
        {
            var process = _mapper.Map<Process>(createProcessContract);

            this.AddOfficeToCandidate(process.Candidate, createProcessContract.Candidate.PreferredOfficeId);

            process.Candidate.Status = CandidateStatus.InProgress;

            _candidateRepository.Update(process.Candidate);

            process.CurrentStage = SetProcessCurrentStage(process);

            var createdProcess = _processRepository.Create(process);

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

        private void AddCandidateProfileToCandidate(Candidate candidate, int profileID)
        {
            var profile = _candidateProfileRepository.Query().Where(_ => _.Id == profileID).FirstOrDefault();
            if (profile == null)
                throw new Domain.Model.Exceptions.User.UserNotFoundException(profileID);

            candidate.Profile = profile;
        }

        private void AddCommunityToCandidate(Candidate candidate, int communityID)
        {
            var community = _communityRepository.Query().Where(_ => _.Id == communityID).FirstOrDefault();
            if (community == null)
                throw new Domain.Model.Exceptions.User.UserNotFoundException(communityID);

            candidate.Community = community;
        }

        private void AddUserToCandidate(Candidate candidate, int userID)
        {

            var user = _userRepository.Query().Where(_ => _.Id == userID).FirstOrDefault();
            if (user == null)
                throw new Domain.Model.Exceptions.User.UserNotFoundException(userID);

            candidate.User = user;
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

            _hrStageRepository.Update(process.HrStage);
            _technicalStageRepository.Update(process.TechnicalStage);
            _clientStageRepository.Update(process.ClientStage);
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
                || process.Status == ProcessStatus.OfferAccepted || process.Status == ProcessStatus.Recall))
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

        public void Approve(int processID)
        {
            _processRepository.Approve(processID);

            var process = _processRepository.QueryEager().FirstOrDefault(p => p.Id == processID);

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

        public ProcessStatus SetProcessStatus(Process process)
        {
            switch (process.OfferStage.Status)
            {
                case StageStatus.NA:
                    switch (process.ClientStage.Status)
                    {
                        case StageStatus.NA:
                            switch (process.TechnicalStage.Status)
                            {
                                case StageStatus.NA:
                                    switch (process.HrStage.Status)
                                    {
                                        case StageStatus.NA:
                                            return ProcessStatus.NA;
                                        case StageStatus.InProgress:
                                            return ProcessStatus.InProgress;
                                        case StageStatus.Accepted:
                                            return ProcessStatus.InProgress;
                                        case StageStatus.Declined:
                                            return ProcessStatus.Declined;
                                        case StageStatus.Rejected:
                                            return ProcessStatus.Rejected;
                                        case StageStatus.Hired:
                                            return ProcessStatus.Hired;
                                        default:
                                            return ProcessStatus.NA;
                                    }
                                case StageStatus.InProgress:
                                    return ProcessStatus.InProgress;
                                case StageStatus.Accepted:
                                    return ProcessStatus.InProgress;
                                case StageStatus.Declined:
                                    return ProcessStatus.Declined;
                                case StageStatus.Rejected:
                                    return ProcessStatus.Rejected;
                                case StageStatus.Hired:
                                    return ProcessStatus.Hired;
                                default:
                                    return ProcessStatus.NA;
                            }
                        case StageStatus.InProgress:
                            return ProcessStatus.InProgress;
                        case StageStatus.Accepted:
                            return ProcessStatus.InProgress;
                        case StageStatus.Declined:
                            return ProcessStatus.Declined;
                        case StageStatus.Rejected:
                            return ProcessStatus.Rejected;
                        default:
                            return ProcessStatus.NA;
                    }
                case StageStatus.InProgress:
                    return ProcessStatus.InProgress;
                case StageStatus.Accepted:
                    return ProcessStatus.OfferAccepted;
                case StageStatus.Declined:
                    return ProcessStatus.Declined;
                case StageStatus.Rejected:
                    return ProcessStatus.Rejected;
                case StageStatus.Hired:
                    return ProcessStatus.Hired;
                default:
                    return ProcessStatus.NA;
            }
        }

        public CandidateStatus SetCandidateStatus(ProcessStatus processStatus)
        {
            switch (processStatus)
            {
                case ProcessStatus.NA:
                    return CandidateStatus.New;
                case ProcessStatus.InProgress:
                    return CandidateStatus.InProgress;
                case ProcessStatus.Recall:
                    return CandidateStatus.Recall;
                case ProcessStatus.Hired:
                    return CandidateStatus.Hired;
                case ProcessStatus.Rejected:
                    return CandidateStatus.Rejected;
                case ProcessStatus.Declined:
                    return CandidateStatus.Rejected;
                case ProcessStatus.OfferAccepted:
                    return CandidateStatus.InProgress;
                default:
                    return CandidateStatus.New;
            }
        }

        public ProcessCurrentStage SetProcessCurrentStage(Process process)
        {
            switch (process.HrStage.Status)
            {
                case StageStatus.NA:
                    return ProcessCurrentStage.NA;
                case StageStatus.InProgress:
                    return ProcessCurrentStage.HrStage;
                case StageStatus.Accepted:
                    switch (process.TechnicalStage.Status)
                    {
                        case StageStatus.NA:
                            return ProcessCurrentStage.TechnicalStage;
                        case StageStatus.InProgress:
                            return ProcessCurrentStage.TechnicalStage;
                        case StageStatus.Accepted:
                            switch (process.ClientStage.Status)
                            {
                                case StageStatus.NA:
                                    return ProcessCurrentStage.ClientStage;
                                case StageStatus.InProgress:
                                    return ProcessCurrentStage.ClientStage;
                                case StageStatus.Accepted:
                                    switch (process.OfferStage.Status)
                                    {
                                        case StageStatus.NA:
                                            return ProcessCurrentStage.OfferStage;
                                        case StageStatus.InProgress:
                                            return ProcessCurrentStage.OfferStage;
                                        case StageStatus.Accepted:
                                            return ProcessCurrentStage.OfferStage;
                                        default:
                                            return ProcessCurrentStage.Finished;
                                    }
                                default:
                                    return ProcessCurrentStage.Finished;
                            }
                        default:
                            return ProcessCurrentStage.Finished;
                    }
                default:
                    return ProcessCurrentStage.Finished;
            }
        }

    }
}
