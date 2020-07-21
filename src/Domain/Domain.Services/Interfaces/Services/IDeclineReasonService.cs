using Domain.Services.Contracts;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IDeclineReasonService
    {
        CreatedDeclineReasonContract Create(CreateDeclineReasonContract contract);
        ReadedDeclineReasonContract Read(int id);
        void Update(UpdateDeclineReasonContract contract);
        void Delete(int id);
        IEnumerable<ReadedDeclineReasonContract> List();
        IEnumerable<ReadedDeclineReasonContract> ListNamed();
    }
}
