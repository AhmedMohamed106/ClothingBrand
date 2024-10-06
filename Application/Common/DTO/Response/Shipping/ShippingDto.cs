using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Response.Shipping
{
    public class ShippingDto
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public List<OrderDto> orders { get; set; }
    }

}
