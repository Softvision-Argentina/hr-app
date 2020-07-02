using Domain.Services.Contracts;
using Domain.Services.Contracts.ReaddressReason;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IReaddressReasonService
    {
        CreatedReaddressReason Create(CreateReaddressReason contract);
        ReadReaddressReason Read(int id);
        void Update(int id, UpdateReaddressReason contract);
        void Delete(int id);
        IEnumerable<ReadReaddressReason> List();
        IEnumerable<ReadReaddressReason> ListBy(ReaddressReasonSearchModel readdressReasonSearchModel);

    }
}
