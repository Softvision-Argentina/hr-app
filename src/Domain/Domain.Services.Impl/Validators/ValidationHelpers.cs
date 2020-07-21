using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Services.Impl.Validators
{
    public static class ValidationHelpers
    {
        public static string ToClientMessage(this ValidationException exception)
        {
            StringBuilder sb = new StringBuilder();
            if (exception == null || exception.Errors.Count() == 0)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            foreach (var error in exception.Errors)
            {
                sb.AppendLine(error.ErrorMessage);
            }

            return sb.ToString();
        }

        public static List<KeyValuePair<string, string>> ToListOfMessages(this ValidationException exception)
        {
            if (exception == null || exception.Errors.Count() == 0)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return exception.Errors.Select(e => new KeyValuePair<string, string>(e.PropertyName, e.ErrorMessage)).ToList();
        }
    }
}
