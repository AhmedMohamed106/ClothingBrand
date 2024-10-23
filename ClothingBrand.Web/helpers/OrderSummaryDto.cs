using ClothingBrand.Application.Common.DTO.OrderDto;

namespace ClothingBrand.Web.helpers
{
    public class OrderSummaryDto
    {
<<<<<<< HEAD
        public int Id { get; set; }
=======
        public int orderId { get; set; } 
>>>>>>> origin/Mekawy
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
