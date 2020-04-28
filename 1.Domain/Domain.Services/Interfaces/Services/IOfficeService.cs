using Domain.Services.Contracts.Office;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IOfficeService
    {
        CreatedOfficeContract Create(CreateOfficeContract contract);
        ReadedOfficeContract Read(int id);
        void Update(UpdateOfficeContract contract);
        void Delete(int id);
        IEnumerable<ReadedOfficeContract> List();
    }
}
