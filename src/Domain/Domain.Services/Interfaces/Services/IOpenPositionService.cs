using Domain.Model;
using Domain.Services.Contracts.OpenPositions;
using System;
using System.Collections.Generic;
namespace Domain.Services.Interfaces.Services
{
    public interface IOpenPositionService
    {
        CreatedOpenPositionContract Create(CreateOpenPositionContract openPosition);
        ReadedOpenPositionContract GetById(int id);
        IEnumerable<ReadedOpenPositionContract> Get();
        void Update(UpdateOpenPositionContract openPosition);
        void Delete(int id);
    }
}
