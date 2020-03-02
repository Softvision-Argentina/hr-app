using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model.Exceptions
{
    public enum DeclineReasonErrorSubCodes
    {
        InvalidUpdateStatus,
        DeleteDeclineReasonNotFound,
        DeclineReasonDeleted,
        InvalidUpdate,
        UpdateDeclineReasonNotFound,
        UpdateHasNotChanges,
        DeclineReasonNotFound
    }
}
