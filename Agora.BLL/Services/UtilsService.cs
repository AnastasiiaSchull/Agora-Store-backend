using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Agora.BLL.Services
{
    public class UtilsService : IUtilsService
    {
        public string GetFirstImageUrl(string? folderPath, HttpRequest request)
        {
            if (string.IsNullOrEmpty(folderPath))
                return null;

            var fullFolderPath = Path.Combine("wwwroot", folderPath);

            if (!Directory.Exists(fullFolderPath))
                return null;

            var firstImage = Directory
                .GetFiles(fullFolderPath)
                .FirstOrDefault(file =>
                    file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".webp", StringComparison.OrdinalIgnoreCase));

            if (firstImage == null)
                return null;

            var fileName = Path.GetFileName(firstImage);
            return $"{request.Scheme}://{request.Host}/{folderPath}/{fileName}";
        }

        public List<string> GetImagesUrl(string? folderPath, HttpRequest request)
        {
            if (string.IsNullOrEmpty(folderPath))
                return null;

            var fullFolderPath = Path.Combine("wwwroot", folderPath);
            if (!Directory.Exists(fullFolderPath))
                return null;

            var images = Directory
                .GetFiles(fullFolderPath)
                .Where(file =>
                    file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".webp", StringComparison.OrdinalIgnoreCase));

            if (images == null)
                return null;

            List<string> imagesUrls = new List<string>();
            foreach(var image in images)
            {
                var fileName = Path.GetFileName(image);
                string url = $"{request.Scheme}://{request.Host}/{folderPath}/{fileName}";
                imagesUrls.Add(url);
            }

            return imagesUrls;
        }
    }
}
