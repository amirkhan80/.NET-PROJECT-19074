using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SmartServiceHub.Services
{
    public interface IImageService
    {
        Task<string?> UploadFileAsync(IFormFile file, string folder = "vehicles");
    }
}
