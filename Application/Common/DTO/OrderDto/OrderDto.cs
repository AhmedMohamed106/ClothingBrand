using ClothingBrand.Application.Common.DTO.Response.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.OrderDto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string UserId { get; set; }
        public ShippingDto ShippingDetails { get; set; } // Include Shipping details
        public List<OrderItemDto> OrderItems { get; set; }

        public OrderDto()
        {
            OrderItems = new List<OrderItemDto>();
        }

    }
}
