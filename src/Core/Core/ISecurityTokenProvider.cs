using System.Collections.Generic;
using System.Security.Claims;

namespace Core
{
    public interface ISecurityTokenProvider
    {
        string BuildSecurityToken(string userName, List<Claim> claims);
    }
}
