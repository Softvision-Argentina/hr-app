// <copyright file="IGoogleDriveUploadService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using Google.Apis.Drive.v3;
    using Microsoft.AspNetCore.Http;

    public interface IGoogleDriveUploadService
    {
        DriveService Authorize();

        Google.Apis.Drive.v3.Data.File Upload(DriveService driveService, IFormFile file);
    }
}
