using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Response
{
    public class GEtCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }


        // Navigation property
        public  List<string> Products { get; set; }
    }
}
