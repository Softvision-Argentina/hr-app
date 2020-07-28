using Domain.Services.Interfaces.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v3.Data.File;
using Google.Apis.Logging;
using Core;
using System.Threading.Tasks;

namespace Domain.Services.Impl.Services
{
    public class GoogleDriveUploadService : IGoogleDriveUploadService
    {
        private readonly ILog<GoogleDriveUploadService> _log;
        public GoogleDriveUploadService(ILog<GoogleDriveUploadService> logger)
        {
            _log = logger;
        }

        public File Upload(DriveService service, IFormFile file)
        {
            _log.LogInformation("Inicio: GoogleDriveUploadService.Upload");

            string fileName = Path.GetRandomFileName();
            fileName = fileName.Replace(".", "");
            fileName.Substring(0, 8); 
            fileName = file.FileName + " " + fileName;

            string folderId = "1KMcb74YrmMd-j3BVNWyC9L9uf5Czivbs";
            var fileMetadata = new File()
                {
                    Name = fileName,
                    Parents = new List<string>() { folderId } //FOLDER 
                };

                FilesResource.CreateMediaUpload requestAdd;
                var stream = new MemoryStream();
                file.CopyTo(stream);
                requestAdd = service.Files.Create(fileMetadata, stream, "application/pdf");
                requestAdd.Fields = "id";
                _log.LogInformation("pre upload: GoogleDriveUploadService.Upload linea 48");
                var response = requestAdd.Upload();
            
            _log.LogError($"request upload response exception: {response?.Exception?.Message ?? "no-exception"}");

            FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = 100;
                listRequest.Fields = "nextPageToken, files(id, name, webViewLink)";
                IList<File> files = listRequest.Execute().Files;

                var webViewLink = files.First(x => x.Name == fileName);

                stream.Close();

            _log.LogInformation("Fin: GoogleDriveUploadService.Upload");

            return webViewLink;
        }

        public async Task<DriveService> Authorize()
        {
            _log.LogInformation("Inicio: GoogleDriveUploadService.Authorize Linea 68");

            string[] Scopes = { DriveService.Scope.DriveFile, DriveService.Scope.Drive };
            string ApplicationName = "ReporteSV";

            var directory = GetDirectory();
            var keyFilePath = $@"D:\home\site\wwwroot\credentials.json"; 
            var credPath = $"{directory}\\token.json";
            _log.LogInformation($"CHECK KEYFILEPATH {keyFilePath}");
            _log.LogInformation("LLEGO HASTA POST CHECKFILEPATH");
            UserCredential credential;

            var prueba = new FileStream(keyFilePath, FileMode.Open, FileAccess.Read);
             _log.LogInformation($"{prueba}");

            using (var stream = new FileStream(keyFilePath, FileMode.Open, FileAccess.Read))
            {
                _log.LogInformation("LLEGO HASTA POST CHECKFILEPATH linea 83");
                try
                {
                    credential = (await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None));
                }
                catch
                {
                    _log.LogInformation($"ENTRO AL CATCH");
                    throw new Exception("No se logueo");
                }
                //new FileDataStore(credPath, true)).Result;
                _log.LogInformation($"check cred {credential}");

            }
            _log.LogInformation("En el medio del logue: Linea 87");

            var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

            _log.LogInformation("Fin: GoogleDriveUploadService.Authorize");

            return service;

        }

        private static string GetMimeType(string fileName)
        {
            var mimeType = "application/unknown";
            var ext = Path.GetExtension(fileName).ToLower();
            var regKey = Registry.ClassesRoot.OpenSubKey(ext);

            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            System.Diagnostics.Debug.WriteLine(mimeType); 

            return mimeType;
        }

        private string GetDirectory()
        {
            var directory = AppContext.BaseDirectory;
            var dirSplit = directory.Split("\\bin");
            var dirNeeded = dirSplit[0];

            return dirNeeded;
        }
    }
}
