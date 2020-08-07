// <copyright file="AuditableExtensionsTester.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.UnitTests
{
    using System;
    using Core.ExtensionHelpers;
    using Domain.Model;
    using Xunit;

    public class AuditableExtensionsTester
    {
        [Theory(DisplayName = "Verify that valid username set correctly auditable properties")]
        [InlineData("Valid Username")]
        public void GivenValidUserName_AuditCreateShouldSetProperties(string userName)
        {
            IAuditable auditable = new Cv { Id = 1, CreatedBy = null, CreatedDate = default(DateTime), LastModifiedBy = null, LastModifiedDate = default(DateTime) };

            auditable.AuditCreate(userName);

            Assert.Equal(userName, auditable.CreatedBy);
            Assert.NotNull(auditable.LastModifiedBy);
            Assert.NotEqual(default(DateTime), auditable.CreatedDate);
            Assert.NotEqual(default(string), auditable.LastModifiedBy);
        }

        [Theory(DisplayName = "Verify that invalid username throws correct exception")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void GivenInvalidUsername_AuditCreateShouldThrowException(string userName)
        {
            IAuditable auditable = new Cv { Id = 1, CreatedBy = null, CreatedDate = default(DateTime), LastModifiedBy = null, LastModifiedDate = default(DateTime) };

            Exception ex = Assert.Throws<InvalidAuditableUsernameException>(() =>
                auditable.AuditCreate(userName));

            Assert.Equal(CoreExceptionConstants.InvalidAuditableUserName, ex.Message);
        }

        [Theory(DisplayName = "Verify that valid username set correctly auditable properties")]
        [InlineData("Valid Username")]
        public void GivenValidUserName_AuditModifyShouldSetProperties(string userName)
        {
            IAuditable auditable = new Cv { Id = 1, LastModifiedBy = null, LastModifiedDate = default(DateTime) };

            auditable.AuditModify(userName);

            Assert.Equal(userName, auditable.LastModifiedBy);
            Assert.NotEqual(default(DateTime), auditable.LastModifiedDate);
        }

        [Theory(DisplayName = "Verify that invalid username throws correct exception")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void GivenInvalidUsername_AuditModifyShouldThrowException(string userName)
        {
            IAuditable auditable = new Cv { Id = 1, LastModifiedBy = null, LastModifiedDate = default(DateTime) };

            Exception ex = Assert.Throws<InvalidAuditableUsernameException>(() =>
                auditable.AuditModify(userName));

            Assert.Equal(CoreExceptionConstants.InvalidAuditableUserName, ex.Message);
        }
    }
}
