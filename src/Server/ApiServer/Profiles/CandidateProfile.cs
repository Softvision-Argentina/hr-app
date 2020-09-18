// <copyright file="CandidateProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Candidates;
    using AutoMapper;
    using Domain.Services.Contracts.Candidate;

    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            this.CreateMap<CreateCandidateViewModel, CreateCandidateContract>();
            this.CreateMap<CreatedCandidateContract, CreatedCandidateViewModel>();
            this.CreateMap<ReadedCandidateContract, ReadedCandidateViewModel>();
            this.CreateMap<ReadedCandidateAppContract, ReadedCandidateAppViewModel>();
            this.CreateMap<UpdateCandidateViewModel, UpdateCandidateContract>();
        }
    }
}
