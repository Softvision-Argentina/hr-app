// <copyright file="Message.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Mailer.Entities
{
    using MimeKit;

    public class Message : MimeMessage
    {
        /// <summary>
        /// Creates a new message with an HTML body.
        /// </summary>
        /// <param name="to">To address.</param>
        /// <param name="subject">Mail Subject.</param>
        /// <param name="body">HTML body.</param>
        /// <param name="from">From address.</param>
        public Message(string to, string subject, MessageBody body, string from = "")
        {
            this.To.Add(new MailAddress(to));
            this.Subject = subject;
            this.Body = body.ToMessageBody();

            if (!string.IsNullOrEmpty(from))
            {
                this.From.Add(new MailAddress(from));
            }
        }

        /// <summary>
        /// Creates a new message with a text body.
        /// </summary>
        /// <param name="to">To address.</param>
        /// <param name="subject">Mail Subject.</param>
        /// <param name="text">Mail text.</param>
        /// <param name="from">From address.</param>
        public Message(string to, string subject, string text, string from = "")
        {
            this.To.Add(new MailAddress(to));
            this.Subject = subject;
            this.Body = new MessageText(text);

            if (!string.IsNullOrEmpty(from))
            {
                this.From.Add(new MailAddress(from));
            }
        }
    }
}
