﻿namespace Domain.Services.Impl.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AutoMapper;
    using Core.ExtensionHelpers;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Model.Exceptions.Process;
    using Domain.Services.Contracts.Process;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Interfaces.Repositories;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;
    using Mailer.Entities;
    using Mailer.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    public class ProcessService : IProcessService
    {
        private readonly IMapper mapper;
        private readonly IProcessRepository processRepository;
        private readonly IProcessStageRepository processStageRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Community> communityRepository;
        private readonly IRepository<CandidateProfile> candidateProfileRepository;
        private readonly IRepository<Candidate> candidateRepository;
        private readonly IRepository<Office> officeRepository;
        private readonly IRepository<DeclineReason> declineReasonRepository;
        private readonly IHrStageRepository hrStageRepository;
        private readonly ITechnicalStageRepository technicalStageRepository;
        private readonly IClientStageRepository clientStageRepository;
        private readonly IOfferStageRepository offerStageRepository;
        private readonly IPreOfferStageRepository preOfferStageRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly IRepository<User> userRepository;
        private readonly IConfiguration config;
        private readonly IHttpContextAccessor httpContext;
        private readonly IMailSender mailSender;
        private readonly IValidator<UpdateProcessContract> updateProcessContractValidator;
        private readonly IValidator<CreateProcessContract> createProcessContractValidator;
        private readonly IReaddressStatusService readdressStatusService;
        private readonly IOptions<AppSettings> appSettings;

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
            IReaddressStatusService readdressStatuService,
            IOptions<AppSettings> appSettings)
        {
            this.candidateRepository = candidateRepository;
            this.candidateProfileRepository = candidateProfileRepository;
            this.communityRepository = communityRepository;
            this.officeRepository = officeRepository;
            this.declineReasonRepository = declineReasonRepository;
            this.mapper = mapper;
            this.processRepository = processRepository;
            this.processStageRepository = processStageRepository;
            this.hrStageRepository = hrStageRepository;
            this.technicalStageRepository = technicalStageRepository;
            this.clientStageRepository = clientStageRepository;
            this.offerStageRepository = offerStageRepository;
            this.unitOfWork = unitOfWork;
            this.notificationRepository = notificationRepository;
            this.userRepository = userRepository;
            this.config = config;
            this.httpContext = httpContext;
            this.preOfferStageRepository = preOfferStageRepository;
            this.mailSender = mailSender;
            this.createProcessContractValidator = createProcessContractValidator;
            this.updateProcessContractValidator = updateProcessContractValidator;
            this.readdressStatusService = readdressStatuService;
            this.appSettings = appSettings;
        }

        public ReadedProcessContract Read(int id)
        {
            var process = this.processRepository
                .QueryEager().SingleOrDefault(_ => _.Id == id);

            return this.mapper.Map<ReadedProcessContract>(process);
        }

        public IEnumerable<ReadedProcessContract> GetProcessesByCommunity(string community)
        {
            var candidateQuery = this.processRepository
                .QueryEager().Where(pro => pro.Candidate.Community.Name.Equals(community) && pro.Status != ProcessStatus.Eliminated);

            var candidateResult = candidateQuery.ToList();

            return this.mapper.Map<List<ReadedProcessContract>>(candidateResult);
        }

        public void Delete(int id)
        {
            var process = this.processRepository.QueryEager().FirstOrDefault(p => p.Id == id);

            process.Candidate.Status = this.SetCandidateStatus(ProcessStatus.Rejected);

            process.Status = ProcessStatus.Eliminated;

            this.processRepository.Update(process);

            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedProcessContract> List()
        {
            var candidateQuery = this.processRepository
                .QueryEager().Where(_ => _.Status != ProcessStatus.Eliminated).ToList();

            var candidateResult = candidateQuery.OrderByDescending(x => x.StartDate).ToList();

            return this.mapper.Map<List<ReadedProcessContract>>(candidateResult);
        }

        public IEnumerable<ReadedProcessContract> GetDeletedProcesses()
        {
            var candidateQuery = this.processRepository
                    .QueryEager().Where(_ => _.Status == ProcessStatus.Eliminated);

            var candidateResult = candidateQuery.OrderByDescending(x => x.StartDate).ToList();

            return this.mapper.Map<List<ReadedProcessContract>>(candidateResult);
        }

        public IEnumerable<ReadedProcessContract> GetActiveByCandidateId(int candidateId)
        {
            var process = this.processRepository
                .QueryEager().Where(_ => _.CandidateId == candidateId && (_.Status == ProcessStatus.InProgress || _.Status == ProcessStatus.Accepted || _.Status == ProcessStatus.Recall));

            return this.mapper.Map<IEnumerable<ReadedProcessContract>>(process);
        }

        public Process Create(CreateProcessContract createProcessContract)
        {
            this.ValidateContract(createProcessContract);

            var process = this.mapper.Map<Process>(createProcessContract);

            this.ValidateDniExistance(process);

            this.AddOfficeToCandidate(process.Candidate, createProcessContract.Candidate.PreferredOfficeId);

            process.Candidate.Status = CandidateStatus.InProgress;

            if (process.Candidate.LinkedInProfile != null)
            {
                process.Candidate.LinkedInProfile = RegexExtensions.GetLinkedInUsername(process.Candidate.LinkedInProfile);
            }

            this.candidateRepository.Update(process.Candidate);

            process.Status = this.SetProcessStatus(process);
            process.CurrentStage = this.SetProcessCurrentStage(process);

            if (process.HrStage.ReaddressStatus != null)
            {
                this.readdressStatusService.Create(createProcessContract.HrStage.ReaddressStatus.ReaddressReasonId, process.HrStage.ReaddressStatus);
            }

            if (process.TechnicalStage.ReaddressStatus != null)
            {
                this.readdressStatusService.Create(createProcessContract.TechnicalStage.ReaddressStatus.ReaddressReasonId, process.TechnicalStage.ReaddressStatus);
            }

            if (process.ClientStage.ReaddressStatus != null)
            {
                this.readdressStatusService.Create(createProcessContract.ClientStage.ReaddressStatus.ReaddressReasonId, process.ClientStage.ReaddressStatus);
            }

            if (process.PreOfferStage.ReaddressStatus != null)
            {
                this.readdressStatusService.Create(createProcessContract.PreOfferStage.ReaddressStatus.ReaddressReasonId, process.PreOfferStage.ReaddressStatus);
            }

            // process.Candidate.Community = this.AddCommunityToProcess(process.Candidate.Community.Id);
            // process.Candidate.Profile = this.AddProfileToProcess(process.Candidate.Profile.Id);
            var createdProcess = this.processRepository.Create(process);

            process.HrStage.UserOwnerId = process.UserOwnerId;
            process.TechnicalStage.UserOwnerId = process.UserOwnerId;
            process.ClientStage.UserOwnerId = process.UserOwnerId;
            process.PreOfferStage.UserOwnerId = process.UserOwnerId;
            process.OfferStage.UserOwnerId = process.UserOwnerId;

            this.unitOfWork.Complete();

            // var createdProcessContract = this.mapper.Map<CreatedProcessContract>(createdProcess);

            var status = process.Status;

            var mailSendingEnabled = this.appSettings.Value.MailSending;

            try
            {
                if (mailSendingEnabled)
                {
                    if (!process.HrStage.SentEmail)
                    {
                        if (process.HrStage.Status == StageStatus.Accepted && process.HrStage.UserOwner != null)
                        {
                            this.SendHrStageEmailNotification(process);
                            process.HrStage.SentEmail = true;
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Mail could not been sent");
            }

            return createdProcess;
        }

        public Process Update(UpdateProcessContract updateProcessContract)
        {
            this.ValidateContract(updateProcessContract);

            var process = this.mapper.Map<Process>(updateProcessContract);

            this.ValidateDniExistance(process);

            process.Status = this.SetProcessStatus(process);
            process.CurrentStage = this.SetProcessCurrentStage(process);

            var candidate = this.candidateRepository.QueryEager().FirstOrDefault(c => c.Id == process.Candidate.Id);
            candidate.EnglishLevel = process.HrStage.EnglishLevel;
            candidate.Status = this.SetCandidateStatus(process.Status);
            process.Candidate = candidate;
            candidate.DNI = process.PreOfferStage.DNI;
            process.ClientStage.UserOwnerId = process.UserOwnerId;
            process.PreOfferStage.UserOwnerId = process.UserOwnerId;
            process.OfferStage.UserOwnerId = process.UserOwnerId;

            if (process.HrStage.ReaddressStatus != null)
            {
                this.readdressStatusService.Update(updateProcessContract.HrStage.ReaddressStatus.ReaddressReasonId, process.HrStage.ReaddressStatus);
            }

            if (process.TechnicalStage.ReaddressStatus != null)
            {
                this.readdressStatusService.Update(updateProcessContract.TechnicalStage.ReaddressStatus.ReaddressReasonId, process.TechnicalStage.ReaddressStatus);
            }

            if (process.ClientStage.ReaddressStatus != null)
            {
                this.readdressStatusService.Update(updateProcessContract.ClientStage.ReaddressStatus.ReaddressReasonId, process.ClientStage.ReaddressStatus);
            }

            if (process.PreOfferStage.ReaddressStatus != null)
            {
                this.readdressStatusService.Update(updateProcessContract.PreOfferStage.ReaddressStatus.ReaddressReasonId, process.PreOfferStage.ReaddressStatus);
            }

            this.hrStageRepository.Update(process.HrStage);
            this.technicalStageRepository.Update(process.TechnicalStage);
            this.clientStageRepository.Update(process.ClientStage);
            this.preOfferStageRepository.Update(process.PreOfferStage);
            this.offerStageRepository.Update(process.OfferStage);

            if(process.DeclineReasonId != null)
            {
                if (process.DeclineReason.Id == -1)
                {
                    process.DeclineReason = this.declineReasonRepository.Create(new DeclineReason
                    {
                        Name = "Other",
                        Description = updateProcessContract.DeclineReason.Description,
                    });
                    process.DeclineReasonId = process.DeclineReason.Id;
                }
                else
                {
                    process.DeclineReason = this.declineReasonRepository.Get(process.DeclineReason.Id);
                }
            }

            var updatedProcess = this.processRepository.Update(process);
            var status = process.Status;

            try
            {
                var flag = this.appSettings.Value.MailSending;

                if (flag != false)
                {
                    if (!process.HrStage.SentEmail)
                    {
                        if (process.HrStage.Status == StageStatus.Accepted)
                        {
                            this.SendHrStageEmailNotification(process);
                            process.HrStage.SentEmail = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Mail could not been sent");
            }
            finally
            {
                this.unitOfWork.Complete();
            }

            return updatedProcess;
        }

        public Process Approve(int processId)
        {
            this.processRepository.Approve(processId);
            var process = this.processRepository.QueryEager().FirstOrDefault(p => p.Id == processId);
            process.Candidate.Status = this.SetCandidateStatus(process.Status);
            this.unitOfWork.Complete();
            return process;
        }

        public void Reject(int id, string rejectionReason)
        {
            this.processRepository.Reject(id, rejectionReason);
            var process = this.processRepository.QueryEager().FirstOrDefault(p => p.Id == id);
            process.Candidate.Status = this.SetCandidateStatus(process.Status);
            var status = process.Status;

            this.unitOfWork.Complete();
        }

        public ReadedProcessContract Reactivate(int processId)
        {
            var process = this.processRepository.QueryEager().FirstOrDefault(p => p.Id == processId);
            process.Status = SetProcessStatus(process);
            process.Candidate.Status = process.Candidate.Status == CandidateStatus.Rejected || process.Candidate.Status == CandidateStatus.Eliminated ? SetCandidateStatus(process.Status) : process.Candidate.Status;
            this.processRepository.Update(process);
            this.unitOfWork.Complete();
            // return process;
            return this.mapper.Map<ReadedProcessContract>(process);
        }

        private void ValidateDniExistance(Process process)
        {
            var dNIExists = this.preOfferStageRepository.Query().Any(x => x.DNI == process.PreOfferStage.DNI && process.PreOfferStage.DNI != 0 && x.ProcessId != process.Id);
            if (dNIExists)
            {
                throw new Exception("DNI number already exists");
            }
        }

        private void SendHrStageEmailNotification(Process process)
        {
            if (!string.IsNullOrEmpty(process.Candidate.Community.Name) && process.Candidate.Community.Id > 0)
            {
                var email = this.config.GetSection("CommunityManagerEmails").GetValue<string>(process.Candidate.Community.Name);

                if (process.Candidate.Community.Name == "ProductDelivery")
                {
                    email = (process.Candidate.Profile.Name == "ProjectManager") ?
                        this.appSettings.Value.CommunityManagerEmails.ProjectManager :
                        this.appSettings.Value.CommunityManagerEmails.ProductDelivery;
                }

                var messageBody = new MessageBody();
                messageBody.HtmlBody = $"Dear {process.Candidate.Community.Name}'s community manager, <br />" +
                        $"{process.Candidate.Name} {process.Candidate.LastName}, A new candidate has been submitted for {process.Candidate.Community.Name } Community and is waiting for a Technical Interview on <a href='https://recruiting.softvision-ar.com/'>RECRU</a>. <br />" +
                        $"Please reach out to {process.UserOwner.FirstName} {process.UserOwner.LastName} with Interviewer name / s and availability. <br />" +
                        "Thank you.";
                var message = new Message(email, "New candidate for Interview!", messageBody);
                this.mailSender.SendAsync(message);
            }
        }

        private void AddOfficeToCandidate(Candidate candidate, int officeId)
        {
            var office = this.officeRepository.Query().Where(_ => _.Id == officeId).FirstOrDefault();
            if (office == null)
            {
                throw new Domain.Model.Exceptions.Office.OfficeNotFoundException(officeId);
            }

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
                this.createProcessContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
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
                this.updateProcessContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETUPDATE}");
            }
            catch (ValidationException ex)
            {
                throw new UpdateProcessInvalidException(ex.ToListOfMessages());
            }
        }

        private Community AddCommunityToProcess(int communityId)
        {
            var community = this.communityRepository.Query().Where(_ => _.Id == communityId).FirstOrDefault();
            if (community == null)
            {
                throw new Domain.Model.Exceptions.Community.CommunityNotFoundException(communityId);
            }

            return community;
        }

        private CandidateProfile AddProfileToProcess(int profileId)
        {
            var profile = this.candidateProfileRepository.Query().Where(_ => _.Id == profileId).FirstOrDefault();
            if (profile == null)
            {
                throw new Domain.Model.Exceptions.CandidateProfile.CandidateProfileNotFoundException(profileId);
            }

            return profile;
        }
    }
}
