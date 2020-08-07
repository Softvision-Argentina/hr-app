namespace Domain.Services.Interfaces.Services
{
    using System.Threading.Tasks;
    using Domain.Model;
    using Microsoft.AspNetCore.Http;

    public interface IAzureUploadService
    {
        Task<string> Upload(IFormFile file, Candidate candidate);
    }
}
