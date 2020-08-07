// <copyright file="TestAuthenticationExtensions.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Security.AuthenticationTest
{
    using System;
    using Microsoft.AspNetCore.Authentication;

    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuthentication(this AuthenticationBuilder builder)
        {
            return builder.AddTestAuthentication(TestAuthenticationOptions.AuthenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddTestAuthentication(
            this AuthenticationBuilder builder,
            Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddTestAuthentication(TestAuthenticationOptions.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddTestAuthentication(
            this AuthenticationBuilder builder, string authenticationScheme, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddTestAuthentication(authenticationScheme, displayName: null, configureOptions: configureOptions);
        }

        public static AuthenticationBuilder AddTestAuthentication(
            this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
