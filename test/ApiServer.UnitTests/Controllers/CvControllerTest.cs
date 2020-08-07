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
        private Mock<IGoogleDriveUploadService> mockGoogleDrive;

        public CvControllerTest()
        {
            this.mockCvService = new Mock<ICvService>();
            this.mockCandidateService = new Mock<ICandidateService>();
            this.mockGoogleDrive = new Mock<IGoogleDriveUploadService>();
            this.controller = new CvController(this.mockCandidateService.Object, this.mockGoogleDrive.Object, this.mockCvService.Object);
        }

        [Fact(DisplayName = "Verify that method 'AddCv' returns OkObjectResult")]
        public void Should_AddCv()
        {
            var candidateId = 0;
            var cvContract = new CvContractAdd();
            var expectedValue = "FileUploaded";

            this.mockCandidateService.Setup(_ => _.GetCandidate(It.IsAny<int>())).Returns(new Candidate());
            this.mockGoogleDrive.Setup(_ => _.Authorize()).Returns(new DriveService());
            this.mockGoogleDrive.Setup(_ => _.Upload(It.IsAny<DriveService>(), It.IsAny<IFormFile>())).Returns(new File()); // Google.Apis.Drive.v3.Data.File
            this.mockCvService.Setup(_ => _.StoreCvAndCandidateCvId(It.IsAny<Candidate>(), It.IsAny<CvContractAdd>(), It.IsAny<File>()));

            var result = this.controller.AddCv(candidateId, cvContract);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedValue, (result as OkObjectResult).Value);
            this.mockCandidateService.Verify(_ => _.GetCandidate(It.IsAny<int>()), Times.Once);
            this.mockGoogleDrive.Verify(_ => _.Authorize(), Times.Once);
            this.mockGoogleDrive.Verify(_ => _.Upload(It.IsAny<DriveService>(), It.IsAny<IFormFile>()), Times.Once);
            this.mockCvService.Verify(_ => _.StoreCvAndCandidateCvId(It.IsAny<Candidate>(), It.IsAny<CvContractAdd>(), It.IsAny<File>()), Times.Once);
        }
    }
}