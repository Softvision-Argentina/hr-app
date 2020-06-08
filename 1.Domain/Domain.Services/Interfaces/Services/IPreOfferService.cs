using Domain.Services.Contracts.PreOffer;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IPreOfferService
    {
        CreatedPreOfferContract Create(CreatePreOfferContract contract);
        ReadedPreOfferContract Read(int id);
        void Update(UpdatePreOfferContract contract);
        void Delete(int id);
        IEnumerable<ReadedPreOfferContract> List();
    }
}
