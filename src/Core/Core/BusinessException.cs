using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class BusinessException : Exception
    {
        protected virtual int MainErrorCode => (int)ApplicationErrorMainCodes.Generic;

        protected virtual int SubErrorCode => 0;

        public int ErrorCode => MainErrorCode + SubErrorCode;

        public BusinessException(string message)
            : base(string.IsNullOrWhiteSpace(message) ? "There is an business related error" : message) { }

        public BusinessException(string message, Exception exception) : base(message, exception) { }
    }

    public class BusinessValidationException : BusinessException
    {
        List<KeyValuePair<string, string>> _validationMessages;
        public IReadOnlyList<KeyValuePair<string, string>> ValidationMessages => _validationMessages.AsReadOnly();

        protected override int MainErrorCode => (int)ApplicationErrorMainCodes.Validation;

        public BusinessValidationException(IEnumerable<KeyValuePair<string, string>> validationMessages)
            : base(string.Join("\r\n", validationMessages.Select(vm => vm.Value)))
        {
            _validationMessages = validationMessages.ToList();
        }
    }
}
