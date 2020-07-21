using System;

namespace Core
{
    public static class HashUtility
    {
        public static string GetStringSha256Hash(string text)
        {
            try
            {
                if (String.IsNullOrEmpty(text))
                {
                    return String.Empty;
                }

                using (var sha = new System.Security.Cryptography.SHA256Managed())
                {
                    byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                    byte[] hash = sha.ComputeHash(textData);
                    return BitConverter.ToString(hash).Replace("-", String.Empty);
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
