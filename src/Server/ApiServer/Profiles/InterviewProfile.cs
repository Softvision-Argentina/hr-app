// <copyright file="InterviewProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Interview;
    using AutoMapper;
    using Domain.Services.Contracts.Interview;

    public class InterviewProfile : Profile
    {
        public InterviewProfile()
        {
            this.CreateMap<CreateInterviewViewModel, CreateInterviewContract>();
            this.CreateMap<CreatedInterviewContract, CreatedInterviewViewModel>();
            this.CreateMap<ReadedInterviewContract, ReadedInterviewViewModel>();
            this.CreateMap<UpdateInterviewViewModel, UpdateInterviewContract>();
        }
    }
}
