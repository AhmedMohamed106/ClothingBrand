using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Response.ShoppingCart
{
    public class ShoppingCartItemDto
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? imageUrl { get; set; }
        public decimal Price { get; set; } 
        public int Quantity { get; set; }

    }
}
