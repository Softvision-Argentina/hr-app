// <copyright file="RegexExtensions.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.ExtensionHelpers
{
    using System.Text.RegularExpressions;

    public static class RegexExtensions
    {
        public static string GetLinkedInUsername(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }

            var match = Regex.Match(url, @"linkedin\.com\/in\/(?<userId>[^\/]+)");
            if (match.Success)
            {
                return match.Groups["userId"].Value;
            }

            return url;
        }
    }
}
