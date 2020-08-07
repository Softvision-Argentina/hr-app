// <copyright file="AuditableExtensions.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.ExtensionHelpers
{
    using System;

    public static class AuditableExtensions
    {
        public static void AuditCreate(this IAuditable auditable, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidAuditableUsernameException();
            }

            auditable.CreatedBy = userName;
            auditable.CreatedDate = DateTime.UtcNow;

            auditable.LastModifiedBy = auditable.CreatedBy;
            auditable.LastModifiedDate = auditable.CreatedDate;
        }

        public static void AuditModify(this IAuditable auditable, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidAuditableUsernameException();
            }

            auditable.LastModifiedBy = userName;
            auditable.LastModifiedDate = DateTime.UtcNow;
        }
    }
}
