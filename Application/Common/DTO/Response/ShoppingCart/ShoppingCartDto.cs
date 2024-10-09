using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Response.ShoppingCart
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; }
        public List<ShoppingCartItemDto> ShoppingCartItems { get; set; }
    }
}
