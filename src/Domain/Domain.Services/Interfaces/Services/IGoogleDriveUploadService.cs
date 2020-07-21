using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Http;

namespace Domain.Services.Interfaces.Services
{
    public interface IGoogleDriveUploadService
    {
        DriveService Authorize();
        Google.Apis.Drive.v3.Data.File Upload(DriveService driveService, IFormFile file);
    }
}
