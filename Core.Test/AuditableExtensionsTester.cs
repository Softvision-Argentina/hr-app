using Core.ExtensionHelpers;
using Domain.Model;
using System;
using Xunit;

namespace Core.Test
{
    public class AuditableExtensionsTester
    {
        [Theory(DisplayName = "Verify that valid username set correctly auditable properties")]
        [InlineData("Valid Username")]
        public void GivenValidUserName_AuditCreateShouldSetProperties(string userName)
        {
            IAuditable auditable = new Cv { Id = 1, CreatedBy = null, CreatedDate = default, LastModifiedBy = null, LastModifiedDate = default };

            auditable.AuditCreate(userName);

            Assert.Equal(userName, auditable.CreatedBy);
            Assert.NotNull(auditable.LastModifiedBy);
            Assert.NotEqual(default, auditable.CreatedDate);
            Assert.NotEqual(default, auditable.LastModifiedBy);
        }

        [Theory(DisplayName = "Verify that invalid username throws correct exception")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void GivenInvalidUsername_AuditCreateShouldThrowException(string userName)
        {
            IAuditable auditable = new Cv { Id = 1, CreatedBy = null, CreatedDate = default, LastModifiedBy = null, LastModifiedDate = default };

            Exception ex = Assert.Throws<InvalidAuditableUsernameException>(() =>
                auditable.AuditCreate(userName));

            Assert.Equal(CoreExceptionConstants.InvalidAuditableUserName, ex.Message);
        }

        [Theory(DisplayName = "Verify that valid username set correctly auditable properties")]
        [InlineData("Valid Username")]
        public void GivenValidUserName_AuditModifyShouldSetProperties(string userName)
        {
            IAuditable auditable = new Cv { Id = 1, LastModifiedBy = null, LastModifiedDate = default };

            auditable.AuditModify(userName);

            Assert.Equal(userName, auditable.LastModifiedBy);
            Assert.NotEqual(default, auditable.LastModifiedDate);
        }

        [Theory(DisplayName = "Verify that invalid username throws correct exception")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void GivenInvalidUsername_AuditModifyShouldThrowException(string userName)
        {
            IAuditable auditable = new Cv { Id = 1, LastModifiedBy = null, LastModifiedDate = default };

            Exception ex = Assert.Throws<InvalidAuditableUsernameException>(() =>
                auditable.AuditModify(userName));

            Assert.Equal(CoreExceptionConstants.InvalidAuditableUserName, ex.Message);
        }
    }
}
