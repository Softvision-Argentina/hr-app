using ApiServer.Contracts.Interview;
using AutoMapper;
using Domain.Services.Contracts.Interview;

namespace ApiServer.Profiles
{
    public class InterviewProfile : Profile
    {
        public InterviewProfile()
        {
            CreateMap<CreateInterviewViewModel, CreateInterviewContract>();
            CreateMap<CreatedInterviewContract, CreatedInterviewViewModel>();
            CreateMap<ReadedInterviewContract, ReadedInterviewViewModel>();
            CreateMap<UpdateInterviewViewModel, UpdateInterviewContract>();
        }
    }
}
