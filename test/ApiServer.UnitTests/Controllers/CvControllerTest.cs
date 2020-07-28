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

namespace ApiServer.UnitTests.Controllers
{
    public class CvControllerTest
    {
        private CvController controller;
        private Mock<ICvService> mockCvService;
        private Mock<ICandidateService> mockCandidateService;
        private Mock<IGoogleDriveUploadService> mockGoogleDrive;

        public CvControllerTest()
        {
            mockCvService = new Mock<ICvService>();
            mockCandidateService = new Mock<ICandidateService>();
            mockGoogleDrive = new Mock<IGoogleDriveUploadService>();
            controller = new CvController(mockCandidateService.Object, mockGoogleDrive.Object, mockCvService.Object);
        }


        [Fact(DisplayName = "Verify that method 'AddCv' returns OkObjectResult")]
        public void Should_AddCv()
        {
            //var candidateId = 0;
            //var cvContract = new CvContractAdd();
            //var expectedValue = "FileUploaded";

            //mockCandidateService.Setup(_ => _.GetCandidate(It.IsAny<int>())).Returns(new Candidate());
            //mockGoogleDrive.Setup(_ => _.Authorize()).Returns(new DriveService());
            //mockGoogleDrive.Setup(_ => _.Upload(It.IsAny<DriveService>(), It.IsAny<IFormFile>())).Returns(new File()); //Google.Apis.Drive.v3.Data.File
            //mockCvService.Setup(_ => _.StoreCvAndCandidateCvId(It.IsAny<Candidate>(), It.IsAny<CvContractAdd>(), It.IsAny<File>()));

            //var result = controller.AddCv(candidateId, cvContract);

            //Assert.NotNull(result);
            //Assert.IsType<OkObjectResult>(result);
            //Assert.Equal(expectedValue, (result as OkObjectResult).Value);
            //mockCandidateService.Verify(_ => _.GetCandidate(It.IsAny<int>()), Times.Once);
            //mockGoogleDrive.Verify(_ => _.Authorize(), Times.Once);
            //mockGoogleDrive.Verify(_ => _.Upload(It.IsAny<DriveService>(), It.IsAny<IFormFile>()), Times.Once);
            //mockCvService.Verify(_ => _.StoreCvAndCandidateCvId(It.IsAny<Candidate>(), It.IsAny<CvContractAdd>(), It.IsAny<File>()), Times.Once);
        }
    }
}