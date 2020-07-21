using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.ExtensionHelpers
{
    public static class RegexExtensions
    {
        public static string GetLinkedInUsername(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            var match = Regex.Match(url, @"linkedin\.com\/in\/(?<userId>[^\/]+)");
            if (match.Success)
            {
                return match.Groups["userId"].Value;
            }

            return url;
        }
    }
}
