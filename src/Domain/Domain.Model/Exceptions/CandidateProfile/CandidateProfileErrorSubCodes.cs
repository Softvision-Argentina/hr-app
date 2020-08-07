// <copyright file="CandidateProfileErrorSubCodes.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.CandidateProfile
{
    public enum CandidateProfileErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteCandidateProfileNotFound,
        CandidateProfileDeleted,
        InvalidUpdate,
        UpdateCandidateProfileNotFound,
        UpdateHasNotChanges,
        CandidateProfileNotFound,
    }
}
