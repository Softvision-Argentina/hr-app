using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Claims;

namespace ApiServer.Security.AuthenticationTest
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public static string AuthenticationScheme { get; } = "TestAuthentication";

        private static readonly string TestUserName = "AmsSvcInt";

        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new Claim[]
        {
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()),
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", Guid.NewGuid().ToString()),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", TestUserName),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", TestUserName),
            new Claim(SecurityClaims.CAN_LIST_DUMMY, "true")
        }, TestUserName);

        public TestAuthenticationOptions()
        {
        }
    }
}
