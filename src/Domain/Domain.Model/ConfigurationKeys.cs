namespace Domain.Model
{
    public class ConfigurationKeys
    {
        public class LogLevel
        {
            public string Default { get; set; }

            public string System { get; set; }

            public string Microsoft { get; set; }
        }

        public class LogLevel2
        {
            public string Default { get; set; }
        }

        public class Console
        {
            public LogLevel2 LogLevel { get; set; }
        }

        public class Logging
        {
            public bool IncludeScopes { get; set; }

            public LogLevel LogLevel { get; set; }

            public Console Console { get; set; }
        }

        public class ConnectionStrings
        {
            public string SeedDB { get; set; }

            public string SeedDBTesting { get; set; }
        }

        public class MailSettings
        {
            public string Server { get; set; }

            public int Port { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }

            public string FromAlias { get; set; }

            public string FromAddress { get; set; }

            public bool SSLEnabled { get; set; }

            public bool DefaultCredentials { get; set; }
        }

        public class JwtSettings
        {
            public string Key { get; set; }

            public string Issuer { get; set; }

            public string Audience { get; set; }

            public string MinutesToExpiration { get; set; }
        }

        public class CommunityManagerEmails
        {
            public string EnterpriseNet { get; set; }

            public string EnterpriseCoffee { get; set; }

            public string DesignUX { get; set; }

            public string Web { get; set; }

            public string DevOps { get; set; }

            public string ProductDelivery { get; set; }

            public string ProjectManager { get; set; }

            public string ITSupport { get; set; }
        }
    }
}
