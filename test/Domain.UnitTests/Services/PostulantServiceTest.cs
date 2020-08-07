// <copyright file="PostulantServiceTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Contracts.Postulant;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Moq;
    using Xunit;

    public class PostulantServiceTest : BaseDomainTest
    {
        private readonly PostulantService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Postulant>> mockRepositoryPostulant;

        public PostulantServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryPostulant = new Mock<IRepository<Postulant>>();
            this.service = new PostulantService(
                this.mockMapper.Object,
                this.mockRepositoryPostulant.Object);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var postulants = new List<Postulant>() { new Postulant() { Id = 1 } }.AsQueryable();
            var readedPostulantList = new List<ReadedPostulantContract> { new ReadedPostulantContract { Id = 1 } };
            this.mockRepositoryPostulant.Setup(mrt => mrt.QueryEager()).Returns(postulants);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedPostulantContract>>(It.IsAny<List<Postulant>>())).Returns(readedPostulantList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryPostulant.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedPostulantContract>>(It.IsAny<List<Postulant>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var postulants = new List<Postulant>() { new Postulant() { Id = 1 } }.AsQueryable();
            var readedPostulant = new ReadedPostulantContract { Id = 1 };
            this.mockRepositoryPostulant.Setup(mrt => mrt.QueryEager()).Returns(postulants);
            this.mockMapper.Setup(mm => mm.Map<ReadedPostulantContract>(It.IsAny<Postulant>())).Returns(readedPostulant);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedPostulant, actualResult);
            this.mockRepositoryPostulant.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedPostulantContract>(It.IsAny<Postulant>()), Times.Once);
        }
    }
}