// <copyright file="CandidateProfileProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.CandidateProfiles
{
    using ApiServer.Contracts.CandidateProfile;
    using ApiServer.Profiles;
    using Domain.Services.Contracts.CandidateProfile;

    public class CandidateProfileProfile : CandidateProfile
    {
        public CandidateProfileProfile()
        {
            this.CreateMap<CreateCandidateProfileViewModel, CreateCandidateProfileContract>();
            this.CreateMap<CreatedCandidateProfileContract, CreatedCandidateProfileViewModel>();
            this.CreateMap<ReadedCandidateProfileContract, ReadedCandidateProfileViewModel>();
            this.CreateMap<UpdateCandidateProfileViewModel, UpdateCandidateProfileContract>();
        }
    }
}
