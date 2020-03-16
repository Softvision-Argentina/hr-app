using AutoMapper;
using Domain.Services.Contracts.Cv;
using Microsoft.AspNetCore.Mvc;
using Domain.Services.Interfaces.Services;
using Domain.Services.Interfaces.Repositories;
using System;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.Threading;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using Domain.Model;

namespace ApiServer.Controllers
{
    [Route("api/[controller]/{candId}")]
    [ApiController]
    public class CvController : ControllerBase
    {
        private readonly ICvRepository _repo;
        private readonly IMapper _mapper;
        ICandidateService _candidateService;

        public CvController(ICvRepository repo, ICandidateService candidateService, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _candidateService = candidateService;
        }

        [HttpPost]
        public IActionResult AddCv(int candId, [FromForm] CvContractAdd cvContract)
        {
            var candidate = _candidateService.GetCandidate(candId);
            var file = cvContract.File;

            var fileUploaded = Upload(Authorize(), file);

            cvContract.UrlId = fileUploaded.Id;
            cvContract.CandidateId = candidate.Id;
            candidate.Cv = cvContract.UrlId;

            var cv = _mapper.Map<Cv>(cvContract);
            var cand = _mapper.Map<Candidate>(candidate);
            _repo.SaveAll(cv);

            return Ok("FileUploaded");
        }

        private Google.Apis.Drive.v3.Data.File Upload(DriveService driveService, IFormFile file)
        {
            if (file.Length > 0)
            {
                Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
                body.Name = System.IO.Path.GetFileName(file.FileName);
                body.MimeType = GetMimeType(file.ContentType);
                var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                MemoryStream stream = new MemoryStream(fileBytes);
                try
                {
                    FilesResource.CreateMediaUpload request = driveService.Files.Create(body, stream, GetMimeType(file.ContentType));
                    request.SupportsTeamDrives = true;
                    request.Upload();

                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private DriveService Authorize()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive,
                               DriveService.Scope.DriveFile,};
            var clientId = "976816609478-on2g8r4gaqrrb6bj8g1dfbp04ilbl0fk.apps.googleusercontent.com";
            var clientSecret = "4JIOit7VKABsl7DxloQlO1fE";

            //var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            //{
            //    ClientId = clientId,
            //    ClientSecret = clientSecret
            //}, scopes,
            //Environment.UserName, CancellationToken.None, new FileDataStore("MyAppsToken")).Result;

            UserCredential credential =
            GoogleWebAuthorizationBroker
                          .AuthorizeAsync(new ClientSecrets
                          {
                              ClientId = clientId,
                              ClientSecret = clientSecret
                          }
                                          , scopes
                                          , Environment.UserName
                                          , CancellationToken.None
                                          , new FileDataStore("Daimto.GoogleDrive.Auth.Store")
                                          ).Result;


            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MyAppName",

            });
            service.HttpClient.Timeout = TimeSpan.FromMinutes(100);

            return service;
        }


        private static string GetMimeType(string fileName)
        { 
            string mimeType = "application/unknown"; 
            string ext = System.IO.Path.GetExtension(fileName).ToLower(); 
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext); 
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString(); 
            System.Diagnostics.Debug.WriteLine(mimeType); return mimeType; 
        }
    }
}
