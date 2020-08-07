// <copyright file="HashUtility.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core
{
    using System;

    public static class HashUtility
    {
        public static string GetStringSha256Hash(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return string.Empty;
                }

                using (var sha = new System.Security.Cryptography.SHA256Managed())
                {
                    byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                    byte[] hash = sha.ComputeHash(textData);
                    return BitConverter.ToString(hash).Replace("-", string.Empty);
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
