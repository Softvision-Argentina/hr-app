
using Domain.Model;
using Domain.Services.Contracts.Interview;
using System;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IInterviewService
    {
        CreatedInterviewContract Create(CreateInterviewContract contract);
        ReadedInterviewContract Read(int id);
        void Update(UpdateInterviewContract contract);
        void Delete(int id);
        IEnumerable<ReadedInterviewContract> List();
    }
}