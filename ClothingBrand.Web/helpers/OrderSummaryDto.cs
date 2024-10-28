using ClothingBrand.Application.Common.DTO.OrderDto;

namespace ClothingBrand.Web.helpers
{
    public class OrderSummaryDto
    {
        public int orderId { get; set; } 
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }

        // Shipping details
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public List<ItemDto> OrderItems { get; set; }

    }
}
