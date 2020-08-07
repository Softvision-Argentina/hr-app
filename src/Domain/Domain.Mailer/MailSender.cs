// <copyright file="MailSender.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Mailer
{
    using System.Threading.Tasks;
    using Mailer.Entities;
    using Mailer.Interfaces;
    using MailKit.Net.Smtp;

    public class MailSender : IMailSender
    {
        private readonly MailServerSettings mailSettings;

        public MailSender(MailServerSettings mailSettings)
        {
            this.mailSettings = mailSettings;
        }

        /// <inheritdoc cref="IMailSender"/>
        public void Send(Message message)
        {
            using (var client = new SmtpClient())
            {
                if (message.From.Count == 0)
                {
                    message.From.Add(new MailAddress(this.mailSettings.FromAlias, this.mailSettings.FromAddress));
                }

                client.Connect(this.mailSettings.Server, this.mailSettings.Port, this.mailSettings.SSLEnabled);
                client.Authenticate(this.mailSettings.Username, this.mailSettings.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        /// <inheritdoc cref="IMailSender"/>
        public async Task SendAsync(Message message)
        {
            using (var client = new SmtpClient())
            {
                if (message.From.Count == 0)
                {
                    message.From.Add(new MailAddress(this.mailSettings.FromAlias, this.mailSettings.FromAddress));
                }

                client.Connect(this.mailSettings.Server, this.mailSettings.Port, this.mailSettings.SSLEnabled);
                client.Authenticate(this.mailSettings.Username, this.mailSettings.Password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
