using System;

namespace Core
{
    public class CoreExceptionConstants
    {
        public static readonly string InvalidAuditableUserName = "Argument cannot be null, empty or white spaces";
    }

    public class InvalidSettingException : Exception
    {
        public InvalidSettingException(string message) : base(message) { }
    }

    public class InvalidAuditableUsernameException : Exception
    {
        public InvalidAuditableUsernameException() : base(CoreExceptionConstants.InvalidAuditableUserName) { }
    }

}
