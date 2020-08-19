namespace ApiServer.UnitTests.Controllers
{
    using ApiServer.Controllers;
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Cv;
    using Domain.Services.Interfaces.Repositories;
    using Domain.Services.Interfaces.Services;
    using Google.Apis.Drive.v3;
    using Google.Apis.Drive.v3.Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class CvControllerTest
    {
        private CvController controller;
        private Mock<ICvService> mockCvService;
        private Mock<ICandidateService> mockCandidateService;
        private Mock<IAzureUploadService> mockAzure;

        public CvControllerTest()
        {
            this.mockCvService = new Mock<ICvService>();
            this.mockCandidateService = new Mock<ICandidateService>();
            this.mockAzure = new Mock<IAzureUploadService>();
            this.controller = new CvController(this.mockCandidateService.Object, this.mockAzure.Object, this.mockCvService.Object);
        }

        [Fact(DisplayName = "Verify that method 'AddCv' returns OkObjectResult")]
        public void Should_AddCv()
        {
            var candidateId = 0;
            var cvContract = new CvContractAdd();

            this.mockCandidateService.Setup(_ => _.GetCandidate(It.IsAny<int>())).Returns(new Candidate());
            this.mockAzure.Setup(_ => _.Upload(It.IsAny<IFormFile>(), It.IsAny<Candidate>())).ReturnsAsync(It.IsAny<string>());
            this.mockCvService.Setup(_ => _.StoreCvAndCandidateCvId(It.IsAny<Candidate>(), It.IsAny<CvContractAdd>(), It.IsAny<string>()));

            var result = this.controller.AddCv(candidateId, cvContract);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            this.mockCandidateService.Verify(_ => _.GetCandidate(It.IsAny<int>()), Times.Once);
            this.mockAzure.Verify(_ => _.Upload(It.IsAny<IFormFile>(), It.IsAny<Candidate>()), Times.Once);
            this.mockCvService.Verify(_ => _.StoreCvAndCandidateCvId(It.IsAny<Candidate>(), It.IsAny<CvContractAdd>(), It.IsAny<string>()), Times.Once);
        }
    }
}