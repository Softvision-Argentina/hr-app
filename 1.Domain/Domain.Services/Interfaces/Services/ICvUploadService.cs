using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Interfaces.Services
{
    public interface ICvUploadService
    {
        DriveService Authorize();
        Google.Apis.Drive.v3.Data.File Upload(DriveService driveService, IFormFile file);
    }
}
