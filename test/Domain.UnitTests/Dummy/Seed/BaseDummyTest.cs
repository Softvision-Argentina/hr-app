// <copyright file="BaseDummyTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.Persistance;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.UnitTests.Dummy.Builders.Dummy;
    using Domain.Services.Impl.Validators.Seed;
    using Moq;

    public class BaseDummyTest : BaseDomainTest
    {
        protected DummyService service;
        protected Mock<IRepository<Model.Seed.Dummy>> mockDummyRepository;
        protected MockPersistance<Model.Seed.Dummy> mockPersistance;

        protected IList<Model.Seed.Dummy> InMemoryDummies => this.mockPersistance.CreatedAndPersisted;

        public BaseDummyTest(AutomapperFixture automapperFixture)
        {
            this.SetupMockDummyRepository();
            this.SetupMockPersitance();
            this.SetupDummyInMemoryList();

            this.service = new DummyService(
                automapperFixture.Mapper,
                this.mockDummyRepository.Object,
                this.MockUnitOfWork.Object,
                new Mock<ILog<DummyService>>().Object,
                new UpdateDummyContractValidator(),
                new CreateDummyContractValidator());
        }

        private void SetupDummyInMemoryList()
        {
            this.mockPersistance.Create(new DummyBuilder().Build());
            this.mockPersistance.Create(new DummyBuilder().Build());
            this.mockPersistance.Create(new DummyBuilder().Build());
            this.mockPersistance.Create(new DummyBuilder().Build());
            this.mockPersistance.Create(new DummyBuilder().Build());
            this.mockPersistance.Create(new DummyBuilder().Build());

            this.mockPersistance.Persist();
        }

        protected void AssertPersistanceIsCalledForCreateDummy()
        {
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
            this.mockDummyRepository.Verify(_ => _.Create(It.IsAny<Model.Seed.Dummy>()), Times.Once);
            this.mockPersistance.AssertPersited();
        }

        private void SetupMockPersitance()
        {
            this.mockPersistance = new MockPersistance<Model.Seed.Dummy>();

            this.MockUnitOfWork.Setup(_ => _.Complete())
                .Callback(() =>
                {
                    var persisted = 0;
                    persisted += this.mockPersistance.Persist();
                    if (persisted == 0)
                    {
                        throw new InvalidOperationException("Nothing persisted");
                    }
                })
                .Returns(this.mockPersistance.PersistanceCount);

            this.mockDummyRepository
                .Setup(_ => _.Create(It.IsAny<Model.Seed.Dummy>()))
                .Callback<Model.Seed.Dummy>(t => this.mockPersistance.Create(t))
                .Returns<Model.Seed.Dummy>(t =>
                {
                    t.Id = Guid.NewGuid();
                    return t;
                });

            this.mockDummyRepository
                .Setup(_ => _.QueryEager())
                .Returns(this.InMemoryDummies.AsQueryable());
        }

        #region RepositoryMock
        private void SetupMockDummyRepository()
        {
            this.mockDummyRepository = new Mock<IRepository<Model.Seed.Dummy>>(MockBehavior.Strict);
        }
        #endregion RepositoryMock
    }
}
