using Microsoft.AspNetCore.Authentication;
using System;

namespace ApiServer.Security.AuthenticationTest
{
    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder)
           => builder.AddTestAuthentication(TestAuthenticationOptions.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder,
            Action<TestAuthenticationOptions> configureOptions)
            => builder.AddTestAuthentication(TestAuthenticationOptions.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder,
            string authenticationScheme, Action<TestAuthenticationOptions> configureOptions)
            => builder.AddTestAuthentication(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder,
            string authenticationScheme, string displayName, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
