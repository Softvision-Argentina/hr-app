// <copyright file="MessageText.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Mailer.Entities
{
    using MimeKit;

    public class MessageText : TextPart
    {
        public MessageText()
        {
        }

        public MessageText(string text)
        {
            this.Text = text;
        }
    }
}
