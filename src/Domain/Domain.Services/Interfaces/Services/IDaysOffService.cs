using Domain.Services.Contracts.DaysOff;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IDaysOffService
    {
        CreatedDaysOffContract Create(CreateDaysOffContract contract);
        ReadedDaysOffContract Read(int id);
        IEnumerable<ReadedDaysOffContract> ReadByDni(int dni);
        void Update(UpdateDaysOffContract contract);
        void AcceptPetition(int id);
        void Delete(int id);
        IEnumerable<ReadedDaysOffContract> List();
    }
}
