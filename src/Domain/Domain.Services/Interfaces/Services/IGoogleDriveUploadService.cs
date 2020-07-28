using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Domain.Services.Interfaces.Services
{
    public interface IGoogleDriveUploadService
    {
        Task<DriveService> Authorize();
        Google.Apis.Drive.v3.Data.File Upload(DriveService driveService, IFormFile file);
    }
}
