using Domain.Services.Contracts.Seed;
using System;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IDummyService
    {
        CreatedDummyContract Create(CreateDummyContract contract);
        ReadedDummyContract Read(Guid id);
        void Update(UpdateDummyContract contract);
        void Delete(Guid id);
        IEnumerable<ReadedDummyContract> List();
    }
}
