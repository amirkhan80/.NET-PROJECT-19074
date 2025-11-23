using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SmartServiceHub.Services
{
    public class ImageService : IImageService
    {
        public async Task<string?> UploadFileAsync(IFormFile file, string folder = "vehicles")
        {
            if (file == null || file.Length == 0)
                return null;

            // ✅ Render/Linux-safe absolute folder path
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(rootPath, folder);

            // ✅ Create directory if missing
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // ✅ Generate unique file name
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // ✅ Save image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // ✅ Return URL path for API / frontend
            return $"/{folder}/{fileName}";
        }
    }
}
