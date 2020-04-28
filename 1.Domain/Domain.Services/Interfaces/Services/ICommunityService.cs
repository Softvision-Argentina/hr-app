using Domain.Services.Contracts.Community;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface ICommunityService
    {
        CreatedCommunityContract Create(CreateCommunityContract contract);
        ReadedCommunityContract Read(int id);
        void Update(UpdateCommunityContract contract);
        void Delete(int id);
        IEnumerable<ReadedCommunityContract> List();
    }
}
