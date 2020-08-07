// <copyright file="BaseDomainTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Dummy
{
    using Core.Persistance;
    using Moq;
    using Xunit;

    [Collection("Service Test Collection")]
    public class BaseDomainTest
    {
        protected Mock<IUnitOfWork> MockUnitOfWork { get; set; }

        public BaseDomainTest()
        {
            this.MockUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            this.MockUnitOfWork.Setup(_ => _.Complete()).Returns(1);
        }
    }
}
