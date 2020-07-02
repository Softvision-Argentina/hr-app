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
using Mailer.Interfaces;
using Mailer.Entities;
using Domain.Services.Impl.Validators;
using FluentValidation;
using Domain.Model.Exceptions.Process;

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
        private readonly IMailSender _mailSender;
        private readonly IValidator<UpdateProcessContract> _updateProcessContractValidator;
        private readonly IValidator<CreateProcessContract> _createProcessContractValidator;
        private readonly IReaddressStatusService _readdressStatusService;

        public ProcessService(
            IMapper mapper,
            IRepository<User> userRepository,
            IRepository<Candidate> candidateRepository,
            IRepository<CandidateProfile> candidateProfileRepository,
            IRepository<Community> communityRepository,
            IRepository<Office> officeRepository,
            IRepository<DeclineReason> declineReasonRepository,
            IRepository<ReaddressReason> readdressReasonRepository,
            IRepository<ReaddressStatus> readdressStatusRepository,
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
            IPreOfferStageRepository preOfferStageRepository,
            IMailSender mailSender,
            IValidator<UpdateProcessContract> updateProcessContractValidator,
            IValidator<CreateProcessContract> createProcessContractValidator,
            IReaddressStatusService readdressStatuService
            )
            
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
            _mailSender = mailSender;
            _createProcessContractValidator = createProcessContractValidator;
            _updateProcessContractValidator = updateProcessContractValidator;
            _readdressStatusService = readdressStatuService;
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
            ValidateContract(createProcessContract);

            var process = _mapper.Map<Process>(createProcessContract);
            
            ValidateDniExistance(process);
            
            AddOfficeToCandidate(process.Candidate, createProcessContract.Candidate.PreferredOfficeId);

            process.Candidate.Status = CandidateStatus.InProgress;

            _candidateRepository.Update(process.Candidate);

            process.Status = SetProcessStatus(process);
            process.CurrentStage = SetProcessCurrentStage(process);

            var userId = GetUser();

            process.UserOwnerId = userId;

            if (process.HrStage.ReaddressStatus != null)
                _readdressStatusService.Create(createProcessContract.HrStage.ReaddressStatus.ReaddressReasonId, process.HrStage.ReaddressStatus);
           
            if (process.TechnicalStage.ReaddressStatus != null)
                _readdressStatusService.Create(createProcessContract.TechnicalStage.ReaddressStatus.ReaddressReasonId, process.TechnicalStage.ReaddressStatus);

            if (process.ClientStage.ReaddressStatus != null)
                _readdressStatusService.Create(createProcessContract.ClientStage.ReaddressStatus.ReaddressReasonId, process.ClientStage.ReaddressStatus);

            if (process.PreOfferStage.ReaddressStatus != null)
                _readdressStatusService.Create(createProcessContract.PreOfferStage.ReaddressStatus.ReaddressReasonId, process.PreOfferStage.ReaddressStatus);

            var createdProcess = _processRepository.Create(process);

            process.HrStage.UserOwnerId = process.UserOwnerId;
            process.TechnicalStage.UserOwnerId = process.UserOwnerId;
            process.ClientStage.UserOwnerId = process.UserOwnerId;
            process.PreOfferStage.UserOwnerId = process.UserOwnerId;
            process.OfferStage.UserOwnerId = process.UserOwnerId;

            _unitOfWork.Complete();

            var createdProcessContract = _mapper.Map<CreatedProcessContract>(createdProcess);

            var status = process.Status;

            var mailSendingEnabled = _config.GetValue<bool>("MailSending");

            if (process.Candidate.ReferredBy != null && process.Status == ProcessStatus.InProgress)
            {
                var notification = new Notification
                {
                    Text = $"Your referral's {process.Candidate.Name} {process.Candidate.LastName} process status is {status}"
                };

                _notificationRepository.Create(notification, process.Candidate.Id);

                SendEmailNotification(process, status);
            }

            try
            {
                if (mailSendingEnabled)
                {
                    if (!process.HrStage.SentEmail)
                    {
                        if (process.HrStage.Status == StageStatus.Accepted && process.HrStage.UserOwner != null)
                        {
                            SendHrStageEmailNotification(process);
                            process.HrStage.SentEmail = true;
                        }
                    }

                    if (!process.TechnicalStage.SentEmail)
                    {
                        if (!string.IsNullOrEmpty(process.TechnicalStage.Feedback) && process.TechnicalStage.UserOwner != null && process.TechnicalStage.UserDelegate != null)
                        {
                            SendTechnicalStageEmailNotification(process);
                            process.TechnicalStage.SentEmail = true;
                        }
                    }
                }
            }catch(Exception ex)
            {
                throw new Exception("Mail could not been sent");
            }

            return createdProcessContract;
        }

        public void Update(UpdateProcessContract updateProcessContract)
        {
            ValidateContract(updateProcessContract);
            
            var process = _mapper.Map<Process>(updateProcessContract);
            
            ValidateDniExistance(process);

            process.Status = SetProcessStatus(process);
            process.CurrentStage = SetProcessCurrentStage(process);

            var candidate = _candidateRepository.QueryEager().FirstOrDefault(c => c.Id == process.Candidate.Id);
            candidate.EnglishLevel = process.HrStage.EnglishLevel;            
            candidate.Status = SetCandidateStatus(process.Status);
            process.Candidate = candidate;
            candidate.DNI = process.PreOfferStage.DNI;
            process.ClientStage.UserOwnerId = process.UserOwnerId;
            process.PreOfferStage.UserOwnerId = process.UserOwnerId;
            process.OfferStage.UserOwnerId = process.UserOwnerId;

            if (process.HrStage.ReaddressStatus != null)
                _readdressStatusService.Update(updateProcessContract.HrStage.ReaddressStatus.ReaddressReasonId, process.HrStage.ReaddressStatus);
            if (process.TechnicalStage.ReaddressStatus != null)
                _readdressStatusService.Update(updateProcessContract.TechnicalStage.ReaddressStatus.ReaddressReasonId, process.TechnicalStage.ReaddressStatus);
            if (process.ClientStage.ReaddressStatus != null)
                _readdressStatusService.Update(updateProcessContract.ClientStage.ReaddressStatus.ReaddressReasonId, process.ClientStage.ReaddressStatus);
            if (process.PreOfferStage.ReaddressStatus != null)
                _readdressStatusService.Update(updateProcessContract.PreOfferStage.ReaddressStatus.ReaddressReasonId, process.PreOfferStage.ReaddressStatus);

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


            try
            {
                var flag = _config.GetValue<bool>("MailSending");

                if (flag != false)
                {
                    if (!process.HrStage.SentEmail)
                    {
                        if (process.HrStage.Status == StageStatus.Accepted)
                        {
                            SendHrStageEmailNotification(process);
                            process.HrStage.SentEmail = true;
                        }
                    }

                    if (!process.TechnicalStage.SentEmail)
                    {
                        if (!string.IsNullOrEmpty(process.TechnicalStage.Feedback))
                        {
                            SendTechnicalStageEmailNotification(process);
                            process.TechnicalStage.SentEmail = true;
                        }
                    }
                }
            }catch(Exception ex)
            {
                throw new Exception("Mail could not been sent");
            }
            finally
            {
                _unitOfWork.Complete();
            }
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

        private int GetUser()
        {
            var getUser = _httpContext.HttpContext.User.Identity.Name;
            var userId = int.Parse(getUser);
            return userId;
        }

        private void ValidateDniExistance(Process process)
        {
            var DNIExists = _preOfferStageRepository.Query().Any(x => x.DNI == process.PreOfferStage.DNI && process.PreOfferStage.DNI != 0 && x.ProcessId != process.Id);
            if (DNIExists) throw new Exception("DNI number already exists");
        }

        private void SendEmailNotification(Process process, ProcessStatus status)
        {
            var email = GetUserMail(process.Candidate.ReferredBy);
            var messageBody = $"The process status of your referral, {process.Candidate.Name} {process.Candidate.LastName}, is now {status}.";
            var message = new Message(email, "Referral status", messageBody);
            _mailSender.SendAsync(message);
        }

        private void SendHrStageEmailNotification(Process process)
        {
            var email = _config.GetSection("CommunityManagerEmails").GetValue<string>(process.Candidate.Community.Name);

            if (process.Candidate.Community.Name == "Product Delivery")
            {
                email = (process.Candidate.Profile.Name == "Project Manager") ?
                    _config.GetSection("CommunityManagerEmails").GetValue<string>("Project Manager") :
                    _config.GetSection("CommunityManagerEmails").GetValue<string>("Product Delivery");
            }

            var messageBody = new MessageBody();
            messageBody.HtmlBody = $"Dear {process.Candidate.Community.Name}'s community manager, <br />" +
                    $"{process.Candidate.Name} {process.Candidate.LastName}, A new { process.Candidate.Profile.Name } candidate has been submitted for { process.Candidate.Community.Name } Community and is waiting for a Technical Interview on <a href='https://recruiting.softvision-ar.com/'>RECRU</a>. <br />" +
                    $"Please reach out to {process.Candidate.User.FirstName} {process.Candidate.User.LastName} with Interviewer name / s and availability. <br />" +
                    "Thank you.";
            var message = new Message(email, "New candidate for Interview!", messageBody);
            _mailSender.SendAsync(message);
        }

        private void SendTechnicalStageEmailNotification(Process process)
        {
            var userToSend =  _userRepository.QueryEager().FirstOrDefault(x => x.Username == process.UserOwner.Username);
            var email = userToSend.Username;
            var interviewer = _userRepository.QueryEager().FirstOrDefault(x => x.Id == process.TechnicalStage.UserOwnerId);
            var delegateInterviewer = _userRepository.QueryEager().FirstOrDefault(x => x.Id == process.TechnicalStage.UserDelegateId);
            var skills = process.Candidate.CandidateSkills;
            var skillsListed = skills.ToList();

            var messageBody = new MessageBody();
            messageBody.HtmlBody = $"Dear {process.Candidate.User.FirstName} {process.Candidate.User.LastName}, <br />" +
                $"A technical feedback of {process.Candidate.Name} {process.Candidate.LastName}, interviewed by {interviewer.FirstName} {interviewer.LastName} {(delegateInterviewer != null ? "and" + delegateInterviewer.FirstName + " " + delegateInterviewer.LastName + " " : "")}on {process.TechnicalStage.Date} is now available on <a href='https://recruiting.softvision-ar.com/'>RECRU</a>. <br />" +
                $"You can find some information about the technical stage: Status: {process.TechnicalStage.Status}, Seniority: {process.TechnicalStage.Seniority} and Alternative Seniority: {process.TechnicalStage.AlternativeSeniority}. <br />" +
                $"Thank you";
            var message = new Message(email, $"Feedback for {process.Candidate.Name} {process.Candidate.LastName} is now available!", messageBody);
            _mailSender.SendAsync(message);
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
            if (process.OfferStage.Status != StageStatus.NA)
            {
                return ProcessCurrentStage.OfferStage;
            }
            else if (process.PreOfferStage.Status != StageStatus.NA)
            {
                return ProcessCurrentStage.PreOfferStage;
            }
            else if (process.ClientStage.Status != StageStatus.NA)
            {
                return ProcessCurrentStage.ClientStage;
            }
            else if (process.TechnicalStage.Status != StageStatus.NA)
            {
                return ProcessCurrentStage.TechnicalStage;
            }

            return ProcessCurrentStage.HrStage;
        }

        private void ValidateContract(CreateProcessContract contract)
        {
            try
            {
                _createProcessContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateProcessInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateProcessContract contract)
        {
            try
            {
                _updateProcessContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_UPDATE}");
            }
            catch (ValidationException ex)
            {
                throw new UpdateProcessInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
