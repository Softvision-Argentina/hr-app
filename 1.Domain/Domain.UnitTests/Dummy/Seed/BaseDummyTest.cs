﻿using Core;
using Core.Persistance;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.Seed;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.UnitTests.Dummy.Builders.Dummy;

namespace Domain.Services.Impl.UnitTests.Seed
{
    public class BaseDummyTest : BaseDomainTest
    {
        protected DummyService Service;
        protected Mock<IRepository<Model.Seed.Dummy>> MockDummyRepository;
        protected MockPersistance<Model.Seed.Dummy> MockPersistance;
        protected IList<Model.Seed.Dummy> InMemoryDummies => MockPersistance.CreatedAndPersisted;

        public BaseDummyTest(AutomapperFixture automapperFixture)
        {
            SetupMockDummyRepository();
            SetupMockPersitance();
            SetupDummyInMemoryList();

            Service = new DummyService(
                automapperFixture.Mapper,
                MockDummyRepository.Object,
                MockUnitOfWork.Object,
                new Mock<ILog<DummyService>>().Object,
                new UpdateDummyContractValidator(),
                new CreateDummyContractValidator());
        }

        private void SetupDummyInMemoryList()
        {
            MockPersistance.Create(new DummyBuilder().Build());
            MockPersistance.Create(new DummyBuilder().Build());
            MockPersistance.Create(new DummyBuilder().Build());
            MockPersistance.Create(new DummyBuilder().Build());
            MockPersistance.Create(new DummyBuilder().Build());
            MockPersistance.Create(new DummyBuilder().Build());

            MockPersistance.Persist();
        }

        protected void AssertPersistanceIsCalledForCreateDummy()
        {
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
            MockDummyRepository.Verify(_ => _.Create(It.IsAny<Model.Seed.Dummy>()), Times.Once);
            MockPersistance.AssertPersited();
        }

        private void SetupMockPersitance()
        {
            MockPersistance = new MockPersistance<Model.Seed.Dummy>();

            MockUnitOfWork.Setup(_ => _.Complete())
                .Callback(() =>
                {
                    var persisted = 0;
                    persisted += MockPersistance.Persist();
                    if (persisted == 0)
                    {
                        throw new InvalidOperationException("Nothing persisted");
                    }
                })
                .Returns(MockPersistance.PersistanceCount);

            MockDummyRepository
                .Setup(_ => _.Create(It.IsAny<Model.Seed.Dummy>()))
                .Callback<Model.Seed.Dummy>(t => MockPersistance.Create(t))
                .Returns<Model.Seed.Dummy>(t => { t.Id = Guid.NewGuid(); return t; });

            MockDummyRepository
                .Setup(_ => _.QueryEager())
                .Returns(InMemoryDummies.AsQueryable());

            //MockDummyRepository
            //    .Setup(_ => _.Create(It.IsAny<Dummy>()))
            //    .Callback<Dummy>(t => MockPersistance.Create(t))
            //    .Returns<Dummy>(t => { t.Id = Guid.NewGuid(); return t; });

            //MockTaskRepository
            //    .Setup(_ => _.Update(It.IsAny<Task>()))
            //    .Callback<Task>(t => MockPersistance.Update(t))
            //    .Returns<Task>(t => t);
        }

        #region RepositoryMock
        private void SetupMockDummyRepository()
        {
            MockDummyRepository = new Mock<IRepository<Model.Seed.Dummy>>(MockBehavior.Strict);
        }
        #endregion RepositoryMock
    }
}
