using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model.Exceptions.Community
{
    public enum CommunityErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteCommunityNotFound,
        CommunityDeleted,
        InvalidUpdate,
        UpdateCommunityNotFound,
        UpdateHasNotChanges,
        CommunityNotFound
    }
}
