// <copyright file="CandidateProfileProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.CandidateProfiles
{
    using Domain.Model;
    using Domain.Services.Contracts.CandidateProfile;

    public class CandidateProfileProfile : AutoMapper.Profile
    {
        public CandidateProfileProfile()
        {
            this.CreateMap<CandidateProfile, ReadedCandidateProfileContract>();
            this.CreateMap<ReadedCandidateProfileContract, CandidateProfile>();
            this.CreateMap<CreateCandidateProfileContract, CandidateProfile>();
            this.CreateMap<CandidateProfile, CreatedCandidateProfileContract>();
            this.CreateMap<UpdateCandidateProfileContract, CandidateProfile>();
        }
    }
}
