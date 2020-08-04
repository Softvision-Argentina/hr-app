using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Domain.Model;
using static Domain.Model.ConfigurationKeys;

namespace Domain.Model
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public bool UseTestAuthentication { get; set; }
        public bool InMemoryDatabase { get; set; }
        public bool RunMigrations { get; set; }
        public bool RunSeed { get; set; }
        public bool MailSending { get; set; }
        public string Corswhitelist { get; set; }
        public MailSettings MailSettings { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public CommunityManagerEmails CommunityManagerEmails { get; set; }

    }
}
