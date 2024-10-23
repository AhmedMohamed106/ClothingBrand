using ClothingBrand.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClothingBrand.Infrastructure.File
{
   // In Infrastructure Layer (e.g., Infrastructure.Services)
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string SaveImage(IFormFile image, string folder)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            image.CopyTo(fileStream);

            return $"/{folder}/{uniqueFileName}";
        }

        public void DeleteImage(string imageUrl, string folder)
        {
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder , imageUrl.TrimStart('/'));
                try
                {   if (imageUrl != null)
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }

}
