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
    }
}