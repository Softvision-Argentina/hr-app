using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class CoreExceptionConstants
    {
        public static readonly string InvalidSettingMessage = "Invalid setting; {0}.";
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
