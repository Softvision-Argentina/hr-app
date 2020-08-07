// <copyright file="CandidateErrorSubCodes.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.Candidate
{
    public enum CandidateErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteCandidateNotFound,
        CandidateDeleted,
        InvalidUpdate,
        UpdateCandidateNotFound,
        UpdateHasNotChanges,
        CandidateNotFound,
    }
}
