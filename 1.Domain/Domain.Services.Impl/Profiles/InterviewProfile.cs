using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Interview;

namespace Domain.Services.Impl.Profiles
{
    public class InterviewProfile : Profile
    {
        public InterviewProfile()
        {
            CreateMap<Interview, ReadedInterviewContract>()
                .ForMember(x => x.Feedback, opt => opt.MapFrom(r => r.Feedback))
                .ForMember(x => x.ClientInterviewer, opt => opt.MapFrom(r => r.ClientInterviewer))
                .ForMember(x => x.Client, opt => opt.MapFrom(r => r.Client))
                .ForMember(x => x.Project, opt => opt.MapFrom(r => r.Project))
                .ForMember(x => x.InterviewDate, opt => opt.MapFrom(r => r.InterviewDate));

            CreateMap<CreateInterviewContract, Interview>()
                .ForMember(x => x.Feedback, opt => opt.MapFrom(r => r.Feedback))
                .ForMember(x => x.ClientInterviewer, opt => opt.MapFrom(r => r.ClientInterviewer))
                .ForMember(x => x.Client, opt => opt.MapFrom(r => r.Client))
                .ForMember(x => x.Project, opt => opt.MapFrom(r => r.Project))
                .ForMember(x => x.InterviewDate, opt => opt.MapFrom(r => r.InterviewDate));

            CreateMap<Interview, CreateInterviewContract>()
                .ForMember(x => x.Feedback, opt => opt.MapFrom(r => r.Feedback))
                .ForMember(x => x.ClientInterviewer, opt => opt.MapFrom(r => r.ClientInterviewer))
                .ForMember(x => x.Client, opt => opt.MapFrom(r => r.Client))
                .ForMember(x => x.Project, opt => opt.MapFrom(r => r.Project))
                .ForMember(x => x.InterviewDate, opt => opt.MapFrom(r => r.InterviewDate));

            CreateMap<Interview, CreatedInterviewContract>()
                .ForMember(x => x.Id, opt => opt.MapFrom(r => r.Id));

            CreateMap<Interview, ReadedInterviewAppContract>()
                .ForMember(x => x.Feedback, opt => opt.MapFrom(r => r.Feedback))
                .ForMember(x => x.ClientInterviewer, opt => opt.MapFrom(r => r.ClientInterviewer))
                .ForMember(x => x.Client, opt => opt.MapFrom(r => r.Client))
                .ForMember(x => x.Project, opt => opt.MapFrom(r => r.Project))
                .ForMember(x => x.InterviewDate, opt => opt.MapFrom(r => r.InterviewDate));

            CreateMap<UpdateInterviewContract, Interview>()
                .ForMember(x => x.Feedback, opt => opt.MapFrom(r => r.Feedback))
                .ForMember(x => x.ClientInterviewer, opt => opt.MapFrom(r => r.ClientInterviewer))
                .ForMember(x => x.Client, opt => opt.MapFrom(r => r.Client))
                .ForMember(x => x.Project, opt => opt.MapFrom(r => r.Project))
                .ForMember(x => x.InterviewDate, opt => opt.MapFrom(r => r.InterviewDate));
        }
    }
}
