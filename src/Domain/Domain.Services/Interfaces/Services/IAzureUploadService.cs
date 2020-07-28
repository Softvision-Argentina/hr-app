using Domain.Model;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Domain.Services.Interfaces.Services
{
    public interface IAzureUploadService
    {
        Task<string> Upload(IFormFile file, Candidate candidate);
    }
}
