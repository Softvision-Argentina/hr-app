using Domain.Model;
using Domain.Services.Contracts;
using Domain.Services.Contracts.ReaddressReason;
using Domain.Services.Contracts.ReaddressStatus;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IReaddressStatusService
    {
        void Create(int readdressReasonId, ReaddressStatus contract);
        void Update(int readdressReasonId, ReaddressStatus contract);
    }
}
