// <copyright file="InterviewProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Interview;

    public class InterviewProfile : Profile
    {
        public InterviewProfile()
        {
            this.CreateMap<Interview, ReadedInterviewContract>()
                .ForMember(x => x.Feedback, opt => opt.MapFrom(r => r.Feedback))
                .ForMember(x => x.ClientInterviewer, opt => opt.MapFrom(r => r.ClientInterviewer))
                .ForMember(x => x.Client, opt => opt.MapFrom(r => r.Client))
                .ForMember(x => x.Project, opt => opt.MapFrom(r => r.Project))
                .ForMember(x => x.InterviewDate, opt => opt.MapFrom(r => r.InterviewDate))
                .ForMember(x => x.ClientStage, opt => opt.MapFrom(r => r.ClientStage))
                .ForMember(x => x.ClientStageId, opt => opt.MapFrom(r => r.ClientStageId));

            this.CreateMap<CreateInterviewContract, Interview>()
                .ForMember(x => x.Feedback, opt => opt.MapFrom(r => r.Feedback))
                .ForMember(x => x.ClientInterviewer, opt => opt.MapFrom(r => r.ClientInterviewer))
                .ForMember(x => x.Client, opt => opt.MapFrom(r => r.Client))
                .ForMember(x => x.Project, opt => opt.MapFrom(r => r.Project))
                .ForMember(x => x.InterviewDate, opt => opt.MapFrom(r => r.InterviewDate))
                .ForMember(x => x.ClientStage, opt => opt.MapFrom(r => r.ClientStage))
                .ForMember(x => x.ClientStageId, opt => opt.MapFrom(r => r.ClientStageId));

            this.CreateMap<Interview, CreateInterviewContract>()
                .ForMember(x => x.Feedback, opt => opt.MapFrom(r => r.Feedback))
                .ForMember(x => x.ClientInterviewer, opt => opt.MapFrom(r => r.ClientInterviewer))
                .ForMember(x => x.Client, opt => opt.MapFrom(r => r.Client))
                .ForMember(x => x.Project, opt => opt.MapFrom(r => r.Project))
                .ForMember(x => x.InterviewDate, opt => opt.MapFrom(r => r.InterviewDate))
                .ForMember(x => x.ClientStage, opt => opt.MapFrom(r => r.ClientStage))
                .ForMember(x => x.ClientStageId, opt => opt.MapFrom(r => r.ClientStageId));

            this.CreateMap<Interview, CreatedInterviewContract>()
                .ForMember(x => x.Id, opt => opt.MapFrom(r => r.Id));

            this.CreateMap<Interview, ReadedInterviewAppContract>()
                .ForMember(x => x.Feedback, opt => opt.MapFrom(r => r.Feedback))
                .ForMember(x => x.ClientInterviewer, opt => opt.MapFrom(r => r.ClientInterviewer))
                .ForMember(x => x.Client, opt => opt.MapFrom(r => r.Client))
                .ForMember(x => x.Project, opt => opt.MapFrom(r => r.Project))
                .ForMember(x => x.InterviewDate, opt => opt.MapFrom(r => r.InterviewDate));

            this.CreateMap<UpdateInterviewContract, Interview>()
                .ForMember(x => x.Feedback, opt => opt.MapFrom(r => r.Feedback))
                .ForMember(x => x.ClientInterviewer, opt => opt.MapFrom(r => r.ClientInterviewer))
                .ForMember(x => x.Client, opt => opt.MapFrom(r => r.Client))
                .ForMember(x => x.Project, opt => opt.MapFrom(r => r.Project))
                .ForMember(x => x.InterviewDate, opt => opt.MapFrom(r => r.InterviewDate))
                .ForMember(x => x.ClientStage, opt => opt.MapFrom(r => r.ClientStage))
                .ForMember(x => x.ClientStageId, opt => opt.MapFrom(r => r.ClientStageId));
        }
    }
}
