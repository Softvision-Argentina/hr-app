using System;

namespace Core.ExtensionHelpers
{
    public static class AuditableExtensions
    {
        public static void AuditCreate(this IAuditable auditable, string userName)
        {
            if (String.IsNullOrWhiteSpace(userName))
                throw new InvalidAuditableUsernameException();

            auditable.CreatedBy = userName;
            auditable.CreatedDate = DateTime.Now;

            auditable.LastModifiedBy = auditable.CreatedBy;
            auditable.LastModifiedDate = auditable.CreatedDate;
        }

        public static void AuditModify(this IAuditable auditable, string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new InvalidAuditableUsernameException();

            auditable.LastModifiedBy = userName;
            auditable.LastModifiedDate = DateTime.Now;
        }
    }
}
