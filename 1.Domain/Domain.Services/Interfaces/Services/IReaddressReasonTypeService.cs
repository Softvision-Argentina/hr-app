using Domain.Services.Contracts;
using Domain.Services.Contracts.ReaddressReason;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IReaddressReasonTypeService
    {
        CreatedReaddressReasonType Create(CreateReaddressReasonType contract);
        ReadReaddressReasonType Read(int id);
        void Update(int id, UpdateReaddressReasonType contract);
        void Delete(int id);
        IEnumerable<ReadReaddressReasonType> List();
    }
}
