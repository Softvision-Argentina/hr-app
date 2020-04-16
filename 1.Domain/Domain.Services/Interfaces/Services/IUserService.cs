using Domain.Services.Contracts.User;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IUserService
    {
        ReadedUserContract Authenticate(string username, string password);
        ReadedUserContract Authenticate(string username);
        IEnumerable<ReadedUserContract> GetAll();
        IEnumerable<ReadedUserContract> GetFilteredForTech();
        IEnumerable<ReadedUserContract> GetFilteredForHr();
        ReadedUserContract GetById(int id);
        ReadedUserRoleContract GetUserRole(string username);
    }
}
