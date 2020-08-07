namespace Domain.Services.Impl.Services
{
    using System.IO;
    using System.Threading.Tasks;
    using Domain.Model;
    using Domain.Services.Interfaces.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class AzureUploadService : IAzureUploadService
    {
        private readonly IConfiguration configuration;

        public AzureUploadService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> Upload(IFormFile file, Candidate candidate)
        {
            var strorageconn = this.configuration.GetValue<string>("AzureStorage");
            CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

            CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("contcvstore");

            var hash = file.GetHashCode() % 10000;
            var hashForFile = hash.ToString("0000");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"CV-{candidate.Name}-{candidate.LastName}-{hashForFile}.pdf");

            var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            await blockBlob.UploadFromStreamAsync(stream);

            var name = blockBlob.Uri.ToString();

            return name;
        }
    }
}
