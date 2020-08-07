// <copyright file="MailAddress.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Mailer.Entities
{
    using MimeKit;

    public class MailAddress : MailboxAddress
    {
        /// <inheritdoc cref="MailboxAddress"/>
        public MailAddress(string address) : base(address)
        {
        }

        /// <inheritdoc cref="MailboxAddress"/>
        public MailAddress(string alias, string address) : base(alias, address)
        {
        }
    }
}
