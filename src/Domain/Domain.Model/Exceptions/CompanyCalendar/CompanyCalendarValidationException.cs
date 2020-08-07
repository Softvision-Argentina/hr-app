// <copyright file="CompanyCalendarValidationException.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model.Exceptions.CompanyCalendar
{
    using System.Collections.Generic;
    using Core;

    public class CreateContractInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)CompanyCalendarValidationExceptionCodes.CreateContractInvalid;

        public CreateContractInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }

    public class UpdateContractInvalidException : BusinessValidationException
    {
        protected override int SubErrorCode => (int)CompanyCalendarValidationExceptionCodes.UpdateContractInvalid;

        public UpdateContractInvalidException(List<KeyValuePair<string, string>> messages) : base(messages)
        {
        }
    }
}
