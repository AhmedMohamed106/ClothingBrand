using ClothingBrand.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Request
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
        public int StockQuantity { get; set; }
        public string ISBN { get; set; }


        public int CategoryId { get; set; }

        public int DiscountId { get; set; }

    }
}
