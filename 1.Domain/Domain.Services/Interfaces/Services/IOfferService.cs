using Domain.Services.Contracts.Offer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Interfaces.Services
{
    public interface IOfferService
    {
        CreatedOfferContract Create(CreateOfferContract contract);
        ReadedOfferContract Read(int id);
        void Update(UpdateOfferContract contract);
        void Delete(int id);
        IEnumerable<ReadedOfferContract> List();
    }
}
