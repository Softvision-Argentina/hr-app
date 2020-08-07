// <copyright file="IMailSender.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Mailer.Interfaces
{
    using System.Threading.Tasks;
    using Mailer.Entities;

    public interface IMailSender
    {
        void Send(Message message);

        Task SendAsync(Message message);
    }
}
