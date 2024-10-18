using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using ClothingBrand.Application.Common.DTO.OrderDto;
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
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string ShippingMethod { get; set; } // Shipping type like Standard, Express

    }


}
