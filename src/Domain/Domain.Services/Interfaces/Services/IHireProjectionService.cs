using Domain.Services.Contracts.HireProjection;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IHireProjectionService
    {
        CreatedHireProjectionContract Create(CreateHireProjectionContract contract);
        ReadedHireProjectionContract Read(int id);
        void Update(UpdateHireProjectionContract contract);
        void Delete(int id);
        IEnumerable<ReadedHireProjectionContract> List();
    }
}
