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

namespace Domain.Services.Impl.Services
{
    public class GoogleDriveUploadService : IGoogleDriveUploadService
    {
        public File Upload(DriveService service, IFormFile file)
        {
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
                var response = requestAdd.Upload();

                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = 100;
                listRequest.Fields = "nextPageToken, files(id, name, webViewLink)";
                IList<File> files = listRequest.Execute().Files;

                var webViewLink = files.First(x => x.Name == fileName);

                stream.Close();

                return webViewLink;


        }

        public DriveService Authorize()
        {
            string[] Scopes = { DriveService.Scope.DriveFile, DriveService.Scope.Drive };
            string ApplicationName = "ReporteSV";

            var directory = GetDirectory();
            var keyFilePath = $"{directory}\\credentials.json"; 
            var credPath = $"{directory}\\token.json"; 

            UserCredential credential;

            using (var stream = new FileStream(keyFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
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
