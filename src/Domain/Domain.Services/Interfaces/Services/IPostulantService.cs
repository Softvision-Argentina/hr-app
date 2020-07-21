using Domain.Services.Contracts.Postulant;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IPostulantService
    {
        ReadedPostulantContract Read(int id);
        IEnumerable<ReadedPostulantContract> List();
    }
}
