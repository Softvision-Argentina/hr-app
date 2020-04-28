using Domain.Services.Interfaces.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Domain.Services.Impl.Services
{
    public class GoogleDriveUploadService : IGoogleDriveUploadService
    {
        //TODO: Encapsulate and divide driveservice in other interface, and File in its proper interface
        public Google.Apis.Drive.v3.Data.File Upload(DriveService driveService, IFormFile file)
        {
            if (file.Length > 0)
            {
                Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
                body.Name = Path.GetFileName(file.FileName);
                body.MimeType = GetMimeType(file.ContentType);
                var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                MemoryStream stream = new MemoryStream(fileBytes);

                FilesResource.CreateMediaUpload request = driveService.Files.Create(body, stream, GetMimeType(file.ContentType));
                request.SupportsTeamDrives = true;
                request.Upload();

                return request.ResponseBody;
            }
            
            return null;
        }

        public DriveService Authorize()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive,
                               DriveService.Scope.DriveFile,};

            var directory = GetDirectory();
            var keyFilePath = $"{directory}\\api.p12"; 
            var serviceAccountEmail = "descargar@quickstart-1584033347804.iam.gserviceaccount.com"; 

            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            var cred = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = scopes
            }.FromCertificate(certificate));


            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = cred,
                ApplicationName = "Drive API Sample",
            });

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
