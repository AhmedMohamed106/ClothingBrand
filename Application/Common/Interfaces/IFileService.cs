using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.Interfaces
{
    // In Application Layer (e.g., Application.Common.Interfaces)
    public interface IFileService
    {
        string SaveImage(IFormFile imageFile, string folder);
        void DeleteImage(string imageUrl, string folder);

    }

}
